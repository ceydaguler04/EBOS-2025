using System;
using System.Drawing;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    public partial class KullaniciEkleForm : Form
    {
        private TextBox txtAdSoyad, txtEposta, txtSifre;
        private ComboBox cmbRol;
        private Button btnKaydet;

        public KullaniciEkleForm()
        {
            this.Text = "Yeni Kullanıcı Ekle";
            this.Size = new Size(420, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Font genelFont = new Font("Segoe UI", 11F, FontStyle.Regular);

            // Ad Soyad
            Label lblAdSoyad = new Label() { Text = "Ad Soyad:", Left = 30, Top = 25, Width = 100, Font = genelFont };
            txtAdSoyad = new TextBox() { Left = 30, Top = 50, Width = 340, Font = genelFont };

            // E-posta
            Label lblEposta = new Label() { Text = "E-posta:", Left = 30, Top = 90, Width = 100, Font = genelFont };
            txtEposta = new TextBox() { Left = 30, Top = 115, Width = 340, Font = genelFont };

            // Şifre
            Label lblSifre = new Label() { Text = "Şifre:", Left = 30, Top = 155, Width = 100, Font = genelFont };
            txtSifre = new TextBox() { Left = 30, Top = 180, Width = 340, Font = genelFont, UseSystemPasswordChar = true };

            // Rol
            Label lblRol = new Label() { Text = "Rol:", Left = 30, Top = 220, Width = 100, Font = genelFont };
            cmbRol = new ComboBox()
            {
                Left = 30,
                Top = 245,
                Width = 340,
                Font = genelFont,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White
            };
            cmbRol.Items.AddRange(new string[] { "Kullanıcı", "Yönetici", "Organizatör" });
            cmbRol.SelectedIndex = 0;

            // Kaydet Butonu
            btnKaydet = new Button()
            {
                Text = "Kaydet",
                Left = 30,
                Top = 290,
                Width = 120,
                Height = 40,
                BackColor = Color.FromArgb(90, 115, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.Click += BtnKaydet_Click;

            this.Controls.AddRange(new Control[]
            {
                lblAdSoyad, txtAdSoyad,
                lblEposta, txtEposta,
                lblSifre, txtSifre,
                lblRol, cmbRol,
                btnKaydet
            });
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            string adsoyad = txtAdSoyad.Text.Trim();
            string eposta = txtEposta.Text.Trim();
            string sifre = txtSifre.Text.Trim();
            string rol = cmbRol.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(adsoyad) || string.IsNullOrWhiteSpace(eposta) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                bool ayniMailVar = db.Kullanicilar.Any(k => k.Eposta == eposta);
                if (ayniMailVar)
                {
                    MessageBox.Show("Bu e-posta adresi zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var yeni = new Kullanici()
                {
                    AdSoyad = adsoyad,
                    Eposta = eposta,
                    Sifre = sifre,
                    Rol = rol
                };

                db.Kullanicilar.Add(yeni);
                db.SaveChanges();
            }

            MessageBox.Show("Kullanıcı başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void KullaniciEkleForm_Load(object sender, EventArgs e)
        {

        }
    }
}