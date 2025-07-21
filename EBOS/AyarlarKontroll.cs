using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class AyarlarKontroll : UserControl
    {
        private Guna2TextBox txtEskiSifre, txtYeniSifre, txtYeniSifreTekrar;
        private Guna2Button btnSifreGuncelle, btnTemaDegistir;
        private Guna2Button btnYesil, btnLacivert, btnKoyu;
        private Guna2RadioButton rdbYesil, rdbLacivert, rdbKoyu;
        private string aktifKullaniciEposta;

        public AyarlarKontroll(string eposta)
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.aktifKullaniciEposta = eposta;
            ArayuzOlustur();
            this.Load += AyarlarKontroll_Load;
        }

        private void ArayuzOlustur()
        {
            Label lblBaslik = new Label()
            {
                Text = "AYARLAR",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            Guna2Panel pnlProfil = new Guna2Panel()
            {
                FillColor = Color.White,
                Size = new Size(400, 260),
                Location = new Point(30, 80),
                BorderRadius = 12,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1
            };
            this.Controls.Add(pnlProfil);

            Guna2Panel pnlProfilBaslik = new Guna2Panel()
            {
                Size = new Size(400, 40),
                Location = new Point(0, 0),
                FillColor = ColorTranslator.FromHtml("#f44195"),
                BorderRadius = 12
            };
            Label lblProfilBaslik = new Label()
            {
                Text = "Şifre Yenileme",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(15, 10),
                AutoSize = true
            };
            pnlProfilBaslik.Controls.Add(lblProfilBaslik);
            pnlProfil.Controls.Add(pnlProfilBaslik);

            Label lblEskiSifre = new Label() { Text = "Eski Şifre", Location = new Point(20, 65), AutoSize = true, BackColor = Color.Transparent };
            txtEskiSifre = new Guna2TextBox()
            {
                Location = new Point(140, 60),
                Width = 220,
                Height = 30,
                UseSystemPasswordChar = true,
                PlaceholderText = "Eski şifre",
                BorderRadius = 6
            };

            Label lblYeniSifre = new Label() { Text = "Yeni Şifre", Location = new Point(20, 100), AutoSize = true, BackColor = Color.Transparent };
            txtYeniSifre = new Guna2TextBox()
            {
                Location = new Point(140, 95),
                Width = 220,
                Height = 30,
                UseSystemPasswordChar = true,
                PlaceholderText = "Yeni şifre",
                BorderRadius = 6
            };

            Label lblYeniSifreTekrar = new Label() { Text = "Yeni Şifre(Tekrar)", Location = new Point(20, 135), AutoSize = true, BackColor = Color.Transparent };
            txtYeniSifreTekrar = new Guna2TextBox()
            {
                Location = new Point(140, 130),
                Width = 220,
                Height = 30,
                UseSystemPasswordChar = true,
                PlaceholderText = "Yeni şifre (tekrar)",
                BorderRadius = 6
            };

            btnSifreGuncelle = new Guna2Button()
            {
                Text = "Şifreyi Güncelle",
                Location = new Point(140, 185),
                Width = 160,
                FillColor = Color.RoyalBlue,
                ForeColor = Color.White,
                BorderRadius = 6
            };
            btnSifreGuncelle.Click += BtnSifreGuncelle_Click;

            pnlProfil.Controls.AddRange(new Control[] {
                lblEskiSifre, txtEskiSifre,
                lblYeniSifre, txtYeniSifre,
                lblYeniSifreTekrar, txtYeniSifreTekrar,
                btnSifreGuncelle
            });

            Guna2Panel pnlTema = new Guna2Panel()
            {
                FillColor = Color.White,
                Size = new Size(400, 260),
                Location = new Point(437, 80),
                BorderRadius = 12,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1
            };
            this.Controls.Add(pnlTema);

            Guna2Panel pnlTemaBaslik = new Guna2Panel()
            {
                Size = new Size(400, 40),
                Location = new Point(0, 0),
                FillColor = ColorTranslator.FromHtml("#913aaa"),
                BorderRadius = 12
            };
            Label lblTemaBaslik = new Label()
            {
                Text = "Tema Seçimi",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(15, 10),
                AutoSize = true
            };
            pnlTemaBaslik.Controls.Add(lblTemaBaslik);
            pnlTema.Controls.Add(pnlTemaBaslik);

            btnYesil = new Guna2Button()
            {
                Text = "Yeşil Tema",
                Location = new Point(30, 60),
                Size = new Size(350, 45),
                FillColor = Color.FromArgb(90, 115, 47),
                ForeColor = Color.White,
                BorderRadius = 6
            };
            btnYesil.Click += (s, e) => {
                TemaYonetici.AktifTema = "Yesil";
                TemaYonetici.Uygula(this.FindForm());
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                //((OrganisatorPaneli)this.FindForm()).ApplyTheme();
            };

            btnLacivert = new Guna2Button()
            {
                Text = "Lacivert Tema",
                Location = new Point(30, 120),
                Size = new Size(350, 45),
                FillColor = Color.FromArgb(40, 55, 120),
                ForeColor = Color.White,
                BorderRadius = 6
            };
            btnLacivert.Click += (s, e) => {
                TemaYonetici.AktifTema = "Lacivert";
                TemaYonetici.Uygula(this.FindForm());
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                // ((OrganisatorPaneli)this.FindForm()).ApplyTheme();
            };

            btnKoyu = new Guna2Button()
            {
                Text = "Koyu Tema",
                Location = new Point(30, 180),
                Size = new Size(350, 45),
                FillColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderRadius = 6
            };
            btnKoyu.Click += (s, e) => {
                TemaYonetici.AktifTema = "Koyu";
                TemaYonetici.Uygula(this.FindForm());
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                //((OrganisatorPaneli)this.FindForm()).ApplyTheme();
            };

            //btnTemaDegistir = new Guna2Button()
            //{
            //    Text = "Temayı Değiştir",
            //    Location = new Point(120, 190),
            //    Width = 160,
            //    FillColor = Color.Teal,
            //    ForeColor = Color.White,
            //    BorderRadius = 6
            //};

            pnlTema.Controls.AddRange(new Control[] { btnYesil, btnLacivert, btnKoyu, /*btnTemaDegistir*/ });

            rdbYesil = new Guna2RadioButton() { Visible = false };
            rdbLacivert = new Guna2RadioButton() { Visible = false };
            rdbKoyu = new Guna2RadioButton() { Visible = false };
            pnlTema.Controls.AddRange(new Control[] { rdbYesil, rdbLacivert, rdbKoyu });
        }

        private void BtnSifreGuncelle_Click(object sender, EventArgs e)
        {
            string eskiSifre = txtEskiSifre.Text.Trim();
            string yeniSifre = txtYeniSifre.Text.Trim();
            string yeniSifreTekrar = txtYeniSifreTekrar.Text.Trim();

            if (string.IsNullOrWhiteSpace(eskiSifre) || string.IsNullOrWhiteSpace(yeniSifre) || string.IsNullOrWhiteSpace(yeniSifreTekrar))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (yeniSifre != yeniSifreTekrar)
            {
                MessageBox.Show("Yeni şifreler birbiriyle uyuşmuyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == aktifKullaniciEposta);
                if (kullanici == null)
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (kullanici.Sifre != eskiSifre)
                {
                    MessageBox.Show("Eski şifreniz hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                kullanici.Sifre = yeniSifre;
                db.SaveChanges();
                MessageBox.Show("Şifre başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEskiSifre.Clear();
                txtYeniSifre.Clear();
                txtYeniSifreTekrar.Clear();
            }
        }

        private void AyarlarKontroll_Load(object sender, EventArgs e)
        {
            TemaYonetici.Uygula(this.FindForm());
        }
    }
}

//using System;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using EBOS.DataAccess;
//using Guna.UI2.WinForms;

//namespace EBOS
//{
//    public partial class AyarlarKontroll : UserControl
//    {
//        private Guna2TextBox txtEskiSifre, txtYeniSifre, txtYeniSifreTekrar;
//        private Guna2Button btnSifreGuncelle, btnTemaDegistir;
//        private Guna2Button btnYesil, btnLacivert, btnKoyu;
//        private Guna2RadioButton rdbYesil, rdbLacivert, rdbKoyu;
//        private string aktifKullaniciEposta;

//        public AyarlarKontroll(string eposta)
//        {
//            this.Dock = DockStyle.Fill;
//            this.BackColor = Color.White;
//            this.aktifKullaniciEposta = eposta;
//            ArayuzOlustur();
//            this.Load += AyarlarKontroll_Load;
//        }

//        private void ArayuzOlustur()
//        {
//            Label lblBaslik = new Label()
//            {
//                Text = "AYARLAR",
//                Font = new Font("Segoe UI", 20, FontStyle.Bold),
//                Location = new Point(30, 20),
//                AutoSize = true
//            };
//            this.Controls.Add(lblBaslik);

//            // Şifre Yenileme Paneli
//            Guna2Panel pnlProfil = new Guna2Panel()
//            {
//                FillColor = Color.White,
//                Size = new Size(400, 260),
//                Location = new Point(30, 80),
//                BorderRadius = 12,
//                BorderColor = Color.Gainsboro,
//                BorderThickness = 1
//            };
//            this.Controls.Add(pnlProfil);

//            Guna2Panel pnlProfilBaslik = new Guna2Panel()
//            {
//                Size = new Size(400, 40),
//                Location = new Point(0, 0),
//                FillColor = ColorTranslator.FromHtml("#f44195"),
//                BorderRadius = 12
//            };
//            Label lblProfilBaslik = new Label()
//            {
//                Text = "Şifre Yenileme",
//                Font = new Font("Segoe UI", 11, FontStyle.Bold),
//                ForeColor = Color.White,
//                BackColor = Color.Transparent,
//                Location = new Point(15, 10),
//                AutoSize = true
//            };
//            pnlProfilBaslik.Controls.Add(lblProfilBaslik);
//            pnlProfil.Controls.Add(pnlProfilBaslik);

//            Label lblEskiSifre = new Label() { Text = "Eski Şifre", Location = new Point(20, 65), AutoSize = true, BackColor = Color.Transparent };
//            txtEskiSifre = new Guna2TextBox()
//            {
//                Location = new Point(140, 60),
//                Width = 220,
//                Height = 30,
//                UseSystemPasswordChar = true,
//                PlaceholderText = "Eski şifre",
//                BorderRadius = 6
//            };

//            Label lblYeniSifre = new Label() { Text = "Yeni Şifre", Location = new Point(20, 100), AutoSize = true, BackColor = Color.Transparent };
//            txtYeniSifre = new Guna2TextBox()
//            {
//                Location = new Point(140, 95),
//                Width = 220,
//                Height = 30,
//                UseSystemPasswordChar = true,
//                PlaceholderText = "Yeni şifre",
//                BorderRadius = 6
//            };

//            Label lblYeniSifreTekrar = new Label() { Text = "Yeni Şifre(Tekrar)", Location = new Point(20, 135), AutoSize = true, BackColor = Color.Transparent };
//            txtYeniSifreTekrar = new Guna2TextBox()
//            {
//                Location = new Point(140, 130),
//                Width = 220,
//                Height = 30,
//                UseSystemPasswordChar = true,
//                PlaceholderText = "Yeni şifre (tekrar)",
//                BorderRadius = 6
//            };

//            btnSifreGuncelle = new Guna2Button()
//            {
//                Text = "Şifreyi Güncelle",
//                Location = new Point(140, 185),
//                Width = 160,
//                FillColor = Color.RoyalBlue,
//                ForeColor = Color.White,
//                BorderRadius = 6
//            };
//            btnSifreGuncelle.Click += BtnSifreGuncelle_Click;

//            pnlProfil.Controls.AddRange(new Control[] {
//                lblEskiSifre, txtEskiSifre,
//                lblYeniSifre, txtYeniSifre,
//                lblYeniSifreTekrar, txtYeniSifreTekrar,
//                btnSifreGuncelle
//            });

//            // Tema Paneli
//            Guna2Panel pnlTema = new Guna2Panel()
//            {
//                FillColor = Color.White,
//                Size = new Size(400, 260),
//                Location = new Point(437, 80),
//                BorderRadius = 12,
//                BorderColor = Color.Gainsboro,
//                BorderThickness = 1
//            };
//            this.Controls.Add(pnlTema);

//            Guna2Panel pnlTemaBaslik = new Guna2Panel()
//            {
//                Size = new Size(400, 40),
//                Location = new Point(0, 0),
//                FillColor = ColorTranslator.FromHtml("#913aaa"),
//                BorderRadius = 12
//            };
//            Label lblTemaBaslik = new Label()
//            {
//                Text = "Tema Seçimi",
//                Font = new Font("Segoe UI", 11, FontStyle.Bold),
//                ForeColor = Color.White,
//                BackColor = Color.Transparent,
//                Location = new Point(15, 10),
//                AutoSize = true
//            };
//            pnlTemaBaslik.Controls.Add(lblTemaBaslik);
//            pnlTema.Controls.Add(pnlTemaBaslik);

//            // Tema Butonları
//            btnYesil = new Guna2Button()
//            {
//                Text = "Yeşil Tema",
//                Location = new Point(30, 60),
//                Size = new Size(350, 35),
//                FillColor = Color.FromArgb(90, 115, 47),
//                ForeColor = Color.White,
//                BorderRadius = 6
//            };
//            btnYesil.Click += (s, e) => { TemaYonetici.AktifTema = "Yesil"; TemaYonetici.Uygula(this.FindForm()); };

//            btnLacivert = new Guna2Button()
//            {
//                Text = "Lacivert Tema",
//                Location = new Point(30, 100),
//                Size = new Size(350, 35),
//                FillColor = Color.FromArgb(40, 55, 120),
//                ForeColor = Color.White,
//                BorderRadius = 6
//            };
//            btnLacivert.Click += (s, e) => { TemaYonetici.AktifTema = "Lacivert"; TemaYonetici.Uygula(this.FindForm()); };

//            btnKoyu = new Guna2Button()
//            {
//                Text = "Koyu Tema",
//                Location = new Point(30, 140),
//                Size = new Size(350, 35),
//                FillColor = Color.FromArgb(50, 50, 50),
//                ForeColor = Color.White,
//                BorderRadius = 6
//            };
//            btnKoyu.Click += (s, e) => { TemaYonetici.AktifTema = "Koyu"; TemaYonetici.Uygula(this.FindForm()); };

//            btnTemaDegistir = new Guna2Button()
//            {
//                Text = "Temayı Değiştir",
//                Location = new Point(120, 190),
//                Width = 160,
//                FillColor = Color.Teal,
//                ForeColor = Color.White,
//                BorderRadius = 6
//            };

//            pnlTema.Controls.AddRange(new Control[] { btnYesil, btnLacivert, btnKoyu, btnTemaDegistir });

//            rdbYesil = new Guna2RadioButton() { Visible = false };
//            rdbLacivert = new Guna2RadioButton() { Visible = false };
//            rdbKoyu = new Guna2RadioButton() { Visible = false };
//            pnlTema.Controls.AddRange(new Control[] { rdbYesil, rdbLacivert, rdbKoyu });
//        }

//        private void BtnSifreGuncelle_Click(object sender, EventArgs e)
//        {
//            string eskiSifre = txtEskiSifre.Text.Trim();
//            string yeniSifre = txtYeniSifre.Text.Trim();
//            string yeniSifreTekrar = txtYeniSifreTekrar.Text.Trim();

//            if (string.IsNullOrWhiteSpace(eskiSifre) || string.IsNullOrWhiteSpace(yeniSifre) || string.IsNullOrWhiteSpace(yeniSifreTekrar))
//            {
//                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            if (yeniSifre != yeniSifreTekrar)
//            {
//                MessageBox.Show("Yeni şifreler birbiriyle uyuşmuyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            using (var db = new AppDbContext())
//            {
//                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == aktifKullaniciEposta);
//                if (kullanici == null)
//                {
//                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                if (kullanici.Sifre != eskiSifre)
//                {
//                    MessageBox.Show("Eski şifreniz hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                kullanici.Sifre = yeniSifre;
//                db.SaveChanges();
//                MessageBox.Show("Şifre başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                txtEskiSifre.Clear();
//                txtYeniSifre.Clear();
//                txtYeniSifreTekrar.Clear();
//            }
//        }

//        private void AyarlarKontroll_Load(object sender, EventArgs e)
//        {
//            if (TemaYonetici.AktifTema == "Koyu")
//            {
//                this.BackColor = Color.FromArgb(120, 120, 120);
//            }
//            else
//            {
//                this.BackColor = Color.White;
//            }
//        }
//    }
//}
