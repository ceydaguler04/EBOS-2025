// YoneticiPaneli.cs - Yeni Arayüzlü Dashboard
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace EBOS
{
    public partial class YoneticiPaneli : Form
    {
        private Guna2Panel topPanel;
        private Label lblBaslik;
        private Guna2CirclePictureBox profileImage;
        private Guna2Button btnCikis;

        private Guna2Panel leftMenu;
        private Guna2Button btnDashboard;
        private Guna2Button btnEtkinlikler;
        private Guna2Button btnSeanslar;
        private Guna2Button btnKampanyalar;
        private Guna2Button btnKullanicilar;
        private Guna2Button btnAyarlar;

        private Label lblToplamEtkinlik;
        private Label lblToplamSeans;
        private Label lblKampanya;
        private Guna2HtmlLabel lblGrafikBaslik;
        private Guna2Panel grafikPanel;

        public YoneticiPaneli()
        {
            InitializeComponent();
            this.Text = "Yönetici Paneli";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.BackColor = Color.WhiteSmoke;

            // Üst Panel
            topPanel = new Guna2Panel()
            {
                Size = new Size(this.Width, 60),
                Location = new Point(0, 0),
                FillColor = Color.FromArgb(33, 53, 85),
                Dock = DockStyle.Top
            };
            this.Controls.Add(topPanel);

            lblBaslik = new Label()
            {
                Text = "Yönetici Paneli",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };
            topPanel.Controls.Add(lblBaslik);

            btnCikis = new Guna2Button()
            {
                Text = "Çýkýþ Yap",
                FillColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Size = new Size(100, 35),
                Location = new Point(this.Width - 130, 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 8
            };
            btnCikis.Click += (s, e) => this.Close();
            topPanel.Controls.Add(btnCikis);

            profileImage = new Guna2CirclePictureBox()
            {
                Size = new Size(35, 35),
                Location = new Point(this.Width - 170, 12),
                //Image = Properties.Resources.DefaultProfile, // sen kendi profil resmini koyabilirsin
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(profileImage);

            // Sol Menü
            leftMenu = new Guna2Panel()
            {
                Size = new Size(180, this.Height),
                Location = new Point(0, 60),
                FillColor = Color.FromArgb(245, 247, 250),
                Dock = DockStyle.Left
            };
            this.Controls.Add(leftMenu);

            btnDashboard = MenuButon("DASHBOARD", 30);
            btnEtkinlikler = MenuButon("Etkinlikler", 80);
            btnSeanslar = MenuButon("Seanslar", 130);
            btnKampanyalar = MenuButon("Kampanyalar", 180);
            btnKullanicilar = MenuButon("Kullanýcýlar", 230);
            btnAyarlar = MenuButon("Ayarlar", 280);

            leftMenu.Controls.AddRange(new Control[]
            {
                btnDashboard, btnEtkinlikler, btnSeanslar, btnKampanyalar, btnKullanicilar, btnAyarlar
            });

            // Dashboard Göstergeler
            lblToplamEtkinlik = InfoLabel("12\nToplam Etkinlik", new Point(200, 100), Color.MediumTurquoise);
            lblToplamSeans = InfoLabel("45\nToplam Seans", new Point(420, 100), Color.SteelBlue);
            lblKampanya = InfoLabel("3\nAktif Kampanya", new Point(640, 100), Color.Orange);

            this.Controls.AddRange(new Control[] { lblToplamEtkinlik, lblToplamSeans, lblKampanya });

            // Grafik baþlýk
            lblGrafikBaslik = new Guna2HtmlLabel()
            {
                Text = "Genel Ýstatistikler",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(200, 200),
                ForeColor = Color.DimGray
            };
            this.Controls.Add(lblGrafikBaslik);

            // Grafik Panel (þimdilik görsel placeholder)
            grafikPanel = new Guna2Panel()
            {
                Location = new Point(200, 240),
                Size = new Size(800, 300),
                BorderRadius = 10,
                FillColor = Color.White,
                BorderColor = Color.Gainsboro,
                BorderThickness = 1
            };
            this.Controls.Add(grafikPanel);
        }

        private Guna2Button MenuButon(string text, int top)
        {
            return new Guna2Button()
            {
                Text = text,
                Size = new Size(160, 40),
                Location = new Point(10, top),
                BorderRadius = 8,
                FillColor = Color.White,
                ForeColor = Color.FromArgb(51, 51, 51),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                HoverState = { FillColor = Color.FromArgb(225, 233, 245) }
            };
        }

        private Label InfoLabel(string text, Point konum, Color renk)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(180, 60),
                Location = konum,
                BackColor = renk,
                ForeColor = Color.White,
                Location = new Point(55, 38),
                AutoSize = true
            };
        }
        private void YoneticiPaneli_Load(object sender, EventArgs e)
        {
            // Form açýldýðýnda çalýþacak iþlemler buraya
        }

    }
}
