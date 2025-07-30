using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class EtkinlikEkleForm : Form
    {
        private Label lblBaslik, lblAd, lblAciklama, lblSure;
        private Guna2TextBox txtAd, txtAciklama, txtSure;
        private Guna2DateTimePicker dtpTarih;
        private Guna2ComboBox cmbSaat, cmbKategori;
        private Guna2Button btnKaydet, btnMekanSec, btnGorselEkle;
        private PictureBox picGorsel;

        private int? aktifKullaniciId;
        private int? secilenMekanID = null;
        private string secilenGorselYolu = "";

        public EtkinlikEkleForm(int? kullaniciId = null)
        {
            aktifKullaniciId = kullaniciId;

            this.Text = "Etkinlik Ekle";
            this.Size = new Size(400, 650);
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
                Location = new Point(130, 20)
            };
            this.Controls.Add(lblBaslik);

            int left = 40;
            int width = 300;
            int top = 70;

            lblAd = new Label() { Text = "Etkinlik Adı", Font = labelFont, Location = new Point(left, top) };
            txtAd = new Guna2TextBox() { Location = new Point(left, top + 20), Width = width };

            lblAciklama = new Label() { Text = "Açıklama", Font = labelFont, Location = new Point(left, top + 70) };
            txtAciklama = new Guna2TextBox() { Location = new Point(left, top + 90), Width = width };

            lblSure = new Label() { Text = "Süre (dakika)", Font = labelFont, Location = new Point(left, top + 140) };
            txtSure = new Guna2TextBox() { Location = new Point(left, top + 160), Width = width };

            dtpTarih = new Guna2DateTimePicker()
            {
                Location = new Point(left, top + 210),
                Width = width,
                FillColor = Color.FromArgb(255, 199, 107),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Long
            };

            cmbSaat = new Guna2ComboBox()
            {
                Location = new Point(left, top + 260),
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

            btnGorselEkle = new Guna2Button()
            {
                Text = "🖼️  Görsel Ekle",
                Location = new Point(left, top + 360),
                Width = width,
                BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash,
                FillColor = Color.Transparent,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black
            };
            btnGorselEkle.Click += BtnGorselEkle_Click;

            btnMekanSec = new Guna2Button()
            {
                Text = "Mekan Seç",
                Location = new Point(left, top + 420),
                Width = width,
                FillColor = Color.FromArgb(41, 53, 89),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };

            btnKaydet = new Guna2Button()
            {
                Text = "Kaydet",
                Location = new Point(left, top + 470),
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
                dtpTarih, cmbSaat, cmbKategori,
                btnGorselEkle, btnMekanSec, btnKaydet
            });
        }

        private void BtnGorselEkle_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyası|*.jpg;*.jpeg;*.png;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    secilenGorselYolu = ofd.FileName;
                    MessageBox.Show("Seçilen Görsel: " + secilenGorselYolu);
                }
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSure.Text))
            {
                MessageBox.Show("Etkinlik adı ve süre zorunludur.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var etkinlik = new Etkinlik()
                {
                    EtkinlikAdi = txtAd.Text,
                    Aciklama = txtAciklama.Text,
                    SureDakika = int.Parse(txtSure.Text),
                    Tarih = dtpTarih.Value,
                    Saat = TimeSpan.Parse(cmbSaat.SelectedItem.ToString()),
                    TurID = KategoriToTurID(cmbKategori.SelectedItem.ToString()),
                    KullaniciID = aktifKullaniciId ?? 1,
                    GorselYolu = string.IsNullOrEmpty(secilenGorselYolu) ? "" : secilenGorselYolu,
                    MekanID = secilenMekanID ?? 1, // MekanSecForm yapılınca değişecek
                    IlceID = db.Mekanlar.FirstOrDefault(x => x.MekanID == 1)?.IlceID ?? 1
                };

                db.Etkinlikler.Add(etkinlik);
                db.SaveChanges();
                MessageBox.Show("Etkinlik başarıyla eklendi!");
                this.Close();
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
        private void EtkinlikEkleForm_Load(object sender, EventArgs e)
        {

        }
    }
}