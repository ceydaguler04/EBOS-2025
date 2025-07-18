using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class AyarKontrol : UserControl
    {
        private Label lblAdSoyad, lblEmail, lblEskiSifre, lblYeniSifre, lblYeniSifreTekrar;
        private Guna2TextBox txtAdSoyad, txtEmail, txtEskiSifre, txtYeniSifre, txtYeniSifreTekrar;
        private Guna2Button btnSifreGuncelle, btnYesil, btnLacivert, btnKoyu;
        private Guna2Panel panelSol, panelSag;
        private string kullaniciEposta;

        public AyarKontrol(string eposta)
        {
            kullaniciEposta = eposta;
            InitializeComponent();
            ArayuzOlustur();
            TemaYonetici.Uygula(this.FindForm()); // Açılırken temayı uygula
        }

        private void ArayuzOlustur()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // 🔲 Şifre Güncelle Paneli
            var sifrePanel = new Guna2Panel()
            {
                Size = new Size(400, 300),
                Location = new Point(60, 60),
                BorderRadius = 10,
                BorderThickness = 2,
                BorderColor = Color.Silver,
                BackColor = Color.White
            };
            this.Controls.Add(sifrePanel);

            var lblBaslik = new Label
            {
                Text = "Şifre Güncelleme",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                AutoSize = true
            };
            sifrePanel.Controls.Add(lblBaslik);

            var lblEski = new Label { Text = "Eski Şifre:", Location = new Point(20, 60), AutoSize = true };
            var lblYeni = new Label { Text = "Yeni Şifre:", Location = new Point(20, 110), AutoSize = true };
            var lblTekrar = new Label { Text = "Yeni Şifre (Tekrar):", Location = new Point(20, 160), AutoSize = true };

            txtEskiSifre = new Guna2TextBox { Location = new Point(150, 55), Width = 200, UseSystemPasswordChar = true };
            txtYeniSifre = new Guna2TextBox { Location = new Point(150, 105), Width = 200, UseSystemPasswordChar = true };
            txtYeniSifreTekrar = new Guna2TextBox { Location = new Point(150, 155), Width = 200, UseSystemPasswordChar = true };

            btnSifreGuncelle = new Guna2Button
            {
                Text = "Şifreyi Güncelle",
                Location = new Point(150, 210),
                Size = new Size(200, 40)
            };
            btnSifreGuncelle.Click += BtnSifreGuncelle_Click;

            sifrePanel.Controls.AddRange(new Control[]
            {
                lblEski, lblYeni, lblTekrar,
                txtEskiSifre, txtYeniSifre, txtYeniSifreTekrar,
                btnSifreGuncelle
            });

            // 🎨 Tema Paneli
            var temaPanel = new Guna2Panel()
            {
                Size = new Size(250, 300),
                Location = new Point(500, 60),
                BorderRadius = 10,
                BorderThickness = 2,
                BorderColor = Color.Silver,
                BackColor = Color.White
            };
            this.Controls.Add(temaPanel);

            var lblTema = new Label
            {
                Text = "Tema Seçimi",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                AutoSize = true
            };
            temaPanel.Controls.Add(lblTema);

            btnYesil = new Guna2Button
            {
                Text = "Yeşil Tema",
                Location = new Point(30, 70),
                Size = new Size(180, 35)
            };
            btnYesil.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Yesil";
                TemaYonetici.Uygula(this.FindForm());

                if (this.FindForm() is KullaniciPaneli kp)
                    kp.SolMenuTemayiGuncelle();
            };

            btnLacivert = new Guna2Button
            {
                Text = "Lacivert Tema",
                Location = new Point(30, 120),
                Size = new Size(180, 35)
            };
            btnLacivert.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Lacivert";
                TemaYonetici.Uygula(this.FindForm());

                if (this.FindForm() is KullaniciPaneli kp)
                    kp.SolMenuTemayiGuncelle();
            };

            btnKoyu = new Guna2Button
            {
                Text = "Koyu Tema",
                Location = new Point(30, 170),
                Size = new Size(180, 35)
            };
            btnKoyu.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Koyu";
                TemaYonetici.Uygula(this.FindForm());

                if (this.FindForm() is KullaniciPaneli kp)
                    kp.SolMenuTemayiGuncelle();
            };

            temaPanel.Controls.AddRange(new Control[]
            {
                btnYesil, btnLacivert, btnKoyu
            });
        }

        private void BtnSifreGuncelle_Click(object sender, EventArgs e)
        {
            if (txtYeniSifre.Text != txtYeniSifreTekrar.Text)
            {
                MessageBox.Show("Yeni şifreler eşleşmiyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var db = new DataAccess.AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == kullaniciEposta);

                if (kullanici == null)
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (kullanici.Sifre != txtEskiSifre.Text)
                {
                    MessageBox.Show("Eski şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                kullanici.Sifre = txtYeniSifre.Text;
                db.SaveChanges();

                MessageBox.Show("Şifre başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtEskiSifre.Clear();
                txtYeniSifre.Clear();
                txtYeniSifreTekrar.Clear();
            }
        }
    }
}

