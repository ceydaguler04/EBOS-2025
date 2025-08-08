using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;

namespace EBOS
{
    public partial class EtkinlikEkleForm : Form
    {
        private Label lblBaslik, lblAd, lblAciklama, lblSure, lblSaat;
        private Guna2TextBox txtAd, txtAciklama, txtSure;
        private Guna2DateTimePicker dtpTarih;
        private ComboBox cmbSaat;
        private Guna2ComboBox cmbKategori;
        private Guna2Button btnKaydet, btnMekanSec, btnGorselEkle;
        private PictureBox picGorsel;
        private Label lblSecilenMekan;
        private RadioButton rbDosyadanEkle;
        private RadioButton rbYolGir;
        private Guna2TextBox txtGorselYolu;

        private Mekan secilenMekan;
        private int? aktifKullaniciId;
        private int? secilenMekanID = null;
        private string secilenGorselYolu = "";

        private string secilenSehir = "";
        private string secilenIlce = "";
        private string secilenSemt = "";
        private double? secilenEnlem = null;
        private double? secilenBoylam = null;
        private string secilenKonumUrl = "";

        private Etkinlik mevcutEtkinlik;

        public EtkinlikEkleForm(int? kullaniciId)
        {
            InitializeComponent();
            aktifKullaniciId = kullaniciId;

            this.Text = "Etkinlik Ekle";
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);

            lblBaslik = new Label()
            {
                Text = "Etkinlik Ekle",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Location = new Point(100, 20)
            };
            this.Controls.Add(lblBaslik);

            int left = 40;
            int width = 300;
            int top = 70;

            lblAd = new Label() { Text = "Etkinlik Adı", Font = labelFont, Location = new Point(left, top) };
            txtAd = new Guna2TextBox() { Location = new Point(left, top + 20), Width = width, Height = 35 };

            lblAciklama = new Label() { Text = "Açıklama", Font = labelFont, Location = new Point(left, top + 60) };
            txtAciklama = new Guna2TextBox() { Location = new Point(left, top + 80), Width = width, Height = 35 };

            lblSure = new Label() { Text = "Süre (dakika)",Font = labelFont,Location = new Point(left, top + 115) };
            txtSure = new Guna2TextBox() { Location = new Point(left, top + 135),Width = 140,Height = 35 };

            lblSaat = new Label() { Text = "Saat",Font = labelFont,Location = new Point(left + 160, top + 115) };
            
            cmbSaat = new ComboBox()
            {
                Location = new Point(left + 160, top + 135),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                IntegralHeight = false,
                MaxDropDownItems = 10,
                Font = new Font("Segoe UI", 10)
            };

            for (int saat = 0; saat < 24; saat++)
            {
                cmbSaat.Items.Add($"{saat:D2}:00");
                cmbSaat.Items.Add($"{saat:D2}:30");
            }

            cmbSaat.SelectedIndex = 0;

            dtpTarih = new Guna2DateTimePicker()
            {
                Location = new Point(left, top + 180),
                Width = width,
                FillColor = Color.FromArgb(255, 199, 107),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Long
            };

            cmbKategori = new Guna2ComboBox()
            {
                Location = new Point(left, top + 225),
                Width = width,
                Font = new Font("Segoe UI", 10)
            };
            cmbSaat.Items.AddRange(new string[] { "18:00", "19:00", "20:00", "21:00", "22:00" });
            cmbSaat.SelectedIndex = 0;

            cmbKategori = new Guna2ComboBox()
            {
                Location = new Point(left, top + 310),
                Width = width,
                Font = new Font("Segoe UI", 10),
                //PlaceholderText = "Etkinlik türü seçin"
            };
            cmbKategori.Items.Insert(0, "-- Tür Seçiniz --");
            cmbKategori.Items.AddRange(new string[] { "Konser", "Sinema", "Tiyatro", "Workshop", "Seminer" });
            cmbKategori.SelectedIndex = 0;

            ///// RadioButton: Dosyadan Ekle
            rbDosyadanEkle = new RadioButton()
            {
                Text = "Dosyadan Ekle",
                Location = new Point(left, top + 260),
                Checked = true,
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };

            // RadioButton: Yol Gir
            rbYolGir = new RadioButton()
            {
                Text = "Yol Gir",
                Location = new Point(left + 150, top + 260),
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            };
            rbDosyadanEkle.CheckedChanged += RbGorselSecimChanged;
            rbYolGir.CheckedChanged += RbGorselSecimChanged;

            // Yol girişi kutusu (başta gizli)
            txtGorselYolu = new Guna2TextBox()
            {
                PlaceholderText = "Görsel yolu girin (örn: C:\\resim.jpg)",
                Location = new Point(left, top + 390),
                Width = width,
                Height = 35,
                Visible = false
            };
            btnGorselEkle.Click += BtnGorselEkle_Click;

            picGorsel = new PictureBox()
            {
                Location = new Point(left, top + 285),
                Size = new Size(width, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                BackColor = Color.White
            };
            picGorsel.Click += PicGorsel_Click;

            picGorsel.Paint += (s, e) =>
            {
                if (picGorsel.Image == null)
                {
                    using var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString("Görsel eklemek için tıklayın", picGorsel.Font, Brushes.Gray, picGorsel.ClientRectangle, sf);
                }
            };


            lblSecilenMekan = new Label()
            {
                Text = "📍 Seçilen: (Henüz yok)",
                Location = new Point(left, top + 435),
                Width = 320,
                Height = 40,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                AutoSize = false
            };

            btnMekanSec = new Guna2Button()
            {
                Text = "Mekan Seç",
                Location = new Point(left, top + 480),
                Width = width,
                FillColor = Color.FromArgb(41, 53, 89),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            btnMekanSec.Click += BtnMekanSec_Click;

            btnKaydet = new Guna2Button()
            {
                Text = "Kaydet",
                Location = new Point(left, top + 530),
                Width = width,
                FillColor = Color.ForestGreen,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White
            };
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.AddRange(new Control[]
            {
                lblAd, txtAd,
                lblAciklama, txtAciklama,
                lblSure, txtSure,
                lblSaat, cmbSaat,
                dtpTarih, cmbKategori,
                rbDosyadanEkle, rbYolGir, txtGorselYolu,
                picGorsel, btnMekanSec, lblSecilenMekan,
                btnKaydet
            });
        }
        private void PicGorsel_Click(object sender, EventArgs e)
        {
            if (!rbDosyadanEkle.Checked) return;

        private void BtnGorselEkle_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyası|*.jpg;*.jpeg;*.png;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    secilenGorselYolu = ofd.FileName;
                    picGorsel.Image = Image.FromFile(secilenGorselYolu);
                }
            }
        }

        private void BtnMekanSec_Click(object sender, EventArgs e)
        {
            HaritaForm haritaForm = new HaritaForm("https://www.google.com/maps");

            if (haritaForm.ShowDialog() == DialogResult.OK)
            {
                secilenMekanID = haritaForm.SecilenMekanID;

                secilenSehir = haritaForm.SecilenSehirAdi;
                secilenIlce = haritaForm.SecilenIlceAdi;
                secilenSemt = haritaForm.SecilenSemt;

                if (secilenMekanID != null)
                {
                    using var db = new AppDbContext();
                    var mekan = db.Mekanlar.FirstOrDefault(m => m.MekanID == secilenMekanID);
                    if (mekan != null)
                    {
                        lblSecilenMekan.Text = $"📍 Seçilen:\n{haritaForm.SecilenMekanAdi}";
                        lblSecilenMekan.ForeColor = Color.Green;
                    }
                }
                else
                {
                    lblSecilenMekan.Text = "Mekan seçilemedi!";
                    lblSecilenMekan.ForeColor = Color.Red;
                }
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            List<string> eksikler = new List<string>();

            if (string.IsNullOrWhiteSpace(txtAd.Text))
                eksikler.Add("- Etkinlik adı");

            if (string.IsNullOrWhiteSpace(txtSure.Text))
                eksikler.Add("- Süre");
            else if (!int.TryParse(txtSure.Text, out int sureDakika) || sureDakika <= 0)
                eksikler.Add("- Süre geçerli bir sayı olmalı");

            if (cmbSaat.SelectedItem == null)
                eksikler.Add("- Saat");

            if (dtpTarih.Value.Date < DateTime.Today)
                eksikler.Add("- Geçerli bir tarih");

            if (cmbKategori.SelectedIndex <= 0)
                eksikler.Add("- Etkinlik türü");

            if (secilenMekanID == null)
            {
                MessageBox.Show("Lütfen bir mekan seçin.");
                return;
            }

            if (eksikler.Count > 0)
            {
                string mesaj = "Lütfen aşağıdaki alanları doldurun:\n\n" + string.Join("\n", eksikler);
                MessageBox.Show(mesaj, "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                if (mevcutEtkinlik != null)
                {
                    var guncellenecek = db.Etkinlikler.FirstOrDefault(e => e.EtkinlikID == mevcutEtkinlik.EtkinlikID);
                    if (guncellenecek != null)
                    {
                        guncellenecek.EtkinlikAdi = txtAd.Text;
                        guncellenecek.Aciklama = txtAciklama.Text;
                        guncellenecek.SureDakika = int.Parse(txtSure.Text);
                        guncellenecek.Tarih = dtpTarih.Value;
                        guncellenecek.Saat = TimeSpan.Parse(cmbSaat.SelectedItem.ToString());
                        guncellenecek.TurID = KategoriToTurID(cmbKategori.SelectedItem.ToString());
                        guncellenecek.KullaniciID = aktifKullaniciId ?? 2;
                        guncellenecek.GorselYolu = rbDosyadanEkle.Checked ? secilenGorselYolu : txtGorselYolu.Text.Trim();
                        guncellenecek.MekanID = secilenMekanID.Value;

                        var mekan = db.Mekanlar.FirstOrDefault(x => x.MekanID == secilenMekanID);
                        guncellenecek.IlceID = mekan?.IlceID;

                        db.SaveChanges();
                        MessageBox.Show("✅ Etkinlik başarıyla güncellendi!");
                    }
                }
                else
                {
                var etkinlik = new Etkinlik()
                {
                    EtkinlikAdi = txtAd.Text,
                    Aciklama = txtAciklama.Text,
                    SureDakika = int.Parse(txtSure.Text),
                    Tarih = dtpTarih.Value,
                    Saat = TimeSpan.Parse(cmbSaat.SelectedItem.ToString()),
                    TurID = KategoriToTurID(cmbKategori.SelectedItem.ToString()),
                        KullaniciID = aktifKullaniciId ?? 2,
                        GorselYolu = rbDosyadanEkle.Checked ? secilenGorselYolu : txtGorselYolu.Text.Trim(),
                        MekanID = secilenMekanID.Value,
                        IlceID = db.Mekanlar.FirstOrDefault(x => x.MekanID == secilenMekanID)?.IlceID
                };

                db.Etkinlikler.Add(etkinlik);
                db.SaveChanges();
                    MessageBox.Show("✅ Etkinlik başarıyla eklendi!");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void RbGorselSecimChanged(object sender, EventArgs e)
        {
            if (rbDosyadanEkle.Checked)
            {
                txtGorselYolu.Visible = false;
                picGorsel.Enabled = true;
            }
            else
            {
                txtGorselYolu.Visible = true;
                picGorsel.Enabled = false;
                picGorsel.Image = null; // Temizle
                secilenGorselYolu = ""; // Yol sıfırla
            }
        }
        private int KategoriToTurID(string kategori)
        {
            return kategori.ToLower() switch
            {
                "konser" => 1,
                "sinema" => 2,
                "tiyatro" => 3,
                "seminer" => 4,
                "workshop" => 5,
                _ => 0
            };
        }
        public EtkinlikEkleForm(Etkinlik etkinlik, int? kullaniciId) : this(kullaniciId)
        {
            mevcutEtkinlik = etkinlik;

            // Alanları doldur
            txtAd.Text = mevcutEtkinlik.EtkinlikAdi;
            txtAciklama.Text = mevcutEtkinlik.Aciklama;
            txtSure.Text = mevcutEtkinlik.SureDakika.ToString();
            dtpTarih.Value = mevcutEtkinlik.Tarih;
            cmbSaat.SelectedItem = mevcutEtkinlik.Saat.ToString(@"hh\:mm");
            if (!string.IsNullOrEmpty(mevcutEtkinlik.GorselYolu))
            {
                if (File.Exists(mevcutEtkinlik.GorselYolu))
                {
                    picGorsel.ImageLocation = mevcutEtkinlik.GorselYolu;
                    rbDosyadanEkle.Checked = true;
                }
                else
                {
                    txtGorselYolu.Text = mevcutEtkinlik.GorselYolu;
                    rbYolGir.Checked = true;
                }
            }


            // Tür adını eşleştir
            string turAdi = mevcutEtkinlik.EtkinlikTuru?.TurAdi;
            if (!string.IsNullOrEmpty(turAdi))
                cmbKategori.SelectedItem = turAdi;

            // Mekan adını göster
            using (var db = new AppDbContext())
            {
                var mekan = db.Mekanlar.FirstOrDefault(m => m.MekanID == mevcutEtkinlik.MekanID);
                if (mekan != null)
                {
                    lblSecilenMekan.Text = $"📍 Seçilen:\n{mekan.Ad}, {mekan.Sehir}";
                    secilenMekanID = mekan.MekanID;
                }
            }
        }

        private void EtkinlikEkleForm_Load(object sender, EventArgs e)
        {

        }
    }
}