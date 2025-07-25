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
    public partial class SifreSifirlamaForm : Form
    {
        private Guna2TextBox txtEposta;

        public SifreSifirlamaForm()
        {
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(430, 400);
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
            var panel = new Guna2Panel()
            {
                Size = new Size(360, 260),
                Location = new Point((this.Width - 360) / 2, 80),
                BorderRadius = 20,
                FillColor = Color.White,
                ShadowDecoration = { Enabled = true, Depth = 10 }
            };
            this.Controls.Add(panel);

            Label lblBaslik = new Label()
            {
                Text = "ŞİFRE SIFIRLAMA",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 115, 47),
                AutoSize = true,
                Location = new Point((panel.Width - 220) / 2, 20)
            };
            panel.Controls.Add(lblBaslik);

            txtEposta = new Guna2TextBox()
            {
                PlaceholderText = "E-posta adresiniz",
                Location = new Point(40, 70),
                Size = new Size(280, 40),
                BorderRadius = 6
            };
            panel.Controls.Add(txtEposta);

            var btnSifirla = new Guna2Button()
            {
                Text = "Şifreyi Sıfırla",
                Location = new Point(40, 130),
                Size = new Size(280, 45),
                BorderRadius = 10,
                FillColor = Color.FromArgb(90, 115, 47),
                HoverState = { FillColor = Color.FromArgb(70, 100, 40) }
            };
            btnSifirla.Click += BtnSifreSifirla_Click;
            panel.Controls.Add(btnSifirla);

            var linkGiris = new LinkLabel()
            {
                Text = "Girişe dön",
                LinkColor = Color.FromArgb(90, 115, 47),
                ActiveLinkColor = Color.FromArgb(60, 80, 30),
                Location = new Point(40, 190),
                AutoSize = true
            };
            linkGiris.Click += (s, e) => { new GirisForm().Show(); this.Hide(); };
            panel.Controls.Add(linkGiris);
        }

        private void BtnSifreSifirla_Click(object sender, EventArgs e)
        {
            string girilenEposta = txtEposta.Text.Trim();

            if (string.IsNullOrEmpty(girilenEposta))
            {
                MessageBox.Show("Lütfen e-posta adresinizi girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var kullanici = context.Kullanicilar.FirstOrDefault(k => k.Eposta == girilenEposta);

                if (kullanici == null)
                {
                    MessageBox.Show("Bu e-posta adresi ile kayıtlı bir kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string yeniSifre = SifreUret(8);
                kullanici.Sifre = yeniSifre;
                context.SaveChanges();

                MessageBox.Show($"Yeni şifreniz: {yeniSifre}\nLütfen giriş ekranından tekrar giriş yapın.", "Şifre Sıfırlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                new GirisForm().Show();
                this.Hide();
            }
        }

        private string SifreUret(int uzunluk)
        {
            const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(karakterler, uzunluk).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
