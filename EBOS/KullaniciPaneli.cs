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
        private Guna2Panel topPanel;
        private Label lblBaslik;
        private Label lblKullaniciAd;
        private Guna2Panel leftMenu;
        private IconButton aktifButon = null;
        private Panel mainContentPanel;

        private IconButton btnSinema, btnTiyatro, btnKonser, btnWorkshop, btnSeminer, btnAyar, btnBiletlerim;
        private ContextMenuStrip contextMenu;

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
            this.BackColor = Color.WhiteSmoke;

            girisYapanEposta = eposta;

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta.ToLower() == eposta.ToLower());
                girisYapanAdSoyad = kullanici != null ? kullanici.AdSoyad : "Bilinmeyen";
            }

            topPanel = new Guna2Panel()
            {
                Size = new Size(this.Width, 60),
                Location = new Point(0, 0),
                FillColor = Color.FromArgb(90, 115, 47),
                Dock = DockStyle.Top
            };
            this.Controls.Add(topPanel);

            lblBaslik = new Label()
            {
                Text = "Kullanıcı Paneli",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(20, 15),
                AutoSize = true
            };
            topPanel.Controls.Add(lblBaslik);

            lblKullaniciAd = new Label()
            {
                Text = girisYapanAdSoyad,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblKullaniciAd.Location = new Point(this.Width - 160, 20);
            topPanel.Controls.Add(lblKullaniciAd);

            contextMenu = new ContextMenuStrip
            {
                BackColor = Color.FromArgb(90, 115, 70),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ShowImageMargin = false,
                Renderer = new CustomColorRenderer()
            };

            ToolStripMenuItem cikisItem = new ToolStripMenuItem("Çıkış Yap");
            cikisItem.Click += (s, e) =>
            {
                GirisForm giris = new GirisForm();
                giris.Show();
                this.Close();
            };
            contextMenu.Items.Add(cikisItem);
            TemaYonetici.ContextMenuRenkleriUygula(contextMenu);

            lblKullaniciAd.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    contextMenu.Show(lblKullaniciAd, new Point(0, lblKullaniciAd.Height));
                }
            };

            leftMenu = new Guna2Panel()
            {
                Size = new Size(200, this.Height - 60),
                Location = new Point(0, 60),
                FillColor = Color.FromArgb(90, 115, 47)
            };
            this.Controls.Add(leftMenu);

            btnSinema = MenuIconButton("Sinema", IconChar.Film, 40);
            btnTiyatro = MenuIconButton("Tiyatro", IconChar.TheaterMasks, 90);
            btnKonser = MenuIconButton("Konser", IconChar.Music, 140);
            btnWorkshop = MenuIconButton("Workshop", IconChar.Tools, 190);
            btnSeminer = MenuIconButton("Seminer", IconChar.ChalkboardTeacher, 240);
            btnBiletlerim = MenuIconButton("Biletlerim", IconChar.TicketAlt, 290);
            btnAyar = MenuIconButton("Ayarlar", IconChar.Cogs, 340);

            btnSinema.Click += (s, e) => GosterYeniKontrol(new SinemaKontrol(girisYapanEposta), btnSinema);
            btnTiyatro.Click += (s, e) => GosterYeniKontrol(new TiyatroKontrol(girisYapanEposta), btnTiyatro);
            btnKonser.Click += (s, e) => GosterYeniKontrol(new KonserKontrol(girisYapanEposta), btnKonser);
            btnWorkshop.Click += (s, e) => GosterYeniKontrol(new WorkshopKontrol(girisYapanEposta), btnWorkshop);
            btnSeminer.Click += (s, e) => GosterYeniKontrol(new SeminerKontrol(girisYapanEposta), btnSeminer);
            btnBiletlerim.Click += (s, e) => GosterYeniKontrol(new BiletlerimKontrol(girisYapanEposta), btnBiletlerim);
            btnAyar.Click += (s, e) => GosterYeniKontrol(new AyarlarKontroll(girisYapanEposta), btnAyar);

            leftMenu.Controls.AddRange(new Control[] {
                btnSinema, btnTiyatro, btnKonser, btnWorkshop,
                btnSeminer, btnBiletlerim, btnAyar
            });

            mainContentPanel = new Panel()
            {
                Location = new Point(210, 80),
                Size = new Size(850, 550),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(mainContentPanel);

            this.Load += KullaniciPaneli_Load;
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = TemaYonetici.HoverRenk();
            btn.Click += (s, e) => SetActiveButton(btn);
            return btn;
        }

        private void SetActiveButton(IconButton buton)
        {
            if (aktifButon != null)
            {
                aktifButon.BackColor = Color.Transparent;
                aktifButon.ForeColor = Color.White;
                aktifButon.IconColor = Color.White;
            }

            buton.BackColor = TemaYonetici.SeciliButonRengi();
            buton.ForeColor = Color.White;
            buton.IconColor = Color.White;
            aktifButon = buton;
        }

        private void GosterYeniKontrol(UserControl kontrol, IconButton aktif)
        {
            SetActiveButton(aktif);
            mainContentPanel.Controls.Clear();
            kontrol.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(kontrol);
        }

        private void KullaniciPaneli_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            SetActiveButton(btnSinema);
            btnSinema.PerformClick();
        }

        public void ApplyTheme()
        {
            TemaYonetici.Uygula(this);

            if (TemaYonetici.AktifTema == "Yesil")
            {
                this.BackColor = Color.FromArgb(255, 255, 255);
                leftMenu.FillColor = Color.FromArgb(90, 115, 47);
                topPanel.FillColor = Color.FromArgb(90, 115, 47);
            }
            else if (TemaYonetici.AktifTema == "Lacivert")
            {
                this.BackColor = Color.FromArgb(255, 255, 255);
                leftMenu.FillColor = Color.FromArgb(40, 55, 120);
                topPanel.FillColor = Color.FromArgb(40, 55, 120);
            }
            else if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
                leftMenu.FillColor = Color.FromArgb(50, 50, 50);
                topPanel.FillColor = Color.FromArgb(50, 50, 50);
            }

            foreach (Control control in leftMenu.Controls)
            {
                if (control is IconButton btn)
                {
                    btn.FlatAppearance.MouseOverBackColor = TemaYonetici.HoverRenk();
                    btn.Refresh();
                }
            }

            if (aktifButon != null)
            {
                aktifButon.BackColor = TemaYonetici.SeciliButonRengi();
                
            }

            TemaYonetici.ContextMenuRenkleriUygula(contextMenu);
            leftMenu.Invalidate(true);
        }
    }
}