using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Microsoft.EntityFrameworkCore;

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

            // Kolonlar
            dgvBiletler.Columns.Add("Kategori", "Etkinlik Türü");
            dgvBiletler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
            dgvBiletler.Columns.Add("Tarih", "Tarih");
            dgvBiletler.Columns.Add("Koltuk", "Koltuk");
            dgvBiletler.Columns.Add("Fiyat", "Fiyat (₺)");
            dgvBiletler.Columns.Add("Durum", "Durum");
            dgvBiletler.Columns.Add("BiletID", "BiletID");
            dgvBiletler.Columns["BiletID"].Visible = false;

            dgvBiletler.Columns.Add(new DataGridViewButtonColumn()

            {
                HeaderText = "QR Kod",
                Text = "QR Gör",
                Name = "BtnQrKod",
                UseColumnTextForButtonValue = true
            });

            dgvBiletler.Columns.Add(new DataGridViewButtonColumn()
            {
                HeaderText = "İşlem",
                Name = "Iptal",
                Text = "İptal Et",
                UseColumnTextForButtonValue = true
            });

            dgvBiletler.CellClick += DgvBiletler_CellClick;

            BiletleriYukle();
        }

        private void BiletleriYukle()
        {
            dgvBiletler.Rows.Clear();

            using var db = new AppDbContext();

            var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == kullaniciEposta.ToLower());
            if (kullanici == null)
            {
                MessageBox.Show("Kullanıcı bulunamadı. E-posta: " + kullaniciEposta);
                return;
            }

            var seanslar = db.Seanslar.ToList();
            var etkinlikler = db.Etkinlikler.ToList();
            var turler = db.EtkinlikTurleri.ToList();
            var koltuklar = db.Koltuklar.ToList();

            var biletler = db.Biletler
                .Where(b => b.KullaniciID == kullanici.KullaniciID)

                .ToList();

            var biletVeTurListesi = new List<(Bilet bilet, string turAdi)>();

            foreach (var bilet in biletler)
            {
                var seans = seanslar.FirstOrDefault(s => s.SeansID == bilet.SeansID);
                var etkinlik = etkinlikler.FirstOrDefault(e => e.EtkinlikID == seans?.EtkinlikID);
                var tur = turler.FirstOrDefault(t => t.TurID == etkinlik?.TurID);
                var turAdi = tur?.TurAdi ?? "Bilinmiyor";

                biletVeTurListesi.Add((bilet, turAdi));
            }

            var siraliListe = biletVeTurListesi.OrderBy(x => x.turAdi).ToList();

            foreach (var (bilet, turAdi) in siraliListe)
            {
                var seans = seanslar.FirstOrDefault(s => s.SeansID == bilet.SeansID);
                var etkinlik = etkinlikler.FirstOrDefault(e => e.EtkinlikID == seans?.EtkinlikID);
                var koltuk = koltuklar.FirstOrDefault(k => k.KoltukID == bilet.KoltukID);

                //string etkinlikAdi = etkinlik?.EtkinlikAdi ?? "Etkinlik Yok";
                //string tarih = seans?.Tarih.ToString("yyyy-MM-dd") ?? "Yok";
                //string koltukNo = koltuk?.KoltukNo ?? "Yok";
                //string fiyat = bilet.Fiyat.ToString("C2");
                //string durum = (seans?.Tarih < DateTime.Today || !bilet.AktifMi) ? "Pasif" : "Aktif";

                //int rowIndex = dgvBiletler.Rows.Add();
                //dgvBiletler.Rows[rowIndex].Cells["Kategori"].Value = turAdi;
                //dgvBiletler.Rows[rowIndex].Cells["EtkinlikAdi"].Value = etkinlikAdi;
                //dgvBiletler.Rows[rowIndex].Cells["Tarih"].Value = tarih;
                //dgvBiletler.Rows[rowIndex].Cells["Koltuk"].Value = koltukNo;
                //dgvBiletler.Rows[rowIndex].Cells["Fiyat"].Value = fiyat;
                //dgvBiletler.Rows[rowIndex].Cells["Durum"].Value = durum;
                //dgvBiletler.Rows[rowIndex].Cells["BiletID"].Value = bilet.BiletID;
                string etkinlikAdi = etkinlik?.EtkinlikAdi ?? "Etkinlik Yok";
                string tarih = seans?.Tarih.ToString("yyyy-MM-dd") ?? "Yok";
                string koltukNo = koltuk?.KoltukNo ?? "Yok";
                string fiyat = bilet.Fiyat.ToString("C2");
                string durum = (seans?.Tarih < DateTime.Today || !bilet.AktifMi) ? "Pasif" : "Aktif";

                int rowIndex = dgvBiletler.Rows.Add();
                dgvBiletler.Rows[rowIndex].Cells["Kategori"].Value = turAdi;
                dgvBiletler.Rows[rowIndex].Cells["EtkinlikAdi"].Value = etkinlikAdi;
                dgvBiletler.Rows[rowIndex].Cells["Tarih"].Value = tarih;
                dgvBiletler.Rows[rowIndex].Cells["Koltuk"].Value = koltukNo;
                dgvBiletler.Rows[rowIndex].Cells["Fiyat"].Value = fiyat;
                dgvBiletler.Rows[rowIndex].Cells["Durum"].Value = durum;
                dgvBiletler.Rows[rowIndex].Cells["BiletID"].Value = bilet.BiletID;
                if (durum == "Pasif")
                {
                    var row = dgvBiletler.Rows[rowIndex];
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;

                    if (row.Cells["Iptal"] is DataGridViewButtonCell btnCell)
                    {
                        btnCell.FlatStyle = FlatStyle.Popup;
                        btnCell.Style.ForeColor = Color.Gray;
                        btnCell.Style.BackColor = Color.LightGray;
                        btnCell.ReadOnly = true;
                        btnCell.Value = "";
                    }
                }
            }
        }
        //    var biletler = db.Biletler.Where(b => b.KullaniciID == kullanici.KullaniciID).ToList();

        //    // İlişkili verileri topluca çekiyoruz
        //    var seanslar = db.Seanslar.ToList();
        //    var etkinlikler = db.Etkinlikler.ToList();
        //    var turler = db.EtkinlikTurleri.ToList();
        //    var koltuklar = db.Koltuklar.ToList();

        //    foreach (var bilet in biletler)
        //    {
        //        var seans = seanslar.FirstOrDefault(x => x.SeansID == bilet.SeansID);
        //        var etkinlik = etkinlikler.FirstOrDefault(e => e.EtkinlikID == seans?.EtkinlikID);
        //        var tur = turler.FirstOrDefault(t => t.TurID == etkinlik?.TurID);
        //        var koltuk = koltuklar.FirstOrDefault(k => k.KoltukID == bilet.KoltukID);

        //        string turAdi = tur?.TurAdi ?? "Bilinmiyor";
        //        string etkinlikAdi = etkinlik?.EtkinlikAdi ?? "Etkinlik Yok";
        //        string tarih = seans?.Tarih.ToString("yyyy-MM-dd") ?? "Yok";
        //        string koltukNo = koltuk?.KoltukNo ?? "Yok";
        //        string fiyat = bilet.Fiyat.ToString("C2");

        //        string durum = (seans?.Tarih.Date < DateTime.Today || !bilet.AktifMi) ? "Pasif" : "Aktif";

        //        int rowIndex = dgvBiletler.Rows.Add();
        //        dgvBiletler.Rows[rowIndex].Cells["Kategori"].Value = turAdi;
        //        dgvBiletler.Rows[rowIndex].Cells["EtkinlikAdi"].Value = etkinlikAdi;
        //        dgvBiletler.Rows[rowIndex].Cells["Tarih"].Value = tarih;
        //        dgvBiletler.Rows[rowIndex].Cells["Koltuk"].Value = koltukNo;
        //        dgvBiletler.Rows[rowIndex].Cells["Fiyat"].Value = fiyat;
        //        dgvBiletler.Rows[rowIndex].Cells["Durum"].Value = durum;
        //        dgvBiletler.Rows[rowIndex].Cells["BiletID"].Value = bilet.BiletID;
        //        if (durum == "Pasif")
        //        {
        //            var row = dgvBiletler.Rows[rowIndex];
        //            row.DefaultCellStyle.BackColor = Color.LightGray;
        //            row.DefaultCellStyle.ForeColor = Color.DarkGray;

        //            if (row.Cells["Iptal"] is DataGridViewButtonCell btnCell)
        //            {
        //                btnCell.FlatStyle = FlatStyle.Popup;
        //                btnCell.Style.ForeColor = Color.Gray;
        //                btnCell.Style.BackColor = Color.LightGray;
        //                btnCell.ReadOnly = true;
        //                btnCell.Value = "";
        //            }
        //        }
        //    }
        //}


        private void DgvBiletler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0) return;

            //// QR Kod Gösterme
            //if (dgvBiletler.Columns[e.ColumnIndex].Name == "BtnQrKod")
            //{
            //    string etkinlikAdi = dgvBiletler.Rows[e.RowIndex].Cells["EtkinlikAdi"].Value?.ToString();
            //    string koltukNo = dgvBiletler.Rows[e.RowIndex].Cells["Koltuk"].Value?.ToString();

            //    if (!string.IsNullOrEmpty(etkinlikAdi) && !string.IsNullOrEmpty(koltukNo))
            //    {
            //        var qrForm = new FormQrGoster(etkinlikAdi, koltukNo);
            //        qrForm.ShowDialog();
            //    }
            //}// QR Kod Gösterme
            if (e.ColumnIndex >= 0 && dgvBiletler.Columns[e.ColumnIndex].Name == "BtnQrKod")
            {
                if (e.RowIndex < 0) return;

                string etkinlikAdi = dgvBiletler.Rows[e.RowIndex].Cells["EtkinlikAdi"].Value?.ToString();
                string koltukNo = dgvBiletler.Rows[e.RowIndex].Cells["Koltuk"].Value?.ToString();

                if (!string.IsNullOrEmpty(etkinlikAdi) && !string.IsNullOrEmpty(koltukNo))
                {
                    var qrForm = new FormQrGoster(etkinlikAdi, koltukNo);
                    qrForm.ShowDialog();
                }
            }


            // Bilet İptal
            //else if (dgvBiletler.Columns[e.ColumnIndex].Name == "Iptal")
            //{
            //    string durum = dgvBiletler.Rows[e.RowIndex].Cells["Durum"].Value?.ToString();
            //    if (durum != "Aktif")
            //    {
            //        MessageBox.Show("Bu bilet zaten pasif.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    DialogResult onay = MessageBox.Show("Bu bileti iptal etmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (onay != DialogResult.Yes) return;

            //    int biletId = Convert.ToInt32(dgvBiletler.Rows[e.RowIndex].Cells["BiletID"].Value);

            //    using var db = new AppDbContext();
            //    var bilet = db.Biletler.FirstOrDefault(b => b.BiletID == biletId);
            //    if (bilet != null)
            //    {
            //        bilet.AktifMi = false;
            //        db.SaveChanges();
            //        MessageBox.Show("Bilet iptal edildi.");
            //        BiletleriYukle();
            //    }
            //}else if (e.ColumnIndex >= 0 && dgvBiletler.Columns[e.ColumnIndex].Name == "Iptal")
            {
                if (e.RowIndex < 0) return;

                string durum = dgvBiletler.Rows[e.RowIndex].Cells["Durum"].Value?.ToString();
                if (durum != "Aktif")
                {
                    MessageBox.Show("Bu bilet zaten pasif.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult onay = MessageBox.Show("Bu bileti iptal etmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (onay != DialogResult.Yes) return;

                int biletId = Convert.ToInt32(dgvBiletler.Rows[e.RowIndex].Cells["BiletID"].Value);

                using var db = new AppDbContext();
                var bilet = db.Biletler.FirstOrDefault(b => b.BiletID == biletId);
                if (bilet != null)
                {
                    bilet.AktifMi = false;
                    db.SaveChanges();
                    MessageBox.Show("Bilet iptal edildi.");
                    BiletleriYukle();
                }
            }

        }
        private void BiletlerimKontrol_Load (object sender, EventArgs e)
        {// BiletleriYukle();
         dgvBiletler.Columns.Clear();

            dgvBiletler.Columns.Add("Kategori", "Kategori");
            dgvBiletler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
            dgvBiletler.Columns.Add("Tarih", "Tarih");
            dgvBiletler.Columns.Add("Koltuk", "Koltuk");
            dgvBiletler.Columns.Add("Fiyat", "Fiyat");
            dgvBiletler.Columns.Add("Durum", "Durum");

            // Gizli ID kolonu (işlem için)
            var biletIdCol = new DataGridViewTextBoxColumn();
            biletIdCol.Name = "BiletID";
            biletIdCol.Visible = false;
            dgvBiletler.Columns.Add(biletIdCol);

            // İptal Butonu
            var iptalCol = new DataGridViewButtonColumn();
            iptalCol.Name = "Iptal";
            iptalCol.HeaderText = "İptal";
            iptalCol.Text = "İptal Et";
            iptalCol.UseColumnTextForButtonValue = true;
            dgvBiletler.Columns.Add(iptalCol);
        }
    }
}


