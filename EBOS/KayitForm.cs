using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class KayitForm : Form
    {
        public KayitForm()
        {
            InitializeComponent();
            ArayuzOlustur();
        }
        private void KayitForm_Load(object sender, EventArgs e)
        {
            // Şimdilik boş kalabilir, önemli değil
        }

        private void ArayuzOlustur()
        {
            this.Text = "Kayıt Ol";
            this.ClientSize = new System.Drawing.Size(400, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Ad Soyad
            Label lblAd = new Label() { Text = "Ad Soyad:", Location = new System.Drawing.Point(50, 40), AutoSize = true };
            Guna2TextBox txtAd = new Guna2TextBox()
            {
                Name = "txtAd",
                PlaceholderText = "Adınızı ve Soyadınızı girin",
                Location = new System.Drawing.Point(50, 70),
                Size = new System.Drawing.Size(300, 40)
            };

            // E-Posta
            Label lblEposta = new Label() { Text = "E-Posta:", Location = new System.Drawing.Point(50, 120), AutoSize = true };
            Guna2TextBox txtEposta = new Guna2TextBox()
            {
                Name = "txtEposta",
                PlaceholderText = "E-posta adresinizi girin",
                Location = new System.Drawing.Point(50, 150),
                Size = new System.Drawing.Size(300, 40)
            };

            // Şifre
            Label lblSifre = new Label() { Text = "Şifre:", Location = new System.Drawing.Point(50, 200), AutoSize = true };
            Guna2TextBox txtSifre = new Guna2TextBox()
            {
                Name = "txtSifre",
                PlaceholderText = "Şifrenizi girin",
                PasswordChar = '*',
                Location = new System.Drawing.Point(50, 230),
                Size = new System.Drawing.Size(300, 40)
            };

            // Şifre Tekrar
            Label lblTekrar = new Label() { Text = "Şifre (Tekrar):", Location = new System.Drawing.Point(50, 280), AutoSize = true };
            Guna2TextBox txtTekrar = new Guna2TextBox()
            {
                Name = "txtTekrar",
                PlaceholderText = "Şifrenizi tekrar girin",
                PasswordChar = '*',
                Location = new System.Drawing.Point(50, 310),
                Size = new System.Drawing.Size(300, 40)
            };

            // Rol
            Label lblRol = new Label() { Text = "Rol:", Location = new System.Drawing.Point(50, 360), AutoSize = true };
            Guna2ComboBox cmbRol = new Guna2ComboBox()
            {
                Name = "cmbRol",
                Location = new System.Drawing.Point(50, 390),
                Size = new System.Drawing.Size(300, 40),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRol.Items.AddRange(new string[] { "Kullanıcı", "Yönetici" });

            // Kayıt Ol Butonu
            Guna2Button btnKayit = new Guna2Button()
            {
                Text = "Kayıt Ol",
                Location = new System.Drawing.Point(50, 450),
                Size = new System.Drawing.Size(300, 45)
            };
            btnKayit.Click += (s, e) =>
            {
                if (txtSifre.Text != txtTekrar.Text)
                {
                    MessageBox.Show("Şifreler uyuşmuyor!", "Hata");
                    return;
                }

                // Gerçek veri ekleme işlemi yapılabilir
                MessageBox.Show("Kayıt başarıyla tamamlandı!", "Bilgi");

                GirisForm giris = new GirisForm();
                giris.Show();
                this.Hide();
            };

            // Girişe dön linki
            LinkLabel linkGiris = new LinkLabel()
            {
                Text = "Zaten hesabın var mı? Giriş yap",
                Location = new System.Drawing.Point(50, 510),
                AutoSize = true
            };
            linkGiris.Click += (s, e) =>
            {
                GirisForm giris = new GirisForm();
                giris.Show();
                this.Hide();
            };

            this.Controls.AddRange(new Control[]
            {
                lblAd, txtAd,
                lblEposta, txtEposta,
                lblSifre, txtSifre,
                lblTekrar, txtTekrar,
                lblRol, cmbRol,
                btnKayit, linkGiris
            });
        }
    }
}
