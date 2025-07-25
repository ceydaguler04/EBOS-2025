using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;

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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            ArayuzOlustur();
            SeanslariYukle();
            KoltuklariYukle();
            FiyatGuncelle();
        }

        private void ArayuzOlustur()
        {
            Label lblSeans = new Label() { Text = "Seans Seç:", Location = new Point(30, 30), AutoSize = true };
            cmbSeans = new ComboBox() { Location = new Point(150, 25), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblKoltuk = new Label() { Text = "Koltuk Seç:", Location = new Point(30, 80), AutoSize = true };
            cmbKoltuk = new ComboBox() { Location = new Point(150, 75), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblAdet = new Label() { Text = "Bilet Adedi:", Location = new Point(30, 130), AutoSize = true };
            nudAdet = new NumericUpDown() { Location = new Point(150, 125), Width = 60, Minimum = 1, Maximum = 10, Value = 1 };
            nudAdet.ValueChanged += (s, e) => FiyatGuncelle();

            chkKampanya = new CheckBox() { Text = "Kampanya Uygulansın", Location = new Point(150, 165), AutoSize = true };
            chkKampanya.CheckedChanged += (s, e) => FiyatGuncelle();

            Label lblFiyatLabel = new Label() { Text = "Toplam Fiyat:", Location = new Point(30, 210), AutoSize = true };
            lblFiyat = new Label() { Text = "0 ₺", Location = new Point(150, 210), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            Guna2Button btnSatinAl = new Guna2Button()
            {
                Text = "Satın Al",
                Location = new Point(150, 270),
                Size = new Size(150, 40),
                FillColor = Color.Teal,
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnSatinAl.Click += BtnSatinAl_Click;

            this.Controls.AddRange(new Control[] {
                lblSeans, cmbSeans,
                lblKoltuk, cmbKoltuk,
                lblAdet, nudAdet,
                chkKampanya, lblFiyatLabel, lblFiyat,
                btnSatinAl
            });
        }

        private void SeanslariYukle()
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

                bool koltukZatenAlinmis = db.Biletler.Any(b => b.SeansID == seansID && b.KoltukID == koltukID);
                if (koltukZatenAlinmis)
                {
                    MessageBox.Show("Seçtiğiniz koltuk bu seans için zaten alınmış.", "Uyarı");
                    return;
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

                db.SaveChanges();
            }

            MessageBox.Show("Bilet(ler) başarıyla satın alındı!", "Onay", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
