
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using EBOS.DataAccess;

namespace EBOS
{
    public partial class KullaniciPaneli : Form
    {
        // Alanlar
        private Guna2Panel topPanel;
        private Label lblBaslik;
        private Guna2HtmlLabel lblAdMenu;
        private Guna2Panel leftMenu;
        private IconButton aktifButon = null;
        private Panel mainContentPanel;

        private IconButton btnSinema, btnTiyatro, btnKonser, btnWorkshop, btnSeminer, btnAyar, btnBiletlerim;

        private string girisYapanEposta;
        private string girisYapanAdSoyad;

        public KullaniciPaneli(string eposta)
        {
            InitializeComponent();
            this.Text = "Kullanıcı Paneli";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            girisYapanEposta = eposta;

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower());
                girisYapanAdSoyad = kullanici != null ? kullanici.AdSoyad : "Bilinmeyen";
            }

            // Üst Panel
            topPanel = new Guna2Panel()
            {
                Size = new Size(this.Width, 60),
                Dock = DockStyle.Top
            };
            this.Controls.Add(topPanel);

            // Sol Menü
            leftMenu = new Guna2Panel()
            {
                Size = new Size(200, this.Height - topPanel.Height),
                Dock = DockStyle.Left
            };
            this.Controls.Add(leftMenu);

            // Ana İçerik Paneli
            mainContentPanel = new Panel()
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(mainContentPanel);
            this.Controls.SetChildIndex(mainContentPanel, 0);

            // Başlık
            lblBaslik = new Label()
            {
                Text = "KULLANICI PANELİ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(10, 10)
            };
            leftMenu.Controls.Add(lblBaslik);

            // Giriş Yapan Kullanıcı
            lblAdMenu = new Guna2HtmlLabel()
            {
                Text = girisYapanAdSoyad,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(lblAdMenu);
            lblAdMenu.Location = new Point(topPanel.Width - lblAdMenu.Width - 30, 20);

            // Çıkış menüsü
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Çıkış Yap", null, (s, e) =>
            {
                this.Hide();
                new GirisForm().Show();
            });
            lblAdMenu.MouseEnter += (s, e) => contextMenu.Show(Cursor.Position);

            // Menü Butonları
            btnSinema = MenuIconButton("Sinema", IconChar.Film, 70);
            btnTiyatro = MenuIconButton("Tiyatro", IconChar.TheaterMasks, 120);
            btnKonser = MenuIconButton("Konser", IconChar.Music, 170);
            btnWorkshop = MenuIconButton("Workshop", IconChar.Tools, 220);
            btnSeminer = MenuIconButton("Seminer", IconChar.ChalkboardTeacher, 270);
            btnAyar = MenuIconButton("Ayarlar", IconChar.Cogs, 320);
            btnBiletlerim = MenuIconButton("Biletlerim", IconChar.TicketAlt, 370);

            // Eventler
            btnSinema.Click += (s, e) => GosterYeniKontrol(new SinemaKontrol(girisYapanEposta), btnSinema);
            btnTiyatro.Click += (s, e) => GosterYeniKontrol(new TiyatroKontrol(girisYapanEposta), btnTiyatro);
            btnKonser.Click += (s, e) => GosterYeniKontrol(new KonserKontrol(girisYapanEposta), btnKonser);
            btnWorkshop.Click += (s, e) => GosterYeniKontrol(new WorkshopKontrol(girisYapanEposta), btnWorkshop);
            btnSeminer.Click += (s, e) => GosterYeniKontrol(new SeminerKontrol(girisYapanEposta), btnSeminer);
            btnAyar.Click += (s, e) => GosterYeniKontrol(new AyarKontrol(girisYapanEposta), btnAyar);
            btnBiletlerim.Click += (s, e) => GosterYeniKontrol(new BiletlerimKontrol(girisYapanEposta), btnBiletlerim);

            leftMenu.Controls.AddRange(new Control[] {
                btnSinema, btnTiyatro, btnKonser, btnWorkshop,
                btnSeminer, btnAyar, btnBiletlerim
            });

            // TEMA UYGULA
            TemaYonetici.Uygula(this);

            // Açılışta sinema
            GosterYeniKontrol(new SinemaKontrol(girisYapanEposta), btnSinema);
        }

        private IconButton MenuIconButton(string text, IconChar icon, int top)
        {
            var btn = new IconButton()
            {
                Text = text,
                IconChar = icon,
                IconColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(10, 0, 20, 0),
                Size = new Size(220, 40),
                Location = new Point(10, top),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = TemaYonetici.HoverRenk();
            return btn;
        }

        private void GosterYeniKontrol(UserControl kontrol, IconButton aktif)
        {
            if (aktifButon != null)
                aktifButon.BackColor = Color.Transparent;

            aktif.BackColor = TemaYonetici.SeciliButonRengi(); // Tema uyumlu seçili buton rengi
            aktifButon = aktif;

            mainContentPanel.Controls.Clear();
            kontrol.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(kontrol);
        }
        private void KullaniciPaneli_Load(object sender, EventArgs e)
        {
            // Tema veya başlangıç ayarları yapılacaksa buraya yazabilirsin
        }
        public void SolMenuTemayiGuncelle()
        {
            foreach (Control control in leftMenu.Controls)
            {
                if (control is IconButton btn && btn != aktifButon)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.MouseOverBackColor = TemaYonetici.HoverRenk();
                }
                else if (control is IconButton seciliBtn && seciliBtn == aktifButon)
                {
                    seciliBtn.BackColor = TemaYonetici.SeciliButonRengi();
                }
            }

            // Panel renkleri de değişsin
            leftMenu.FillColor = TemaYonetici.TemaButonRenk();
            topPanel.FillColor = TemaYonetici.TemaButonRenk();
        }
    }
}
   
