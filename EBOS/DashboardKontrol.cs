using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using EBOS.DataAccess;
using EBOS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization; // en üste eklemeyi unutma


namespace EBOS
{
    public partial class DashboardKontrol : UserControl
    {
        private Guna2HtmlLabel lblGrafikBaslik;
        private Guna2Panel grafikPanel;
        private Guna2Panel kart1, kart2, kart3, kart4;
        private Label lblDeger1, lblDeger2, lblDeger3, lblDeger4;

        public DashboardKontrol()
        {
            InitializeComponent();
            this.BackColor = Color.WhiteSmoke;

            kart1 = CreateStatCard("Toplam Etkinlik", out lblDeger1, IconChar.Video, Color.FromArgb(229, 38, 164), new Point(30, 40));
            kart2 = CreateStatCard("Toplam Bilet", out lblDeger2, IconChar.Ticket, Color.FromArgb(52, 152, 219), new Point(230, 40));
            kart3 = CreateStatCard("Aktif Kampanya", out lblDeger3, IconChar.Tags, Color.FromArgb(241, 196, 15), new Point(430, 40));
            kart4 = CreateStatCard("Toplam Üye", out lblDeger4, IconChar.UserLock, Color.FromArgb(155, 89, 182), new Point(630, 40));

            this.Controls.Add(kart1);
            this.Controls.Add(kart2);
            this.Controls.Add(kart3);
            this.Controls.Add(kart4);

            // Başlık
            lblGrafikBaslik = new Guna2HtmlLabel()
            {
                Text = "Genel İstatistikler",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 150),
                ForeColor = Color.DimGray,
                BackColor = Color.White,
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
            this.Load += DashboardKontrol_Load;
        }
        private Guna2Panel CreateStatCard(string baslik, out Label lblDeger, IconChar ikon, Color arkaPlanRenk, Point konum)
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

            lblDeger = new Label()
            {
                Text = "0", // Varsayılan değer
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
        private void KategoriGrafikOlustur()
        {
            grafikPanel.Controls.Clear(); // Önceki grafik varsa temizle

            // 🎯 1. GRAFİK — Kategoriye Göre Etkinlik (PASTA GRAFİK)
            var chart1 = new Chart()
            {
                Size = new Size(360, 280),
                Location = new Point(20, 10),
                BackColor = Color.Transparent,
                BorderlineColor = Color.LightGray,
                BorderlineDashStyle = ChartDashStyle.Solid,
                BorderlineWidth = 1
            };

            ChartArea area1 = new ChartArea();
            chart1.ChartAreas.Add(area1);

            Series seri1 = new Series()
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Label = "#PERCENT",
                LegendText = "#VALX",
                ToolTip = "#VALX: #VAL adet (%#PERCENT)"
            };
            seri1["PieDrawingStyle"] = "SoftEdge";
            seri1.SmartLabelStyle.Enabled = true;

            // 🎨 Renkler
            Color[] renkler = { Color.SteelBlue, Color.Orange, Color.IndianRed, Color.MediumSeaGreen, Color.MediumPurple };

            // 📋 Legend (açıklama)
            chart1.Legends.Add(new Legend("Legend1")
            {
                Docking = Docking.Right,
                Font = new Font("Segoe UI", 9),
                Alignment = StringAlignment.Center
            });
            seri1.Legend = "Legend1";

            // 🔗 Verileri çek ve ekle
            using (var context = new AppDbContext())
            {
                var etkinlikler = context.Etkinlikler.Select(e => new { e.TurID }).ToList();
                var turler = context.EtkinlikTurleri.Select(t => new { t.TurID, t.TurAdi }).ToList();

                var kategoriVerileri = etkinlikler
                    .GroupBy(e => e.TurID)
                    .Join(turler, g => g.Key, t => t.TurID,
                        (g, t) => new { Kategori = t.TurAdi, EtkinlikSayisi = g.Count() })
                    .GroupBy(x => x.Kategori)
                    .Select(g => new { Kategori = g.Key, EtkinlikSayisi = g.Sum(x => x.EtkinlikSayisi) })
                    .ToList();

                for (int i = 0; i < kategoriVerileri.Count; i++)
                {
                    var item = kategoriVerileri[i];
                    var dp = seri1.Points.AddXY(item.Kategori, item.EtkinlikSayisi);
                    if (i < renkler.Length)
                        seri1.Points[dp].Color = renkler[i];
                }
            }

            chart1.Series.Add(seri1);
            grafikPanel.Controls.Add(chart1);
        }

        private async void DashboardKontrol_Load(object sender, EventArgs e)
        {
            if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                this.BackColor = Color.White;
            }
            using (var context = new AppDbContext())
            {
                // Güncel sayıları çekiyoruz
                int toplamEtkinlik = await context.Etkinlikler.CountAsync();
                int toplamBilet = await context.Biletler.CountAsync(); // ya da Ticket, Bilet, SatilanBilet tablosunun adı neyse
                int aktifKampanya = await context.Kampanyalar.CountAsync(k => k.BaslangicTarihi <= DateTime.Now && k.BitisTarihi >= DateTime.Now);
                int toplamKullanici = await context.Kullanicilar.CountAsync();

                // Label'lara yazdır
                lblDeger1.Text = toplamEtkinlik.ToString();
                lblDeger2.Text = toplamBilet.ToString();
                lblDeger3.Text = aktifKampanya.ToString();
                lblDeger4.Text = toplamKullanici.ToString();

                KategoriGrafikOlustur();
            }
        }
    }

}