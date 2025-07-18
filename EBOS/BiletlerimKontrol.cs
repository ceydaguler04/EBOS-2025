using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    public partial class BiletlerimKontrol : UserControl
    {
        private string kullaniciEposta;
        private DataGridView dgvBiletler;

        public BiletlerimKontrol(string eposta)
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.kullaniciEposta = eposta;

            Label lblBaslik = new Label()
            {
                Text = "🎫 BİLETLERİM",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            dgvBiletler = new DataGridView()
            {
                Location = new Point(30, 70),
                Size = new Size(800, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvBiletler);

            dgvBiletler.Columns.Add("Kategori", "Etkinlik Türü");
            dgvBiletler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
            dgvBiletler.Columns.Add("Tarih", "Tarih");
            dgvBiletler.Columns.Add("Koltuk", "Koltuk");
            dgvBiletler.Columns.Add("Fiyat", "Fiyat (₺)");

            BiletleriYukle();
        }

        private void BiletleriYukle()
        {
            dgvBiletler.Rows.Clear();

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == kullaniciEposta.ToLower());
                if (kullanici == null) return;

                var biletler = db.Biletler
                                 .Where(b => b.KullaniciID == kullanici.KullaniciID)
                                 .OrderByDescending(b => b.SatinAlmaTarihi)
                                 .ToList();

                foreach (var bilet in biletler)
                {
                    var seans = db.Seanslar.FirstOrDefault(s => s.SeansID == bilet.SeansID);
                    var etkinlik = db.Etkinlikler.FirstOrDefault(e => e.EtkinlikID == seans.EtkinlikID);
                    var tur = db.EtkinlikTurleri.FirstOrDefault(t => t.TurID == etkinlik.TurID);
                    var koltuk = db.Koltuklar.FirstOrDefault(k => k.KoltukID == bilet.KoltukID);

                    dgvBiletler.Rows.Add(
                        tur?.TurAdi ?? "Bilinmiyor",
                        etkinlik?.EtkinlikAdi ?? "Etkinlik Yok",
                        seans?.Tarih.ToShortDateString() ?? "-",
                        koltuk?.KoltukNo ?? "-",
                        bilet.Fiyat.ToString("0.00")
                    );
                }
            }
        }
    }
}
