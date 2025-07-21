using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using EBOS;

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
        private ContextMenuStrip contextMenu;

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
            btnSeanslar.Click += (s, e) =>
            {
                SetActiveButton(btnSeanslar);
                mainContentPanel.Controls.Clear();
                SeanslarimKontrol seanslarKontrol = new SeanslarimKontrol();
                mainContentPanel.Controls.Add(seanslarKontrol);
            };


            btnAyarlar = MenuIconButton("Ayarlar", IconChar.Cogs, 190);
            btnAyarlar.Click += (s, e) =>
            {
                SetActiveButton(btnAyarlar);
                mainContentPanel.Controls.Clear();
                AyarlarKontroll ayarlar = new AyarlarKontroll("ceyda@example.com");
                mainContentPanel.Controls.Add(ayarlar);
            };

            leftMenu.Controls.AddRange(new Control[] {
                btnDashboard, btnEtkinlikler, btnSeanslar, btnAyarlar
            });

            mainContentPanel = new Panel()
            {
                Location = new Point(210, 80),
                Size = new Size(850, 550),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(mainContentPanel);

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
            btn.FlatAppearance.MouseOverBackColor = TemaYonetici.HoverRenk();
            btn.Click += (s, e) => SetActiveButton(btn);
            return btn;
        }

        private void SetActiveButton(IconButton yeniButon)
        {
            if (aktifButon != null)
            {
                // Önceki aktif butonun rengini sıfırla
                aktifButon.BackColor = Color.Transparent;
                aktifButon.ForeColor = Color.White;
                aktifButon.IconColor = Color.White;
            }

            aktifButon = yeniButon;

            // TemaYonetici'den renk al
            Color seciliRenk = TemaYonetici.SeciliButonRengi();

            aktifButon.BackColor = seciliRenk;
            aktifButon.ForeColor = Color.White;
            aktifButon.IconColor = Color.White;
        }


        public void ApplyTheme()
        {
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