using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace EBOS
{
    public partial class DashboardKontrol : UserControl
    {
        private Guna2HtmlLabel lblGrafikBaslik;
        private Guna2Panel grafikPanel;

        public DashboardKontrol()
        {
            InitializeComponent();
            this.BackColor = Color.WhiteSmoke;

            // 🟦 Kart 1 - Toplam Etkinlik
            var kart1 = CreateStatCard("Toplam Etkinlik", "12", IconChar.Video, Color.FromArgb(229,38,164/*72, 219, 208*/), new Point(30, 40));
            this.Controls.Add(kart1);

            // 🟦 Kart 2 - Toplam Seans
            var kart2 = CreateStatCard("Toplam Seans", "45", IconChar.Clock, Color.FromArgb(52, 152, 219), new Point(230, 40));
            this.Controls.Add(kart2);

            // 🟦 Kart 3 - Aktif Kampanya
            var kart3 = CreateStatCard("Aktif Kampanya", "3", IconChar.Tags, Color.FromArgb(241, 196, 15), new Point(430, 40));
            this.Controls.Add(kart3);

            // 🟦 Kart 4 - Toplam Kullanıcı
            var kart4 = CreateStatCard("Toplam Kullanıcı", "78", IconChar.UserLock, Color.FromArgb(155, 89, 182), new Point(630, 40));
            this.Controls.Add(kart4);

            // Başlık
            lblGrafikBaslik = new Guna2HtmlLabel()
            {
                Text = "Genel İstatistikler",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 150),
                ForeColor = Color.DimGray,
                AutoSize = true
            };
            this.Controls.Add(lblGrafikBaslik);

            // Panel
            grafikPanel = new Guna2Panel()
            {
                Location = new Point(30, 180),
                Size = new Size(800, 300),
                BorderRadius = 10,
                FillColor = Color.White,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1
            };
            this.Controls.Add(grafikPanel);
        }

        private Guna2Panel CreateStatCard(string baslik, string deger, IconChar ikon, Color arkaPlanRenk, Point konum)
        {
            int kartGenislik = 175;
            int kartYukseklik = 90;

            var panel = new Guna2Panel()
            {
                Size = new Size(kartGenislik, kartYukseklik),
                Location = konum,
                BorderRadius = 12,
                FillColor = arkaPlanRenk
            };

            var icon = new IconPictureBox()
            {
                IconChar = ikon,
                IconColor = Color.White,
                Size = new Size(32, 32),
                Location = new Point(12, 10),
                BackColor = Color.Transparent
            };

            var lblDeger = new Label()
            {
                Text = deger,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(50, 12),
                Size = new Size(110, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblBaslik = new Label()
            {
                Text = baslik,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(12, 45),
                AutoSize = false,
                Size = new Size(kartGenislik - 24, 30),
                TextAlign = ContentAlignment.TopLeft
            };

            panel.Controls.Add(icon);
            panel.Controls.Add(lblDeger);
            panel.Controls.Add(lblBaslik);

            return panel;
        }
        private void DashboardKontrol_Load(object sender, EventArgs e)
        {
            if (YoneticiPaneli.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                this.BackColor = Color.White;
            }

        }
    }

}