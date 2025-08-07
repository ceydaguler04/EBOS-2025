using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;

namespace EBOS
{
    public partial class EtkinliklerKontrol : UserControl
    {
        private int? aktifKullaniciId;
        private string rol;

        private Label lblBaslik;
        private Guna2TextBox txtArama;
        private Guna2Button btnYeniEkle;
        private Guna2Button btnApiVeriGetir;
        private FlowLayoutPanel flpKartlar;

        public EtkinliklerKontrol(int? kullaniciId = null, string rol = null)
        {
            InitializeComponent();
            this.Load += EtkinliklerKontrol_Load;
            this.Dock = DockStyle.Fill;

            this.aktifKullaniciId = kullaniciId;
            this.rol = rol;
            ArayuzOlustur();
            EtkinlikKartlariniOlustur();
        }

        private void ArayuzOlustur()
        {
            this.BackColor = TemaYonetici.AktifTema == "Koyu" ? Color.FromArgb(120, 120, 120) : Color.White;

            lblBaslik = new Label()
            {
                Text = "ETKİNLİKLER",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            txtArama = new Guna2TextBox()
            {
                PlaceholderText = "Etkinlik adına göre ara",
                Size = new Size(320, 40),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                BorderRadius = 10
            };
            txtArama.TextChanged += (s, e) =>
            {
                EtkinlikKartlariniOlustur(txtArama.Text);
            };

            this.Controls.Add(txtArama);

            btnYeniEkle = new Guna2Button()
            {
                Text = "+ Yeni Etkinlik Ekle",
                Size = new Size(200, 40),
                Location = new Point(370, 80),
                BorderRadius = 10,
                FillColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnYeniEkle.Click += btnYeniEtkinlikEkle_Click;
            this.Controls.Add(btnYeniEkle);
            
            if (!string.IsNullOrEmpty(rol) && rol.ToLower() == "yönetici")
            {
                btnApiVeriGetir = new Guna2Button()
                {
                    Text = "API'den Getir",
                    Size = new Size(150, 40),
                    Location = new Point(580, 80), // Yeni Etkinlik Ekle'nin sağında olsun
                    BorderRadius = 10,
                    FillColor = Color.DarkOrange,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btnApiVeriGetir.Click += BtnApiVeriGetir_Click;
                this.Controls.Add(btnApiVeriGetir);
            }

            flpKartlar = new FlowLayoutPanel()
            {
                Location = new Point(5, 140),
                Size = new Size(this.Width - 60, this.Height - 180),
                AutoScroll = true,
                WrapContents = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(flpKartlar);
            flpKartlar.FlowDirection = FlowDirection.LeftToRight;
            flpKartlar.WrapContents = true;
        }
        private void EtkinlikKartlariniOlustur(string filtre = "")
        {
            flpKartlar.Controls.Clear();
            using (var db = new AppDbContext())
            {

                var etkinlikler = db.Etkinlikler
                    .Include(e => e.EtkinlikTuru)
                    .OrderByDescending(e => e.Tarih)
                    .Take(50)
                    .ToList();
                // filtre boş değilse, filtrele
                if (!string.IsNullOrWhiteSpace(filtre))
                {
                    etkinlikler = etkinlikler
                        .Where(e => e.EtkinlikAdi.ToLower().Contains(filtre.ToLower()))
                        .ToList();
                }

                foreach (var etkinlik in etkinlikler)
                {
                    Guna2Panel kart = new Guna2Panel();
                    kart.Size = new Size(240, 330/*300, 350*/);
                    kart.BorderRadius = 15;
                    kart.FillColor = Color.White;
                    kart.ShadowDecoration.Enabled = true;
                    kart.ShadowDecoration.Depth = 10;
                    kart.Margin = new Padding(8);

                    PictureBox pb = new PictureBox();
                    try
                    {
                        pb.Load(etkinlik.GorselYolu); // API'den gelen URL'yi direkt yükler
                    }
                    catch
                    {
                        pb.Image = null;
                    }

                    pb.Size = new Size(230, 140/*280, 150*/);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Location = new Point(10, 10);
                    kart.Controls.Add(pb);

                    Label lblAd = new Label();
                    lblAd.Text = etkinlik.EtkinlikAdi;
                    lblAd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    lblAd.Location = new Point(10, 170);
                    lblAd.AutoSize = true;
                    kart.Controls.Add(lblAd);

                    Label lblTur = new Label();
                    lblTur.Text = $"Tür: {etkinlik.EtkinlikTuru.TurAdi} | Süre: {etkinlik.SureDakika} dk";
                    lblTur.Font = new Font("Segoe UI", 9);
                    lblTur.Location = new Point(10, 200);
                    lblTur.AutoSize = true;
                    kart.Controls.Add(lblTur);

                    Label lblTarih = new Label();
                    lblTarih.Text = $"📅 {etkinlik.Tarih:dd.MM.yyyy} ⏰ {etkinlik.Saat:hh\\:mm}";
                    lblTarih.Font = new Font("Segoe UI", 9, FontStyle.Italic);
                    lblTarih.ForeColor = Color.Gray;
                    lblTarih.Location = new Point(10, 230);
                    lblTarih.AutoSize = true;
                    kart.Controls.Add(lblTarih);

                    Guna2Button btnDuzenle = new Guna2Button();
                    btnDuzenle.Text = "Düzenle";
                    btnDuzenle.Size = new Size(100, 30);
                    btnDuzenle.FillColor = Color.DodgerBlue;
                    btnDuzenle.ForeColor = Color.White;
                    btnDuzenle.Location = new Point(10, 270);
                    kart.Controls.Add(btnDuzenle);

                    Guna2Button btnSil = new Guna2Button();
                    btnSil.Text = "Sil";
                    btnSil.Size = new Size(100, 30);
                    btnSil.FillColor = Color.Crimson;
                    btnSil.ForeColor = Color.White;
                    btnSil.Location = new Point(120, 270);
                    kart.Controls.Add(btnSil);

                    flpKartlar.Controls.Add(kart);
                }
            }
        }
        private void btnYeniEtkinlikEkle_Click(object sender, EventArgs e)
        {
            var form = new EtkinlikEkleForm(); // null değilse
            form.ShowDialog();
        }

        private async void BtnApiVeriGetir_Click(object sender, EventArgs e)
        {
            var eslesmeyenKategoriler = new HashSet<string>();
            var eslesmeyenMekanlar = new List<string>();

            var tumEtkinlikler = await Helpers.EtkinlikApiYardimcisi.TumEtkinlikleriGetirAsync();

            if (tumEtkinlikler.Count == 0)
            {
                MessageBox.Show("API'den hiç etkinlik verisi gelmedi.");
                return;
            }

            int skipTur = 0, skipMekan = 0, skipIlce = 0, eklendi = 0, zatenVardi = 0;

            var kategoriMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "klasik müzik", "konser" }, { "pop müzik", "konser" }, { "alternatif müzik", "konser" },
                { "parti & canlı müzik", "konser" }, { "caz müzik", "konser" },
                { "aşçılık ve mutfak", "workshop" },
                { "tiyatro ve gösteriler", "tiyatro" }, { "dans ve müzikal gösteriler", "konser" },
                { "çocuk", "seminer" }, { "çocuk gelişimi", "seminer" },
                { "diğer", "seminer" }, { "sanat", "seminer" },
                { "sinema", "sinema" }, { "konser", "konser" },
                { "workshop", "workshop" }, { "seminer", "seminer" }, { "tiyatro", "tiyatro" },
                { "rock müzik", "konser" },
                { "dünya müzik", "konser" },
                { "türk sanat - halk müziği", "konser" },
                { "çocuk tiyatrosu", "tiyatro" },
                { "turizm", "seminer" },
                { "eğitim - öğretim", "seminer" }

            };

            using (var db = new AppDbContext())
            {
                foreach (var item in tumEtkinlikler)
                {
                    if (!DateTime.TryParse(item.Start, out DateTime parsedTarih))
                        continue;

                    if (db.Etkinlikler.Any(x => x.EtkinlikAdi == item.EtkinlikAdi && x.Tarih == parsedTarih))
                    {
                        zatenVardi++;
                        continue;
                    }

                    var gelenKategori = (item.Category?.Name ?? "").Trim().ToLower();
                    kategoriMap.TryGetValue(gelenKategori, out string eslesenKategori);

                    if (string.IsNullOrWhiteSpace(eslesenKategori))
                    {
                        eslesmeyenKategoriler.Add(gelenKategori);
                        skipTur++;
                        continue;
                    }

                    var tur = db.EtkinlikTurleri.FirstOrDefault(t => t.TurAdi.Trim().ToLower() == eslesenKategori.ToLower());
                    if (tur == null) { skipTur++; continue; }
                    int turId = tur.TurID;

                    int venueId = item.Venue?.Id ?? 0;
                    if (venueId == 0) { skipMekan++; continue; }

                    var apiMekan = await Helpers.EtkinlikApiYardimcisi.MekanGetirAsync(venueId);
                    if (apiMekan == null)
                    {
                        eslesmeyenMekanlar.Add($"ID: {venueId} - Etkinlik: {item.EtkinlikAdi}");
                        skipMekan++;
                        continue;
                    }

                    var mekan = db.Mekanlar.FirstOrDefault(m => m.MekanApiId == apiMekan.Id);
                    if (mekan == null)
                    {
                        var yeniMekan = new Mekan
                        {
                            Ad = apiMekan.Name,
                            Adres = apiMekan.Address,
                            Sehir = apiMekan.City?.Name,
                            Ilce = apiMekan.District?.Name,
                            Semt = apiMekan.Neighborhood?.Name ?? "-",
                            Enlem = double.TryParse(apiMekan.Lat, out double lat) ? lat : null,
                            Boylam = double.TryParse(apiMekan.Lng, out double lng) ? lng : null,
                            MekanApiId = apiMekan.Id
                        };

                        db.Mekanlar.Add(yeniMekan);
                        db.SaveChanges();
                        mekan = yeniMekan;
                    }
                    int? mekanId = mekan?.MekanID;

                    int? ilceId = null;
                    var ilceAdi = apiMekan?.District?.Name;
                    if (!string.IsNullOrWhiteSpace(ilceAdi))
                    {
                        var ilce = db.Ilceler.FirstOrDefault(i => i.IlceAdi.ToLower() == ilceAdi.ToLower());
                        if (ilce != null)
                            ilceId = ilce.IlceID;
                        else
                            skipIlce++;
                    }
                    else skipIlce++;

                    int sureDakika = 120;
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Start) && !string.IsNullOrEmpty(item.End))
                        {
                            var baslangic = DateTime.Parse(item.Start);
                            var bitis = DateTime.Parse(item.End);
                            sureDakika = (int)(bitis - baslangic).TotalMinutes;
                        }
                    }
                    catch { }

                    db.Etkinlikler.Add(new Etkinlik
                    {
                        EtkinlikAdi = item.EtkinlikAdi,
                        Aciklama = item.Aciklama,
                        SureDakika = sureDakika,
                        TurID = turId,
                        Tarih = parsedTarih,
                        Saat = parsedTarih.TimeOfDay,
                        GorselYolu = item.GorselUrl ?? "",
                        IlceID = ilceId,
                        MekanID = mekanId,
                        KullaniciID = aktifKullaniciId ?? 2
                    });

                    eklendi++;
                }

                var kayit = db.SaveChanges();
                MessageBox.Show(
                    $"Kaydedilen: {eklendi}\n" +
                    $"SaveChanges dönen: {kayit}\n" +
                    $"Zaten vardı: {zatenVardi}\n" +
                    $"Tür bulunamadı: {skipTur}\n" +
                    $"Mekan bulunamadı: {skipMekan}\n" +
                    $"İlçe bulunamadı/boş: {skipIlce}"
                );

                if (eslesmeyenKategoriler.Count > 0)
                    MessageBox.Show("Eşleşmeyen Kategoriler:\n\n" + string.Join("\n", eslesmeyenKategoriler));

                if (eslesmeyenMekanlar.Count > 0)
                    MessageBox.Show("Eşleşmeyen Mekanlar:\n\n" + string.Join("\n", eslesmeyenMekanlar));
            }

            EtkinlikKartlariniOlustur();
        }

        private void EtkinliklerKontrol_Load(object sender, EventArgs e)
        {
            if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                this.BackColor = Color.White;
            }
            EtkinlikKartlariniOlustur();
        }
    }
}