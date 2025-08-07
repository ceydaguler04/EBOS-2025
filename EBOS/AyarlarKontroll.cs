using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;
using System.Net;
using System.Net.Mail;

namespace EBOS
{
    public partial class AyarlarKontroll : UserControl
    {
        private Guna2TextBox txtEskiSifre, txtYeniSifre, txtYeniSifreTekrar;
        private Guna2Button btnSifreGuncelle;
        private Guna2Button btnYesil, btnLacivert, btnKoyu;
        private Guna2RadioButton rdbYesil, rdbLacivert, rdbKoyu;

        private Guna2TextBox txtYeniMail;
        private Guna2Button btnMailGuncelle;
        private string dogrulamaKodu;
        private DateTime kodOlusturulmaZamani;

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
                Location = new Point(30, 70),
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
                Location = new Point(437, 70),
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
            btnYesil.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Yesil";
                TemaYonetici.Uygula(this.FindForm());
                TemaAyarla();
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                else if (form is KullaniciPaneli kulForm)
                    kulForm.ApplyTheme();
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
            btnLacivert.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Lacivert";
                TemaYonetici.Uygula(this.FindForm());
                TemaAyarla();
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                else if (form is KullaniciPaneli kulForm)
                    kulForm.ApplyTheme();
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
            btnKoyu.Click += (s, e) =>
            {
                TemaYonetici.AktifTema = "Koyu";
                TemaYonetici.Uygula(this.FindForm());
                TemaAyarla();
                var form = this.FindForm();
                if (form is OrganisatorPaneli orgForm)
                    orgForm.ApplyTheme();
                else if (form is YoneticiPaneli yonForm)
                    yonForm.ApplyTheme();
                else if (form is KullaniciPaneli kulForm)
                    kulForm.ApplyTheme();
                this.BackColor = Color.FromArgb(180, 180, 180);
            };

            pnlTema.Controls.AddRange(new Control[] { btnYesil, btnLacivert, btnKoyu, /*btnTemaDegistir*/ });

            rdbYesil = new Guna2RadioButton() { Visible = false };
            rdbLacivert = new Guna2RadioButton() { Visible = false };
            rdbKoyu = new Guna2RadioButton() { Visible = false };
            pnlTema.Controls.AddRange(new Control[] { rdbYesil, rdbLacivert, rdbKoyu });

            // Mail Güncelleme Paneli
            Guna2Panel pnlMail = new Guna2Panel()
            {
                FillColor = Color.White,
                Size = new Size(400, 200/*250*/),
                Location = new Point(230, 350),
                BorderRadius = 12,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1
            };
            this.Controls.Add(pnlMail);

            Guna2Panel pnlMailBaslik = new Guna2Panel()
            {
                Size = new Size(400, 40),
                Location = new Point(0, 0),
                FillColor = ColorTranslator.FromHtml("#FF0000"),
                BorderRadius = 12
            };
            Label lblMailBaslik = new Label()
            {
                Text = "Mail Güncelleme",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(15, 10),
                AutoSize = true
            };
            pnlMailBaslik.Controls.Add(lblMailBaslik);
            pnlMail.Controls.Add(pnlMailBaslik);

            Label lblYeniMail = new Label()
            {
                Text = "Yeni E-posta",
                Location = new Point(20, 65),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            txtYeniMail = new Guna2TextBox()
            {
                Location = new Point(140, 60),
                Width = 220,
                Height = 30,
                BorderRadius = 6,
                PlaceholderText = "Yeni e-posta"
            };

            btnMailGuncelle = new Guna2Button()
            {
                Text = "Maili Güncelle",
                Location = new Point(140, 110/*180*/),
                Width = 160,
                FillColor = Color.SeaGreen,
                ForeColor = Color.White,
                BorderRadius = 6
            };

            btnMailGuncelle.Click += (s, e) =>
            {
                string yeniMail = txtYeniMail.Text.Trim();

                if (string.IsNullOrWhiteSpace(yeniMail) || !yeniMail.Contains("@"))
                {
                    MessageBox.Show("Lütfen geçerli bir e-posta giriniz.");
                    return;
                }

                using (var db = new AppDbContext())
                {
                    var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == aktifKullaniciEposta);
                    if (kullanici == null)
                    {
                        MessageBox.Show("Kullanıcı bulunamadı.");
                        return;
                    }

                    bool ayniMailVar = db.Kullanicilar.Any(k => k.Eposta == yeniMail);
                    if (ayniMailVar)
                    {
                        MessageBox.Show("Bu e-posta zaten kullanımda.");
                        return;
                    }

                    // Kod üret
                    Random rnd = new Random();
                    dogrulamaKodu = rnd.Next(100000, 999999).ToString();
                    kodOlusturulmaZamani = DateTime.Now;

                    try
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("ebos.otomasyon@gmail.com");
                        mail.To.Add(aktifKullaniciEposta);
                        mail.Subject = "EBOS - E-posta Güncelleme Doğrulama Kodu";
                        mail.Body = $"Merhaba {kullanici.AdSoyad},\n\n" +
                                    $"E-posta adresinizi değiştirmek üzere bir işlem başlattınız. Doğrulama kodunuz aşağıdadır:\n\n" +
                                    $"🛡 Kod: {dogrulamaKodu}\n\n" +
                                    $"Bu kod 3 dakika geçerlidir.\n\n" +
                                    $"Bu işlemi siz başlatmadıysanız lütfen bu e-postayı dikkate almayınız.\n\n" +
                                    $"EBOS Bilet Otomasyon Sistemi";

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.Credentials = new NetworkCredential("ebos.otomasyon@gmail.com", "jkhokwvtjqlioags");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        MessageBox.Show("Kod e-posta adresinize gönderildi.\nKod 1 dakika içinde gelmezse spam klasörünüzü kontrol ediniz.");
                        btnMailGuncelle.Enabled = false;

                        KodDogrulamaForm dogrulamaForm = new KodDogrulamaForm();

                        dogrulamaForm.KodYenidenGonderildi += (snd, args) =>
                        {
                            dogrulamaKodu = new Random().Next(100000, 999999).ToString();
                            kodOlusturulmaZamani = DateTime.Now;

                            try
                            {
                                MailMessage mail2 = new MailMessage();
                                mail2.From = new MailAddress("ebos.otomasyon@gmail.com");
                                mail2.To.Add(aktifKullaniciEposta);
                                mail2.Subject = "EBOS - Yeni Doğrulama Kodu";
                                mail2.Body = $"Yeni kodunuz: {dogrulamaKodu}\nBu kod 3 dakika geçerlidir.";

                                SmtpClient smtp2 = new SmtpClient("smtp.gmail.com", 587);
                                smtp2.Credentials = new NetworkCredential("ebos.otomasyon@gmail.com", "jkhokwvtjqlioags");
                                smtp2.EnableSsl = true;
                                smtp2.Send(mail2);

                                MessageBox.Show("Yeni doğrulama kodu gönderildi.", "Bilgi");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Yeni mail gönderilemedi: " + ex.Message);
                            }
                        };

                        bool dogruMu = false;

                        while (!dogruMu)
                        {
                            var sonuc = dogrulamaForm.ShowDialog();

                            if (sonuc != DialogResult.OK)
                            {
                                btnMailGuncelle.Enabled = true;
                                break;
                            }


                            if ((DateTime.Now - kodOlusturulmaZamani).TotalMinutes > 3)
                            {
                                MessageBox.Show("Kodun süresi doldu. Lütfen yeniden kod isteyin.");
                                btnMailGuncelle.Enabled = true;
                                break;
                            }

                            if (string.Equals(dogrulamaForm.GirilenKod, dogrulamaKodu))
                            {
                                kullanici.Eposta = yeniMail;
                                db.SaveChanges();
                                MessageBox.Show("E-posta başarıyla güncellendi.");
                                btnMailGuncelle.Enabled = true;
                                aktifKullaniciEposta = yeniMail;
                                txtYeniMail.Clear();
                                dogruMu = true;
                            }
                            else
                            {
                                MessageBox.Show("Girilen kod hatalı. Lütfen tekrar deneyin.");
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mail gönderilemedi: " + ex.Message);
                    }
                }
            };

            pnlMail.Controls.AddRange(new Control[] {
                lblYeniMail,
                txtYeniMail,
                btnMailGuncelle
            });

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
            TemaAyarla();
        }
        public void TemaAyarla()
        {
            if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(180, 180, 180);
            }
            else
            {
                this.BackColor = Color.White;
            }
        }

    }
}