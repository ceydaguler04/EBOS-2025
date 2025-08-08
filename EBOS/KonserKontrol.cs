using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;

namespace EBOS
{
    public partial class KonserKontrol : UserControl
    {
        private readonly string kullaniciEposta;
        private FlowLayoutPanel flpKartlar;
        private Guna2TextBox txtArama;

        public KonserKontrol(string eposta)
        {
            kullaniciEposta = eposta;
            InitializeComponent();
            Dock = DockStyle.Fill;

            KartArayuzuOlustur();
            KonserEtkinlikleriYukle();
        }

        /* -------------------------------------------------  UI  ------------------------------------------------- */
        private void KartArayuzuOlustur()
        {
            // 🔍 Arama kutusu
            txtArama = new Guna2TextBox
            {
                PlaceholderText = "Etkinlik adına göre ara",
                Size = new Size(300, 38),
                Location = new Point(15, 15),
                Font = new Font("Segoe UI", 10),
                BorderRadius = 10,
                BorderThickness = 1,
                BorderColor = Color.Silver,
                FillColor = Color.Transparent,
                BackColor = Color.Transparent
            };
            txtArama.TextChanged += (_, __) => KonserEtkinlikleriYukle(txtArama.Text);

            var ustPanel = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = BackColor
            };
            ustPanel.Controls.Add(txtArama);
            Controls.Add(ustPanel);

            // 📇 Kart alanı
            flpKartlar = new FlowLayoutPanel
            {
                Location = new Point(5, 75),
                Size = new Size(Width - 10, Height - 85),
                AutoScroll = true,

                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(flpKartlar);
        }

        /* --------------------------------------------  Veri & Kart  --------------------------------------------- */
        private void KonserEtkinlikleriYukle(string filtre = "")
        {
            flpKartlar.Controls.Clear();

            using var db = new AppDbContext();

            int kategoriId = db.EtkinlikTurleri
                               .Where(t => t.TurAdi.ToLower() == "konser")
                               .Select(t => t.TurID)
                               .FirstOrDefault();

            var etkinlikler = db.Etkinlikler
                                .Include(e => e.EtkinlikTuru)
                                .Where(e => e.TurID == kategoriId &&
                                       (string.IsNullOrEmpty(filtre) ||
                                        e.EtkinlikAdi.ToLower().Contains(filtre.ToLower())))
                                .OrderByDescending(e => e.Tarih)
                                .ToList();

            foreach (var e in etkinlikler)
                flpKartlar.Controls.Add(KartOlustur(e));

            if (etkinlikler.Count == 0)
            {
                flpKartlar.Controls.Add(new Label
                {
                    Text = "Konser etkinliği bulunamadı.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Padding = new Padding(20)
                });
            }
        }

        private Control KartOlustur(Etkinlik e)
        {
            var kart = new Guna2Panel
            {
                Size = new Size(180, 340),   // standart kart
                BorderRadius = 12,
                FillColor = Color.White,
                Margin = new Padding(10),
                ShadowDecoration = { Enabled = true, Depth = 6 }
            };

            var pb = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(10, 10),
                Size = new Size(160, 90)
            };
            try { pb.Load(e.GorselYolu); } catch { }
            kart.Controls.Add(pb);

            kart.Controls.Add(new Label
            {
                Text = e.EtkinlikAdi,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(10, 110),
                Size = new Size(160, 40),
                AutoEllipsis = true
            });

            kart.Controls.Add(new Label
            {
                Text = $"📅 {e.Tarih:dd.MM.yyyy}\n⏰ {e.Saat:hh\\:mm}",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(10, 150),
                Size = new Size(160, 28)
            });

            kart.Controls.Add(new Label
            {
                Text = $"Süre : {e.SureDakika} dk\nTür  : {e.EtkinlikTuru.TurAdi}",
                Font = new Font("Segoe UI", 8),
                Location = new Point(10, 180),
                Size = new Size(160, 32)
            });

            var btnBilet = new Guna2Button
            {
                Text = "Bilet Al",
                Size = new Size(70, 28),
                Location = new Point(10, 290),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FillColor = Color.FromArgb(40, 120, 80),
                ForeColor = Color.White,
                BorderRadius = 5
            };
            btnBilet.Click += (_, __) => new BiletAlForm(e.EtkinlikAdi, kullaniciEposta).ShowDialog();
            kart.Controls.Add(btnBilet);

            var btnPuan = new Guna2Button
            {
                Text = "Puanla",
                Size = new Size(70, 28),
                Location = new Point(100, 290),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FillColor = Color.FromArgb(100, 100, 160),
                ForeColor = Color.White,
                BorderRadius = 5
            };
            btnPuan.Click += (_, __) => new DegerlendirForm(e.EtkinlikAdi, kullaniciEposta).ShowDialog();
            kart.Controls.Add(btnPuan);

            return kart;
        }
    }
}



