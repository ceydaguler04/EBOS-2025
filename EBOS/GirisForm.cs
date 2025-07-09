using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class GirisForm : Form
    {
        public GirisForm()
        {
            InitializeComponent();
            EkraniOlustur();
        }

        private void EkraniOlustur()
        {
            this.Text = "Giriş Yap";
            this.ClientSize = new System.Drawing.Size(400, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // E-Posta Label
            Label lblEposta = new Label()
            {
                Text = "E-Posta:",
                Location = new System.Drawing.Point(50, 80),
                AutoSize = true
            };
            this.Controls.Add(lblEposta);

            // E-Posta TextBox
            Guna2TextBox txtEposta = new Guna2TextBox()
            {
                Name = "txtEposta",
                PlaceholderText = "E-posta adresinizi girin",
                Location = new System.Drawing.Point(50, 110),
                Size = new System.Drawing.Size(300, 40)
            };
            this.Controls.Add(txtEposta);

            // Şifre Label
            Label lblSifre = new Label()
            {
                Text = "Şifre:",
                Location = new System.Drawing.Point(50, 170),
                AutoSize = true
            };
            this.Controls.Add(lblSifre);

            // Şifre TextBox
            Guna2TextBox txtSifre = new Guna2TextBox()
            {
                Name = "txtSifre",
                PlaceholderText = "Şifrenizi girin",
                PasswordChar = '*',
                Location = new System.Drawing.Point(50, 200),
                Size = new System.Drawing.Size(300, 40)
            };
            this.Controls.Add(txtSifre);

            // Rol Label
            Label lblRol = new Label()
            {
                Text = "Rol:",
                Location = new System.Drawing.Point(50, 260),
                AutoSize = true
            };
            this.Controls.Add(lblRol);

            // Rol ComboBox
            Guna2ComboBox cmbRol = new Guna2ComboBox()
            {
                Name = "cmbRol",
                Location = new System.Drawing.Point(50, 290),
                Size = new System.Drawing.Size(300, 40),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRol.Items.AddRange(new string[] { "Kullanıcı", "Yönetici" });
            this.Controls.Add(cmbRol);

            // Giriş Butonu
            Guna2Button btnGiris = new Guna2Button()
            {
                Text = "Giriş Yap",
                Location = new System.Drawing.Point(50, 350),
                Size = new System.Drawing.Size(300, 45)
            };
            btnGiris.Click += (s, e) =>
            {
                string rol = cmbRol.SelectedItem?.ToString();

                if (rol == "Yönetici")
                {
                    //  YoneticiPaneli panel = new YoneticiPaneli();
                    //  panel.Show();
                }
                else if (rol == "Kullanıcı")
                {
                    AnaSayfaForm ana = new AnaSayfaForm(); // ✅ Senin kullanıcı sayfan
                    ana.Show();
                }
                else
                {
                    MessageBox.Show("Lütfen bir rol seçin.", "Uyarı");
                    return;
                }

                this.Hide();
            };
            this.Controls.Add(btnGiris);

            // Kayıt Ol Linki
            LinkLabel linkKayit = new LinkLabel()
            {
                Text = "Hesabınız yok mu? Kayıt Ol",
                Location = new System.Drawing.Point(50, 410),
                AutoSize = true
            };
            linkKayit.Click += (s, e) =>
            {
                KayitForm kayit = new KayitForm();
                kayit.Show();
                this.Hide();
            };
            this.Controls.Add(linkKayit);

            // Şifremi Unuttum Linki
            LinkLabel linkSifreUnuttum = new LinkLabel()
            {
                Text = "Şifremi unuttum",
                Location = new System.Drawing.Point(50, 440),
                AutoSize = true
            };
            linkSifreUnuttum.Click += (s, e) =>
            {
                MessageBox.Show("Bu özellik henüz aktif değil. (Şifre sıfırlama ekranı yakında)", "Bilgi");
            };
            this.Controls.Add(linkSifreUnuttum);
        }

        // 🔽 Buraya eklemelisin:
        private void GirisForm_Load(object sender, EventArgs e)
        {
            // Şu anda boş kalabilir
        }
    }
}
