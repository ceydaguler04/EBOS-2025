using System;
using System.Windows.Forms;
using System.Drawing;

namespace EBOS
{
    public partial class KullaniciPaneli : Form
    {
        public KullaniciPaneli()
        {
            InitializeComponent();
            PaneliOlustur();
        }
        private void KullaniciPaneli_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde yapılacak işlemler varsa buraya yaz
        }

        private void PaneliOlustur()
        {
            // Genel Form Ayarları
            this.Text = "Kullanıcı Paneli";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.FromArgb(2, 48, 151);

            // Başlık Label
            Label lblBaslik = new Label();
            lblBaslik.Text = "Hoş Geldiniz!";
            lblBaslik.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblBaslik.ForeColor = Color.White;
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new Point(300, 50);
            this.Controls.Add(lblBaslik);

            // Örnek bir buton (Etkinlikler)
            Button btnEtkinlikler = new Button();
            btnEtkinlikler.Text = "Etkinlikler";
            btnEtkinlikler.Size = new Size(200, 40);
            btnEtkinlikler.Location = new Point(300, 150);
            this.Controls.Add(btnEtkinlikler);

            // Örnek bir buton (Biletlerim)
            Button btnBiletlerim = new Button();
            btnBiletlerim.Text = "Biletlerim";
            btnBiletlerim.Size = new Size(200, 40);
            btnBiletlerim.Location = new Point(300, 210);
            this.Controls.Add(btnBiletlerim);

            // Çıkış Butonu
            Button btnCikis = new Button();
            btnCikis.Text = "Çıkış Yap";
            btnCikis.Size = new Size(200, 40);
            btnCikis.Location = new Point(300, 270);
            btnCikis.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnCikis);

        }
    }
}
