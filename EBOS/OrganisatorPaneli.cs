using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace EBOS
{
    public partial class OrganisatorPaneli : Form
    {
        private Guna2Panel topPanel;
        private Label lblBaslik;
        private Label lblKullaniciAd;
        private Guna2Panel leftMenu;
        private IconButton btnDashboard;
        private IconButton btnEtkinlikler;
        private IconButton btnSeanslar;
        private IconButton btnBiletler;
        private IconButton btnAyarlar;
        private IconButton aktifButon = null;
        private Panel mainContentPanel;

        public static string AktifTema = "Yesil";

        public OrganisatorPaneli(string kullaniciAdi = "Organizatör")
        {
            InitializeComponent();
            this.Text = "Organizatör Paneli";
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
                Text = "Organizatör Paneli",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(20, 15),
                AutoSize = true
            };
            topPanel.Controls.Add(lblBaslik);

            lblKullaniciAd = new Label()
            {
                Text = kullaniciAdi,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            lblKullaniciAd.Location = new Point(this.Width - 160, 20);
            lblKullaniciAd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            topPanel.Controls.Add(lblKullaniciAd);

            ContextMenuStrip contextMenu = new ContextMenuStrip
            {
                BackColor = Color.FromArgb(90, 115, 70),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ShowImageMargin = false,
                Renderer = new CustomColorRenderer()
            };

            ToolStripMenuItem cikisItem = new ToolStripMenuItem("Çıkış Yap");
            cikisItem.Click += (s, e) => this.Close();
            contextMenu.Items.Add(cikisItem);

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
            btnEtkinlikler = MenuIconButton("Etkinliklerim", IconChar.CalendarAlt, 90);
            btnEtkinlikler.Click += (s, e) =>
            {
                SetActiveButton(btnEtkinlikler);
                mainContentPanel.Controls.Clear();
                EtkinliklerKontrol etkinliklerKontrol = new EtkinliklerKontrol();
                etkinliklerKontrol.Dock = DockStyle.Fill;
                mainContentPanel.Controls.Add(etkinliklerKontrol);
            };
            btnSeanslar = MenuIconButton("Seanslarım", IconChar.Clock, 140);
            btnBiletler = MenuIconButton("Biletlerim", IconChar.TicketAlt, 190);
            btnAyarlar = MenuIconButton("Ayarlar", IconChar.Cogs, 240);
            btnAyarlar.Click += (s, e) =>
            {
                SetActiveButton(btnAyarlar);
                mainContentPanel.Controls.Clear();
                var ayarlar = new AyarlarKontroll(("ceyda@example.com"));
                mainContentPanel.Controls.Add(ayarlar);
            };


            leftMenu.Controls.AddRange(new Control[]
            {
                btnDashboard, btnEtkinlikler, btnSeanslar,
                btnBiletler, btnAyarlar
            });

            mainContentPanel = new Panel()
            {
                Location = new Point(210, 80),
                Size = new Size(850, 550),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(mainContentPanel);

            //btnDashboard.Click += (s, e) =>
            //{
            //    SetActiveButton(btnDashboard);
            //    mainContentPanel.Controls.Clear();
            //    Label lbl = new Label();
            //    lbl.Text = "Dashboard (Organizatör)";
            //    lbl.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            //    lbl.Dock = DockStyle.Fill;
            //    lbl.TextAlign = ContentAlignment.MiddleCenter;
            //    mainContentPanel.Controls.Add(lbl);
            //};

            this.Load += OrganisatorPaneli_Load;
        }

        private void OrganisatorPaneli_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            SetActiveButton(btnDashboard);
            btnDashboard.PerformClick();
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 130, 45);
            btn.Click += (s, e) => SetActiveButton(btn);
            return btn;
        }

        private void SetActiveButton(IconButton buton)
        {
            if (aktifButon != null)
            {
                aktifButon.BackColor = Color.Transparent;
            }

            buton.BackColor = Color.FromArgb(90, 130, 45);
            aktifButon = buton;
        }

        public void ApplyTheme()
        {
            if (AktifTema == "Yesil")
            {
                this.BackColor = Color.FromArgb(255, 255, 255);
                leftMenu.FillColor = Color.FromArgb(90, 115, 47);
                topPanel.FillColor = Color.FromArgb(90, 115, 47);
            }
            else if (AktifTema == "Lacivert")
            {
                this.BackColor = Color.FromArgb(255, 255, 255);
                leftMenu.FillColor = Color.FromArgb(40, 55, 120);
                topPanel.FillColor = Color.FromArgb(40, 55, 120);
            }
            else if (AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
                leftMenu.FillColor = Color.FromArgb(50, 50, 50);
                topPanel.FillColor = Color.FromArgb(50, 50, 50);
            }
        }
    }
}
