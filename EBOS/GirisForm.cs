using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Guna.UI2.WinForms;
using EBOS.DataAccess;

namespace EBOS
{
    public partial class GirisForm : Form
    {
        private Guna2TextBox txtEposta, txtSifre;
        private Guna2Button btnGiris;
        private LinkLabel linkKayit, linkSifreUnuttum;
        private Guna2Panel kartPanel;

        public GirisForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(430, 600);
            this.BackColor = Color.White;
            this.Opacity = 0;
            this.Load += FadeIn;

            ArayuzOlustur();
        }

        private void FadeIn(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();

            fadeTimer.Interval = 10;
            fadeTimer.Tick += (s, ev) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeTimer.Stop();
            };
            fadeTimer.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                Color.FromArgb(90, 115, 47), Color.FromArgb(60, 80, 30), 45f))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void ArayuzOlustur()
        {
            // Kart Panel
            kartPanel = new Guna2Panel()
            {
                Size = new Size(360, 430),
                Location = new Point((this.Width - 360) / 2, 80),
                BorderRadius = 20,
                FillColor = Color.White,
                ShadowDecoration = { Enabled = true, Depth = 10, BorderRadius = 20 }
            };
            this.Controls.Add(kartPanel);

            // Başlık
            Label lblBaslik = new Label()
            {
                Text = "EBOS GİRİŞ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 115, 47),
                AutoSize = true,
                Location = new Point((kartPanel.Width - 180) / 2, 25)
            };
            kartPanel.Controls.Add(lblBaslik);

            // E-posta
            txtEposta = new Guna2TextBox()
            {
                PlaceholderText = "E-posta",
                Size = new Size(280, 40),
                Location = new Point(40, 90),
                BorderRadius = 8
            };
            kartPanel.Controls.Add(txtEposta);

            // Şifre
            txtSifre = new Guna2TextBox()
            {
                PlaceholderText = "Şifre",
                PasswordChar = '*',
                Size = new Size(280, 40),
                Location = new Point(40, 150),
                BorderRadius = 8
            };
            kartPanel.Controls.Add(txtSifre);

            // Giriş Butonu
            btnGiris = new Guna2Button()
            {
                Text = "Giriş Yap",
                Size = new Size(280, 45),
                Location = new Point(40, 210),
                BorderRadius = 10,
                FillColor = Color.FromArgb(90, 115, 47),
                HoverState = { FillColor = Color.FromArgb(70, 100, 40) }
            };
            btnGiris.Click += BtnGiris_Click;
            kartPanel.Controls.Add(btnGiris);

            // Kayıt Ol
            linkKayit = new LinkLabel()
            {
                Text = "Hesabınız yok mu? Kayıt Ol",
                LinkColor = Color.FromArgb(90, 115, 47),
                ActiveLinkColor = Color.FromArgb(60, 80, 30),
                Location = new Point(40, 275),
                AutoSize = true
            };
            linkKayit.Click += (s, e) => { new KayitForm().Show(); this.Hide(); };
            kartPanel.Controls.Add(linkKayit);

            // Şifremi unuttum
            linkSifreUnuttum = new LinkLabel()
            {
                Text = "Şifremi unuttum",
                LinkColor = Color.FromArgb(120, 40, 40),
                ActiveLinkColor = Color.DarkRed,
                Location = new Point(40, 300),
                AutoSize = true
            };
            linkSifreUnuttum.Click += (s, e) => { new SifreSifirlamaForm().Show(); this.Hide(); };
            kartPanel.Controls.Add(linkSifreUnuttum);
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            string eposta = txtEposta.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (string.IsNullOrWhiteSpace(eposta) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen e-posta ve şifre giriniz.", "Uyarı");
                return;
            }

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower());

                if (kullanici == null)
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata");
                    return;
                }

                if (kullanici.Sifre != sifre)
                {
                    MessageBox.Show("Şifre hatalı.", "Hata");
                    return;
                }

                string rol = kullanici.Rol?.ToLowerInvariant();

                if (rol == "yonetici")
                {
                    new YoneticiPaneli().Show();
                }
                else if (rol == "kullanici" || rol == "kullanıcı")
                {
                    new KullaniciPaneli(kullanici.Eposta).Show();
                }
                else
                {
                    MessageBox.Show("Tanımlı olmayan bir rol: " + rol, "Hata");
                    return;
                }

                this.Hide();
            }
        }
    }
}
