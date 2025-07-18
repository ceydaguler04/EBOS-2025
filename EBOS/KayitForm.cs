using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    public partial class KayitForm : Form
    {
        public KayitForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(430, 620);
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
            Guna2Panel kart = new Guna2Panel()
            {
                Size = new Size(360, 500),
                Location = new Point((this.Width - 360) / 2, 60),
                BorderRadius = 20,
                FillColor = Color.White,
                ShadowDecoration = { Enabled = true, Depth = 10 }
            };
            this.Controls.Add(kart);

            Label lblBaslik = new Label()
            {
                Text = "EBOS KAYIT",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 115, 47),
                AutoSize = true,
                Location = new Point((kart.Width - 180) / 2, 20)
            };
            kart.Controls.Add(lblBaslik);

            var txtAdSoyad = new Guna2TextBox() { PlaceholderText = "Ad Soyad", Location = new Point(40, 70), Size = new Size(280, 40), BorderRadius = 6 };
            var txtEposta = new Guna2TextBox() { PlaceholderText = "E-posta", Location = new Point(40, 120), Size = new Size(280, 40), BorderRadius = 6 };
            var txtSifre = new Guna2TextBox() { PlaceholderText = "Şifre", Location = new Point(40, 170), Size = new Size(280, 40), PasswordChar = '*', BorderRadius = 6 };
            var txtTekrar = new Guna2TextBox() { PlaceholderText = "Şifre (Tekrar)", Location = new Point(40, 220), Size = new Size(280, 40), PasswordChar = '*', BorderRadius = 6 };
            var cmbRol = new Guna2ComboBox() { Location = new Point(40, 270), Size = new Size(280, 40), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRol.Items.AddRange(new string[] { "Kullanıcı", "Yönetici" });

            var btnKayit = new Guna2Button()
            {
                Text = "Kayıt Ol",
                Location = new Point(40, 330),
                Size = new Size(280, 45),
                BorderRadius = 10,
                FillColor = Color.FromArgb(90, 115, 47),
                HoverState = { FillColor = Color.FromArgb(70, 100, 40) }
            };

            btnKayit.Click += (s, e) =>
            {
                string adSoyad = txtAdSoyad.Text.Trim();
                string eposta = txtEposta.Text.Trim();
                string sifre = txtSifre.Text.Trim();
                string tekrar = txtTekrar.Text.Trim();
                string rol = cmbRol.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(eposta) ||
                    string.IsNullOrWhiteSpace(sifre) || string.IsNullOrWhiteSpace(tekrar) || string.IsNullOrWhiteSpace(rol))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı");
                    return;
                }

                if (sifre != tekrar)
                {
                    MessageBox.Show("Şifreler uyuşmuyor!", "Hata");
                    return;
                }

                using (var db = new AppDbContext())
                {
                    if (db.Kullanicilar.Any(k => k.Eposta == eposta))
                    {
                        MessageBox.Show("Bu e-posta zaten kayıtlı!", "Hata");
                        return;
                    }

                    db.Kullanicilar.Add(new Kullanici { AdSoyad = adSoyad, Eposta = eposta, Sifre = sifre, Rol = rol });
                    db.SaveChanges();
                }

                MessageBox.Show("Kayıt başarıyla tamamlandı!", "Bilgi");
                new GirisForm().Show();
                this.Hide();
            };

            var linkGiris = new LinkLabel()
            {
                Text = "Zaten hesabın var mı? Giriş yap",
                LinkColor = Color.FromArgb(90, 115, 47),
                ActiveLinkColor = Color.FromArgb(60, 80, 30),
                Location = new Point(40, 390),
                AutoSize = true
            };
            linkGiris.Click += (s, e) => { new GirisForm().Show(); this.Hide(); };

            kart.Controls.AddRange(new Control[]
            {
                txtAdSoyad, txtEposta, txtSifre, txtTekrar, cmbRol, btnKayit, linkGiris
            });
        }
    }
}
