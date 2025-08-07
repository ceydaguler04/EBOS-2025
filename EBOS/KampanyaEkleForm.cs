using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public partial class KampanyaEkleForm : Form
    {
        private Guna2TextBox txtAd, txtKod, txtAciklama, txtMinTutar, txtIndirim;
        private Guna2DateTimePicker dtpBaslangic, dtpBitis;
        private Guna2CheckBox chkAktif;
        private Guna2Button btnKaydet;

        public KampanyaEkleForm()
        {
            this.Text = "Yeni Kampanya Ekle";
            this.Size = new Size(400, 650);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            int y = 20;

            Label lblAd = new Label() { Text = "Kampanya Adı", Location = new Point(30, y), AutoSize = true, Font = labelFont };
            txtAd = new Guna2TextBox() { Location = new Point(30, y += 25), Width = 320 };

            Label lblKod = new Label() { Text = "Kampanya Kodu", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            txtKod = new Guna2TextBox() { Location = new Point(30, y += 25), Width = 320 };

            Label lblAciklama = new Label() { Text = "Açıklama", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            txtAciklama = new Guna2TextBox() { Location = new Point(30, y += 25), Width = 320 };

            Label lblMinTutar = new Label() { Text = "Minimum Tutar (₺)", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            txtMinTutar = new Guna2TextBox() { Location = new Point(30, y += 25), Width = 320 };

            Label lblIndirim = new Label() { Text = "İndirim Yüzdesi (%)", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            txtIndirim = new Guna2TextBox() { Location = new Point(30, y += 25), Width = 320 };

            Label lblBaslangic = new Label() { Text = "Başlangıç Tarihi", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            dtpBaslangic = new Guna2DateTimePicker() { Location = new Point(30, y += 25), Width = 320 };

            Label lblBitis = new Label() { Text = "Bitiş Tarihi", Location = new Point(30, y += 50), AutoSize = true, Font = labelFont };
            dtpBitis = new Guna2DateTimePicker() { Location = new Point(30, y += 25), Width = 320 };

            btnKaydet = new Guna2Button()
            {
                Text = "Kaydet",
                Location = new Point(30, y += 50),
                Width = 320,
                FillColor = Color.FromArgb(232, 62, 140),
                ForeColor = Color.White,
                BorderRadius = 10
            };
            btnKaydet.Click += BtnKaydet_Click;

            Controls.AddRange(new Control[] {
                lblAd, txtAd,
                lblKod, txtKod,
                lblAciklama, txtAciklama,
                lblMinTutar, txtMinTutar,
                lblIndirim, txtIndirim,
                lblBaslangic, dtpBaslangic,
                lblBitis, dtpBitis,
                btnKaydet
            });
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    Kampanya yeni = new Kampanya
                    {
                        KampanyaAdi = txtAd.Text,
                        KampanyaKodu = txtKod.Text,
                        Aciklama = txtAciklama.Text,
                        MinTutar = decimal.Parse(txtMinTutar.Text),
                        IndirimYuzdesi = int.Parse(txtIndirim.Text),
                        BaslangicTarihi = dtpBaslangic.Value,
                        BitisTarihi = dtpBitis.Value
                    };

                    db.Kampanyalar.Add(yeni);
                    db.SaveChanges();
                }

                MessageBox.Show("Kampanya başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void KampanyaEkleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
