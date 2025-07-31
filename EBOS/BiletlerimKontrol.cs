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
                Size = new Size(750, 380),
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
            dgvBiletler.Columns.Add("Durum", "Durum");

            // QR butonu kolonu
            DataGridViewButtonColumn btnQr = new DataGridViewButtonColumn
            {
                HeaderText = "QR Kod",
                Text = "QR Gör",
                UseColumnTextForButtonValue = true,
                Name = "BtnQrKod" // <<< Bunu ekledik
            };
            dgvBiletler.Columns.Add(btnQr);


            dgvBiletler.CellClick += DgvBiletler_CellClick;

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

                    string durum = (seans.Tarih.Date < DateTime.Today) ? "Pasif" : "Aktif";

                    int rowIndex = dgvBiletler.Rows.Add(
                        tur?.TurAdi ?? "Bilinmiyor",
                        etkinlik?.EtkinlikAdi ?? "Etkinlik Yok",
                        seans?.Tarih.ToShortDateString() ?? "-",
                        koltuk?.KoltukNo ?? "-",
                        bilet.Fiyat.ToString("0.00"),
                        durum
                    );

                    // Pasif biletler gri olsun
                    if (durum == "Pasif")
                        dgvBiletler.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void DgvBiletler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvBiletler.Columns["BtnQrKod"].Index)
            {
                string etkinlikAdi = dgvBiletler.Rows[e.RowIndex].Cells["EtkinlikAdi"].Value?.ToString();
                string koltukNo = dgvBiletler.Rows[e.RowIndex].Cells["Koltuk"].Value?.ToString();

                if (!string.IsNullOrEmpty(etkinlikAdi) && !string.IsNullOrEmpty(koltukNo))
                {
                    FormQrGoster qrForm = new FormQrGoster(etkinlikAdi, koltukNo);
                    qrForm.ShowDialog();
                }
            }
        }

    }
}

