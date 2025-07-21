using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public partial class YoneticiPaneli : Form
    {
        private Guna2Panel topPanel;
        private Label lblBaslik;
        private Label lblKullaniciAd;
        private Guna2Panel leftMenu;

        private IconButton btnDashboard;
        private IconButton btnEtkinlikler;
        //private IconButton btnSeanslar;
        private IconButton btnKampanyalar;
        private IconButton btnKullanicilar;
        private IconButton btnAyarlar;

        private IconButton aktifButon = null;
        private Panel mainContentPanel;
        private ContextMenuStrip contextMenu;

        public YoneticiPaneli()
        {
            InitializeComponent();
            this.Text = "Y�netici Paneli";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

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
                Text = "Y�netici Paneli",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(20, 15),
                AutoSize = true
            };
            topPanel.Controls.Add(lblBaslik);

            lblKullaniciAd = new Label()
            {
                Text = "Ceyda G�ler",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            lblKullaniciAd.Location = new Point(this.Width - 160, 20);
            lblKullaniciAd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            topPanel.Controls.Add(lblKullaniciAd);

            contextMenu = new ContextMenuStrip
            {
                BackColor = Color.FromArgb(90, 115, 70),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ShowImageMargin = false,
                Renderer = new CustomColorRenderer()
            };

            ToolStripMenuItem cikisItem = new ToolStripMenuItem("��k�� Yap");
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

            btnDashboard = MenuIconButton("Dashboard", IconChar.Home, 40);
            btnDashboard.Click += (s, e) =>
            {
                SetActiveButton(btnDashboard);
                mainContentPanel.Controls.Clear();
                DashboardKontrol dashboard = new DashboardKontrol();
                dashboard.Dock = DockStyle.Fill;
                mainContentPanel.Controls.Add(dashboard);
            };

            btnEtkinlikler = MenuIconButton("Etkinlikler", IconChar.CalendarAlt, 90);
            btnEtkinlikler.Click += (s, e) =>
            {
                SetActiveButton(btnEtkinlikler);
                mainContentPanel.Controls.Clear();
                EtkinliklerKontrol etkinliklerKontrol = new EtkinliklerKontrol();
                etkinliklerKontrol.Dock = DockStyle.Fill;
                mainContentPanel.Controls.Add(etkinliklerKontrol);
            };

            //btnSeanslar = MenuIconButton("Seanslar", IconChar.Clock, 140);
            //btnSeanslar.Click += (s, e) =>
            //{
            //    SetActiveButton(btnSeanslar);
            //    mainContentPanel.Controls.Clear();
            //    SeanslarimKontrol seanslarKontrol = new SeanslarimKontrol();
            //    mainContentPanel.Controls.Add(seanslarKontrol);
            //};

            btnKampanyalar = MenuIconButton("Kampanyalar", IconChar.Tags, 140);
            btnKampanyalar.Click += (s, e) =>
            {
                SetActiveButton(btnKampanyalar);
                mainContentPanel.Controls.Clear();
                KampanyalarKontrol kampanyaSayfasi = new KampanyalarKontrol();
                kampanyaSayfasi.Dock = DockStyle.Fill;
                mainContentPanel.Controls.Add(kampanyaSayfasi);
            };

            btnKullanicilar = MenuIconButton("Kullan�c�lar", IconChar.Users, 190);
            btnKullanicilar.Click += (s, e) =>
            {
                SetActiveButton(btnKullanicilar);
                mainContentPanel.Controls.Clear();
                KullanicilarKontrol kontrol = new KullanicilarKontrol();
                mainContentPanel.Controls.Add(kontrol);
            };

            btnAyarlar = MenuIconButton("Ayarlar", IconChar.Cogs, 240);
            btnAyarlar.Click += (s, e) =>
            {
                SetActiveButton(btnAyarlar);
                mainContentPanel.Controls.Clear();
                AyarlarKontroll kontrol = new AyarlarKontroll("ceyda@example.com");
                mainContentPanel.Controls.Add(kontrol);
            };

            leftMenu.Controls.AddRange(new Control[] {
                btnDashboard, btnEtkinlikler, /*btnSeanslar,*/
                btnKampanyalar, btnKullanicilar, btnAyarlar
            });

            mainContentPanel = new Panel()
            {
                Location = new Point(210, 80),
                Size = new Size(850, 550),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(mainContentPanel);

            this.Load += YoneticiPaneli_Load;
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

        private void YoneticiPaneli_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            SetActiveButton(btnDashboard);
            mainContentPanel.Controls.Clear();
            DashboardKontrol dashboard = new DashboardKontrol();
            dashboard.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(dashboard);
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
                }
            }

            if (aktifButon != null)
            {
                aktifButon.BackColor = TemaYonetici.SeciliButonRengi();
            }

            TemaYonetici.ContextMenuRenkleriUygula(contextMenu);

            leftMenu.Refresh();
        }
    }
}

//using FontAwesome.Sharp;
//using Guna.UI2.WinForms;

//namespace EBOS
//{
//    public partial class YoneticiPaneli : Form
//    {
//        private Guna2Panel topPanel;
//        private Label lblBaslik;
//        private Label lblKullaniciAd;
//        private Guna2Panel leftMenu;

//        private IconButton btnDashboard;
//        private IconButton btnEtkinlikler;
//        private IconButton btnSeanslar;
//        private IconButton btnKampanyalar;
//        private IconButton btnKullanicilar;
//        private IconButton btnAyarlar;

//        private IconButton aktifButon = null;
//        private Panel mainContentPanel;

//        public YoneticiPaneli()
//        {
//            InitializeComponent();
//            this.Text = "Y�netici Paneli";
//            this.Size = new Size(1100, 700);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.FormBorderStyle = FormBorderStyle.FixedDialog;
//            this.MaximizeBox = false;
//            this.BackColor = Color.WhiteSmoke;

//            topPanel = new Guna2Panel()
//            {
//                Size = new Size(this.Width, 60),
//                Location = new Point(0, 0),
//                FillColor = Color.FromArgb(90, 115, 47),
//                Dock = DockStyle.Top
//            };
//            this.Controls.Add(topPanel);

//            lblBaslik = new Label()
//            {
//                Text = "Y�netici Paneli",
//                Font = new Font("Segoe UI", 16, FontStyle.Bold),
//                ForeColor = Color.White,
//                BackColor = Color.Transparent,
//                Location = new Point(20, 15),
//                AutoSize = true
//            };
//            topPanel.Controls.Add(lblBaslik);

//            lblKullaniciAd = new Label()
//            {
//                Text = "Ceyda G�ler",
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                ForeColor = Color.White,
//                BackColor = Color.Transparent,
//                AutoSize = true,
//                Cursor = Cursors.Hand
//            };
//            lblKullaniciAd.Location = new Point(this.Width - 160, 20);
//            lblKullaniciAd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
//            topPanel.Controls.Add(lblKullaniciAd);

//            ContextMenuStrip contextMenu = new ContextMenuStrip
//            {
//                BackColor = Color.FromArgb(90, 115, 70),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10, FontStyle.Regular),
//                ShowImageMargin = false,
//                Renderer = new CustomColorRenderer()
//            };

//            ToolStripMenuItem cikisItem = new ToolStripMenuItem("��k�� Yap");
//            cikisItem.Click += (s, e) =>
//            {
//                GirisForm giris = new GirisForm();
//                giris.Show();
//                this.Close();
//            };
//            contextMenu.Items.Add(cikisItem);

//            lblKullaniciAd.MouseDown += (s, e) =>
//            {
//                if (e.Button == MouseButtons.Left)
//                {
//                    contextMenu.Show(lblKullaniciAd, new Point(0, lblKullaniciAd.Height));
//                }
//            };

//            leftMenu = new Guna2Panel()
//            {
//                Size = new Size(200, this.Height - 60),
//                Location = new Point(0, 60),
//                FillColor = Color.FromArgb(90, 115, 47)
//            };
//            this.Controls.Add(leftMenu);

//            btnDashboard = MenuIconButton("Dashboard", IconChar.Home, 40);
//            btnDashboard.Click += (s, e) =>
//            {
//                SetActiveButton(btnDashboard);
//                mainContentPanel.Controls.Clear();
//                DashboardKontrol dashboard = new DashboardKontrol();
//                dashboard.Dock = DockStyle.Fill;
//                mainContentPanel.Controls.Add(dashboard);
//            };

//            btnEtkinlikler = MenuIconButton("Etkinlikler", IconChar.CalendarAlt, 90);
//            btnEtkinlikler.Click += (s, e) =>
//            {
//                SetActiveButton(btnEtkinlikler);
//                mainContentPanel.Controls.Clear();
//                EtkinliklerKontrol etkinliklerKontrol = new EtkinliklerKontrol();
//                etkinliklerKontrol.Dock = DockStyle.Fill;
//                mainContentPanel.Controls.Add(etkinliklerKontrol);
//            };

//            btnKampanyalar = MenuIconButton("Kampanyalar", IconChar.Tags, 140);
//            btnKampanyalar.Click += (s, e) =>
//            {
//                SetActiveButton(btnKampanyalar);
//                mainContentPanel.Controls.Clear();
//                KampanyalarKontrol kampanyaSayfasi = new KampanyalarKontrol();
//                kampanyaSayfasi.Dock = DockStyle.Fill;
//                mainContentPanel.Controls.Add(kampanyaSayfasi);
//            };

//            btnKullanicilar = MenuIconButton("Kullan�c�lar", IconChar.Users, 190);
//            btnKullanicilar.Click += (s, e) =>
//            {
//                SetActiveButton(btnKullanicilar);
//                mainContentPanel.Controls.Clear();
//                KullanicilarKontrol kontrol = new KullanicilarKontrol();
//                mainContentPanel.Controls.Add(kontrol);
//            };

//            btnAyarlar = MenuIconButton("Ayarlar", IconChar.Cogs, 240);
//            btnAyarlar.Click += (s, e) =>
//            {
//                SetActiveButton(btnAyarlar);
//                mainContentPanel.Controls.Clear();
//                AyarlarKontroll kontrol = new AyarlarKontroll("ceyda@example.com");
//                mainContentPanel.Controls.Add(kontrol);
//            };

//            leftMenu.Controls.AddRange(new Control[] {
//                btnDashboard, btnEtkinlikler, btnSeanslar,
//                btnKampanyalar, btnKullanicilar, btnAyarlar
//            });

//            SetActiveButton(btnDashboard);

//            mainContentPanel = new Panel()
//            {
//                Location = new Point(210, 80),
//                Size = new Size(850, 550),
//                BackColor = Color.WhiteSmoke
//            };
//            this.Controls.Add(mainContentPanel);

//            this.Load += YoneticiPaneli_Load;
//        }

//        private IconButton MenuIconButton(string text, IconChar icon, int top)
//        {
//            var btn = new IconButton()
//            {
//                Text = text,
//                IconChar = icon,
//                IconColor = Color.White,
//                TextAlign = ContentAlignment.MiddleLeft,
//                TextImageRelation = TextImageRelation.ImageBeforeText,
//                Padding = new Padding(10, 0, 20, 0),
//                Size = new Size(220, 40),
//                Location = new Point(10, top),
//                FlatStyle = FlatStyle.Flat,
//                BackColor = Color.Transparent,
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//            };

//            btn.FlatAppearance.BorderSize = 0;
//            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 130, 45);
//            btn.Click += (s, e) => SetActiveButton(btn);
//            return btn;
//        }

//        private void SetActiveButton(IconButton buton)
//        {
//            if (aktifButon != null)
//            {
//                aktifButon.BackColor = Color.Transparent;
//            }

//            buton.BackColor = Color.FromArgb(90, 130, 45);
//            aktifButon = buton;
//        }

//        private void YoneticiPaneli_Load(object sender, EventArgs e)
//        {
//            TemaYonetici.Uygula(this);
//            SetActiveButton(btnDashboard);
//            mainContentPanel.Controls.Clear();
//            DashboardKontrol dashboard = new DashboardKontrol();
//            dashboard.Dock = DockStyle.Fill;
//            mainContentPanel.Controls.Add(dashboard);
//        }
//    }
//}