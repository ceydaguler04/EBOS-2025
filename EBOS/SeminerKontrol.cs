using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class SeminerKontrol : UserControl
    {
        private string kullaniciEposta;
        private FlowLayoutPanel flpKartlar;

        public SeminerKontrol(string eposta)
        {
            kullaniciEposta = eposta;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            KartArayuzuOlustur();
        }

        private void KartArayuzuOlustur()
        {
            flpKartlar = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(15),
                BackColor = TemaYonetici.AktifTema == "Koyu" ? Color.FromArgb(120, 120, 120) : Color.White
            };
            this.Controls.Add(flpKartlar);

            var sahteEtkinlikler = new[]
            {
                new { EtkinlikAdi = "Seminer 1", Gorsel = "seminer1.jpg" },
                new { EtkinlikAdi = "Seminer 2", Gorsel = "seminer2.jpg" },
                new { EtkinlikAdi = "Seminer 3", Gorsel = "seminer3.jpg" },
                new { EtkinlikAdi = "Seminer 4", Gorsel = "seminer1.jpg" },
                new { EtkinlikAdi = "Seminer 5", Gorsel = "seminer2.jpg" }
            };

            foreach (var etkinlik in sahteEtkinlikler)
            {
                var kart = new Guna2Panel()
                {
                    Size = new Size(240, 360),
                    BorderRadius = 15,
                    FillColor = Color.White,
                    Margin = new Padding(15),
                    ShadowDecoration = { Enabled = true, Depth = 10 }
                };

                var pb = new PictureBox()
                {
                    ImageLocation = Path.Combine(Application.StartupPath, "Gorseller", etkinlik.Gorsel),
                    Size = new Size(220, 140),
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                var lblAd = new Label()
                {
                    Text = etkinlik.EtkinlikAdi,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(10, 160),
                    AutoSize = true
                };


                var btnBiletAl = new Guna2Button()
                {
                    Text = "Bilet Al",
                    Size = new Size(100, 35),
                    Location = new Point(10, 280),
                    FillColor = Color.FromArgb(40, 120, 80),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BorderRadius = 6
                };
                btnBiletAl.Click += (s, e) =>
                {
                    var form = new BiletAlForm(etkinlik.EtkinlikAdi, kullaniciEposta);
                    form.ShowDialog();
                };

                var btnDegerlendir = new Guna2Button()
                {
                    Text = "Değerlendir",
                    Size = new Size(100, 35),
                    Location = new Point(120, 280),
                    FillColor = Color.FromArgb(100, 100, 160),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BorderRadius = 6
                };
                btnDegerlendir.Click += (s, e) =>
                {
                    var form = new DegerlendirForm(etkinlik.EtkinlikAdi, kullaniciEposta);
                    form.ShowDialog();
                };

                kart.Controls.Add(pb);
                kart.Controls.Add(lblAd);
                kart.Controls.Add(btnBiletAl);
                kart.Controls.Add(btnDegerlendir);
                flpKartlar.Controls.Add(kart);
            }
        }
    }
}
