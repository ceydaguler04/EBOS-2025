using System;
using System.Drawing;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    public partial class BiletAlForm : Form
    {
        private string kullaniciEposta;
        private string kategori;

        private ComboBox cmbSeans;
        private ComboBox cmbKoltuk;
        private NumericUpDown nudAdet;
        private CheckBox chkKampanya;
        private Label lblFiyat;

        public BiletAlForm(string kategori, string eposta)
        {
            this.kategori = kategori;
            this.kullaniciEposta = eposta;

            this.Text = "Bilet Satın Al";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            ArayuzOlustur();
        }

        private void ArayuzOlustur()
        {
            using (var db = new AppDbContext())
            {
                var seanslar = db.Seanslar
                    .Where(s => s.Etkinlik.EtkinlikTuru.TurAdi == kategori)
                    .ToList();

                cmbSeans.DataSource = seanslar;
                cmbSeans.DisplayMember = "SeansAdi";
                cmbSeans.ValueMember = "SeansID";
            }
        }

        private void KoltuklariYukle()
        {
            using (var db = new AppDbContext())
            {
                var koltuklar = db.Koltuklar.ToList();
                cmbKoltuk.DataSource = koltuklar;
                cmbKoltuk.DisplayMember = "KoltukNo";
                cmbKoltuk.ValueMember = "KoltukID";
            }
        }

        private void FiyatGuncelle()
        {
            decimal birimFiyat = chkKampanya.Checked ? 50 : 100;
            decimal toplam = birimFiyat * nudAdet.Value;
            lblFiyat.Text = $"{toplam:0.00} ₺";
        }

        private void BtnSatinAl_Click(object sender, EventArgs e)
        {
            if (cmbSeans.SelectedValue == null || cmbKoltuk.SelectedValue == null)
            {
                MessageBox.Show("Lütfen seans ve koltuk seçiniz.", "Uyarı");
                return;
            }

            int seansID = Convert.ToInt32(cmbSeans.SelectedValue);
            int koltukID = Convert.ToInt32(cmbKoltuk.SelectedValue);
            int adet = (int)nudAdet.Value;
            bool kampanya = chkKampanya.Checked;

            decimal birimFiyat = kampanya ? 50 : 100;

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == kullaniciEposta.ToLower());
                if (kullanici == null)
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata");
                    return;
                }
                }

                for (int i = 0; i < adet; i++)
                {
                    var bilet = new Bilet()
                    {
                        KullaniciID = kullanici.KullaniciID,
                        SeansID = seansID,
                        KoltukID = koltukID,
                        Fiyat = birimFiyat,
                        KampanyaUygulandiMi = kampanya,
                        SatinAlmaTarihi = DateTime.Now
                    };
                    db.Biletler.Add(bilet);
                }
            }
        }
    }
}
