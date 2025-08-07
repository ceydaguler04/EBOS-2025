using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using Microsoft.Web.WebView2.WinForms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace EBOS
{
    public partial class HaritaForm : Form
    {
        private TextBox txtAdres;
        private Button btnAra;
        private WebView2 webHarita;
        private Button btnSec;
        public string? SecilenKonumUrl { get; private set; }
        public int? SecilenMekanID { get; private set; } // ✅ Yeni eklendi
        public string? SecilenMekanAdi { get; private set; }
        public string HaritaUrl { get; set; }

        public string SecilenSehirAdi { get; set; }
        public string SecilenIlceAdi { get; set; }
        public string SecilenSemt { get; set; }
        public double? Enlem { get; set; }
        public double? Boylam { get; set; }

        public HaritaForm(string url)
        {
            InitializeComponent(); 
            HaritaUrl = url;
            InitUI();
        }

        private async void InitUI()
        {
            this.Text = "Harita";
            this.Size = new Size(1200, 900);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtAdres = new TextBox
            {
                PlaceholderText = "Adres arayın",
                Location = new Point(20, 20),
                Width = 1000,
                Font = new Font("Segoe UI", 10)
            };

            btnAra = new Button
            {
                Text = "Ara",
                Location = new Point(1030, 18),
                Width = 120,
                Height = 28,
                Font = new Font("Segoe UI", 9)
            };
            btnAra.Click += BtnAra_Click;

            webHarita = new WebView2
            {
                Location = new Point(20, 60),
                Size = new Size(1130, 720)
            };

            btnSec = new Button
            {
                Text = "Seç ve Kaydet",
                Width = 1130,
                Height = 50,
                Location = new Point(20, 800),
                BackColor = Color.MediumBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnSec.Click += BtnSec_Click;

            this.Controls.Add(txtAdres);
            this.Controls.Add(btnAra);
            this.Controls.Add(webHarita);
            this.Controls.Add(btnSec);

            await webHarita.EnsureCoreWebView2Async(null);

            // 🔥 Harita yüklenince URL'yi git
            if (!string.IsNullOrWhiteSpace(HaritaUrl))
                webHarita.Source = new Uri(HaritaUrl);
            else
                webHarita.Source = new Uri("https://www.google.com/maps");
        }

        private void BtnAra_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                string adres = txtAdres.Text.Replace(',', '.'); // 🔧 önemli satır
                adres = Uri.EscapeDataString(adres);
                string url = $"https://www.google.com/maps/search/{adres}";
                webHarita.Source = new Uri(url);
            }
        }
        private async void BtnSec_Click(object? sender, EventArgs e)
        {
            var url = webHarita.Source.ToString();

            if (string.IsNullOrEmpty(url) || url == "https://www.google.com/maps")
            {
                MessageBox.Show("Lütfen harita üzerinden bir konum seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Eğer kullanıcı konumu seçtiyse URL'den mekan adını çek
            if (url.Contains("/place/"))
            {
                try
                {
                    int start = url.IndexOf("/place/") + 7;
                    int end = url.IndexOf("/", start);
                    if (end > start)
                    {
                        string rawName = url.Substring(start, end - start);
                        SecilenMekanAdi = Uri.UnescapeDataString(rawName.Replace("+", " "));
                    }
                }
                catch
                {
                    SecilenMekanAdi = "Ad alınamadı";
                }
            }

            if (url.Contains("@"))
            {
                try
                {
                    int start = url.IndexOf("@") + 1;
                    int end = url.IndexOf("z", start);
                    string coordPart = url.Substring(start, end - start);
                    string[] parts = coordPart.Split(',');

                    if (parts.Length >= 2)
                    {
                        double lat = double.Parse(parts[0], CultureInfo.InvariantCulture);
                        double lng = double.Parse(parts[1], CultureInfo.InvariantCulture);

                        this.Enlem = lat;
                        this.Boylam = lng;

                        // 🔐 Google Geocoding API
                        string apiKey = "AIzaSyA5OXT7GB-HwL-bf8SHIZOE23hTtDfgBXU";
                        string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}&key={apiKey}";

                        string adres = "";
                        string sehir = "";
                        string ilce = "";
                        string semt = "";
                        string mekanAdi = "";

                        using (HttpClient client = new HttpClient())
                        {
                            var response = await client.GetStringAsync(requestUrl);
                            dynamic result = JsonConvert.DeserializeObject(response);

                            if (result.status != "OK")
                            {
                                MessageBox.Show("Konum bilgisi alınamadı: " + result.status);
                                return;
                            }

                            var firstResult = result.results[0];
                            adres = firstResult.formatted_address;

                            foreach (var component in firstResult.address_components)
                            {
                                var types = component.types.ToObject<List<string>>();

                                if (types.Contains("administrative_area_level_1")) // Şehir
                                    sehir = component.long_name;
                                else if (types.Contains("administrative_area_level_2")) // İlçe
                                    ilce = component.long_name;
                                else if (types.Contains("sublocality") || types.Contains("neighborhood")) // Semt
                                    semt = component.long_name;
                            }
                        }

                        // 🌍 Harita URL'ini sonradan kullanmak istersen
                        this.SecilenKonumUrl = $"https://www.google.com/maps/place/{lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}";

                        // 💾 Veritabanı işlemleri
                        using (var db = new AppDbContext())
                        {
                            var dbSehir = db.Sehirler.FirstOrDefault(s => s.SehirAdi == sehir);
                            if (dbSehir == null)
                            {
                                dbSehir = new Sehir { SehirAdi = sehir };
                                db.Sehirler.Add(dbSehir);
                                db.SaveChanges();
                            }

                            var dbIlce = db.Ilceler.FirstOrDefault(i => i.IlceAdi == ilce && i.SehirID == dbSehir.SehirID);
                            if (dbIlce == null)
                            {
                                dbIlce = new Ilce { IlceAdi = ilce, SehirID = dbSehir.SehirID };
                                db.Ilceler.Add(dbIlce);
                                db.SaveChanges();
                            }
                            var varMi = db.Mekanlar.FirstOrDefault(m =>
                                Math.Abs(Convert.ToDouble(m.Enlem) - this.Enlem.GetValueOrDefault()) < 0.0001 &&
                                Math.Abs(Convert.ToDouble(m.Boylam) - this.Boylam.GetValueOrDefault()) < 0.0001);

                            if (varMi != null)
                            {
                                this.SecilenMekanID = varMi.MekanID;
                                MessageBox.Show("Bu konum zaten kayıtlı. Mevcut mekan seçildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                return;
                            }

                            var mekan = db.Mekanlar.FirstOrDefault(m => m.Ad == mekanAdi && m.IlceID == dbIlce.IlceID);
                            if (mekan == null)
                            {
                                mekan = new Mekan
                                {
                                    Ad = this.SecilenMekanAdi,
                                    Sehir = sehir,
                                    Ilce = ilce,
                                    Semt = semt,
                                    Enlem = this.Enlem,
                                    Boylam = this.Boylam,
                                    IlceID = dbIlce.IlceID,
                                    Adres = adres
                                };

                                db.Mekanlar.Add(mekan);
                                db.SaveChanges();
                            }

                            this.SecilenMekanID = mekan.MekanID;
                        }

                        MessageBox.Show($"Konum başarıyla seçildi:\nŞehir: {sehir}\nİlçe: {ilce}\nSemt: {semt}");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    string detay = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show("Hata oluştu: " + detay);
                }
            }
        }
        private void HaritaForm_Load(object sender, EventArgs e)
        {

        }
    }
}