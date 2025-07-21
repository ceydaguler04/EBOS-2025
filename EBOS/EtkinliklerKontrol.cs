using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class EtkinliklerKontrol : UserControl
    {
        private int? aktifKullaniciId;
        private int? rol;

        private Label lblBaslik;
        private Guna2TextBox txtArama;
        private Guna2Button btnYeniEkle;
        private FlowLayoutPanel flpKartlar;

        public EtkinliklerKontrol(int? kullaniciId = null, int? rol = null)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.aktifKullaniciId = kullaniciId;
            this.rol = rol;
            ArayuzOlustur();
            EtkinlikKartlariniOlustur();
        }

        private void ArayuzOlustur()
        {
            this.BackColor = TemaYonetici.AktifTema == "Koyu" ? Color.FromArgb(120, 120, 120) : Color.White;

            lblBaslik = new Label()
            {
                Text = "ETKİNLİKLER",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            txtArama = new Guna2TextBox()
            {
                PlaceholderText = "Etkinlik adına göre ara",
                Size = new Size(320, 40),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                BorderRadius = 10
            };
            this.Controls.Add(txtArama);

            btnYeniEkle = new Guna2Button()
            {
                Text = "+ Yeni Etkinlik Ekle",
                Size = new Size(200, 40),
                Location = new Point(370, 80),
                BorderRadius = 10,
                FillColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(btnYeniEkle);

            flpKartlar = new FlowLayoutPanel()
            {
                Location = new Point(5, 140),
                Size = new Size(this.Width - 60, this.Height - 180),
                AutoScroll = true,
                WrapContents = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(flpKartlar);
            flpKartlar.FlowDirection = FlowDirection.LeftToRight;
            flpKartlar.WrapContents = true;
        }

        //private void EtkinlikKartlariniOlustur()
        //{
        //    flpKartlar.Controls.Clear();

        //    using (var db = new AppDbContext())
        //    {
        //        var etkinlikler = db.Etkinlikler.AsQueryable();

        //        if (rol == 3)
        //        {
        //            etkinlikler = etkinlikler.Where(x => x.KullaniciID == aktifKullaniciId);
        //        }

        //        foreach (var etkinlik in etkinlikler.ToList())
        //        {
        //            Guna2Panel kart = new Guna2Panel();
        //            kart.Size = new Size(300, 350);
        //            kart.BorderRadius = 15;
        //            kart.FillColor = Color.White;
        //            kart.ShadowDecoration.Enabled = true;
        //            kart.ShadowDecoration.Depth = 10;
        //            kart.Margin = new Padding(15);

        //            PictureBox pb = new PictureBox();
        //            pb.ImageLocation = Path.Combine(Application.StartupPath, "Gorseller", etkinlik.GorselYolu);
        //            pb.Size = new Size(280, 150);
        //            pb.SizeMode = PictureBoxSizeMode.StretchImage;
        //            pb.Location = new Point(10, 10);
        //            kart.Controls.Add(pb);

        //            Label lblAd = new Label();
        //            lblAd.Text = etkinlik.EtkinlikAdi;
        //            lblAd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        //            lblAd.Location = new Point(10, 170);
        //            lblAd.AutoSize = true;
        //            kart.Controls.Add(lblAd);

        //            Label lblTur = new Label();
        //            lblTur.Text = $"Tür: {etkinlik.EtkinlikTuru.TurAdi} | Süre: {etkinlik.SureDakika} dk";
        //            lblTur.Font = new Font("Segoe UI", 9);
        //            lblTur.Location = new Point(10, 200);
        //            lblTur.AutoSize = true;
        //            kart.Controls.Add(lblTur);

        //            Label lblTarih = new Label();
        //            lblTarih.Text = $"📅 {etkinlik.Tarih.ToString("dd.MM.yyyy")} ⏰ {etkinlik.Saat}";
        //            lblTarih.Font = new Font("Segoe UI", 9, FontStyle.Italic);
        //            lblTarih.ForeColor = Color.Gray;
        //            lblTarih.Location = new Point(10, 230);
        //            lblTarih.AutoSize = true;
        //            kart.Controls.Add(lblTarih);

        //            Guna2Button btnDuzenle = new Guna2Button();
        //            btnDuzenle.Text = "Düzenle";
        //            btnDuzenle.Size = new Size(100, 30);
        //            btnDuzenle.FillColor = Color.DodgerBlue;
        //            btnDuzenle.ForeColor = Color.White;
        //            btnDuzenle.Location = new Point(10, 270);
        //            kart.Controls.Add(btnDuzenle);

        //            Guna2Button btnSil = new Guna2Button();
        //            btnSil.Text = "Sil";
        //            btnSil.Size = new Size(100, 30);
        //            btnSil.FillColor = Color.Crimson;
        //            btnSil.ForeColor = Color.White;
        //            btnSil.Location = new Point(120, 270);
        //            kart.Controls.Add(btnSil);

        //            flpKartlar.Controls.Add(kart);
        //        }
        //    }
        //}
        private void EtkinlikKartlariniOlustur()
        {
            flpKartlar.Controls.Clear();

            // Sahte veri listesi
            var etkinlikler = new List<dynamic>()
    {
        new {
            EtkinlikAdi = "Rock Festivali",
            TurAdi = "Konser",
            SureDakika = 120,
            GorselYolu = "afis.jpg",
            Tarih = new DateTime(2025, 7, 15),
            Saat = new TimeSpan(20, 0, 0)
        },
        new {
            EtkinlikAdi = "Aile Arasında",
            TurAdi = "Tiyatro",
            SureDakika = 100,
            GorselYolu = "aile.jpg",
            Tarih = new DateTime(2025, 7, 17),
            Saat = new TimeSpan(18, 30, 0)
        },
         new {
            EtkinlikAdi = " Festivali",
            TurAdi = "Konser",
            SureDakika = 120,
            GorselYolu = "afis.jpg",
            Tarih = new DateTime(2025, 7, 15),
            Saat = new TimeSpan(20, 0, 0)
        },
    };

            foreach (var etkinlik in etkinlikler)
            {
                Guna2Panel kart = new Guna2Panel();
                kart.Size = new Size(240,330/*300, 350*/);
                kart.BorderRadius = 15;
                kart.FillColor = Color.White;
                kart.ShadowDecoration.Enabled = true;
                kart.ShadowDecoration.Depth = 10;
                kart.Margin = new Padding(8);

                PictureBox pb = new PictureBox();
                pb.ImageLocation = Path.Combine(Application.StartupPath, "Gorseller", etkinlik.GorselYolu);
                pb.Size = new Size(230,140/*280, 150*/);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Location = new Point(10, 10);
                kart.Controls.Add(pb);

                Label lblAd = new Label();
                lblAd.Text = etkinlik.EtkinlikAdi;
                lblAd.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblAd.Location = new Point(10, 170);
                lblAd.AutoSize = true;
                kart.Controls.Add(lblAd);

                Label lblTur = new Label();
                lblTur.Text = $"Tür: {etkinlik.TurAdi} | Süre: {etkinlik.SureDakika} dk";
                lblTur.Font = new Font("Segoe UI", 9);
                lblTur.Location = new Point(10, 200);
                lblTur.AutoSize = true;
                kart.Controls.Add(lblTur);

                Label lblTarih = new Label();
                lblTarih.Text = $"📅 {etkinlik.Tarih:dd.MM.yyyy} ⏰ {etkinlik.Saat:hh\\:mm}";
                lblTarih.Font = new Font("Segoe UI", 9, FontStyle.Italic);
                lblTarih.ForeColor = Color.Gray;
                lblTarih.Location = new Point(10, 230);
                lblTarih.AutoSize = true;
                kart.Controls.Add(lblTarih);

                Guna2Button btnDuzenle = new Guna2Button();
                btnDuzenle.Text = "Düzenle";
                btnDuzenle.Size = new Size(100, 30);
                btnDuzenle.FillColor = Color.DodgerBlue;
                btnDuzenle.ForeColor = Color.White;
                btnDuzenle.Location = new Point(10, 270);
                kart.Controls.Add(btnDuzenle);

                Guna2Button btnSil = new Guna2Button();
                btnSil.Text = "Sil";
                btnSil.Size = new Size(100, 30);
                btnSil.FillColor = Color.Crimson;
                btnSil.ForeColor = Color.White;
                btnSil.Location = new Point(120, 270);
                kart.Controls.Add(btnSil);

                flpKartlar.Controls.Add(kart);
            }
        }
        private void EtkinliklerKontrol_Load(object sender, EventArgs e)
        {
            if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                this.BackColor = Color.White;
            }
        }
    }
}

//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using EBOS.DataAccess;
//using Guna.UI2.WinForms;

//namespace EBOS
//{
//    public partial class EtkinliklerKontrol : UserControl
//    {
//        /// <summary>


//            private int? aktifKullaniciId;
//            private int? rol;

//            public EtkinliklerKontrol(int? kullaniciId = null, int? rol = null)
//            {
//                InitializeComponent();
//                this.Dock = DockStyle.Fill;

//                this.aktifKullaniciId = kullaniciId;
//                this.rol = rol;
//            }

//            private void Yukle()
//            {
//                using (var db = new AppDbContext())
//                {
//                    var veriler = db.Etkinlikler.AsQueryable();

//                    if (rol == 3) // organizatör ise
//                    {
//                        veriler = veriler.Where(x => x.KullaniciID == aktifKullaniciId);
//                    }

//                    dgvEtkinlikler.DataSource = veriler.ToList();
//                }
//            }


//        /// </summary>
//        private Label lblBaslik;
//        private Guna2TextBox txtArama;
//        private Guna2Button btnYeniEkle;
//        private Guna2DataGridView dgvEtkinlikler;
//        private Guna2Button btnExcel;

//        public EtkinliklerKontrol()
//        {
//            InitializeComponent();
//            this.Dock = DockStyle.Fill;
//            this.BackColor = Color.White;
//            ArayuzOlustur();
//        }

//        private void ArayuzOlustur()
//        {
//            // Başlık
//            lblBaslik = new Label()
//            {
//                Text = "ETKİNLİKLER",
//                Font = new Font("Segoe UI", 20, FontStyle.Bold),
//                Location = new Point(30, 20),
//                AutoSize = true
//            };
//            this.Controls.Add(lblBaslik);

//            // Arama kutusu
//            txtArama = new Guna2TextBox()
//            {
//                PlaceholderText = "Etkinlik adına göre ara",
//                Size = new Size(320, 40),
//                Location = new Point(30, 80),
//                Font = new Font("Segoe UI", 10),
//                BorderRadius = 10
//            };
//            this.Controls.Add(txtArama);

//            // Yeni Etkinlik Ekle Butonu
//            btnYeniEkle = new Guna2Button()
//            {
//                Text = "+ Yeni Etkinlik Ekle",
//                Size = new Size(200, 40),
//                Location = new Point(370, 80),
//                BorderRadius = 10,
//                FillColor = Color.FromArgb(0, 123, 255),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10, FontStyle.Bold)
//            };
//            this.Controls.Add(btnYeniEkle);

//            // DataGridView
//            dgvEtkinlikler = new Guna2DataGridView()
//            {
//                Location = new Point(30, 140),
//                Size = new Size(this.Width - 60, 300),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
//                ReadOnly = true,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
//                ColumnHeadersHeight = 40,
//                AllowUserToAddRows = false,
//                AllowUserToResizeRows = false,
//                ScrollBars = ScrollBars.Both,
//                RowTemplate = { Height = 40 },
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.WhiteSmoke }
//            };
//            dgvEtkinlikler.CellPainting += DgvEtkinlikler_CellPainting;
//            this.Controls.Add(dgvEtkinlikler);

//            // Kolonlar
//            dgvEtkinlikler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
//            dgvEtkinlikler.Columns[0].Width = 160;

//            dgvEtkinlikler.Columns.Add("Turu", "Türü");
//            dgvEtkinlikler.Columns[1].Width = 100;

//            dgvEtkinlikler.Columns.Add("Sure", "Süresi");
//            dgvEtkinlikler.Columns[2].Width = 80;

//            dgvEtkinlikler.Columns.Add("Gorsel", "Görsel");
//            dgvEtkinlikler.Columns[3].Width = 120;

//            dgvEtkinlikler.Columns.Add("Tarih", "Tarih");
//            dgvEtkinlikler.Columns[4].Width = 110;

//            dgvEtkinlikler.Columns.Add("Saat", "Saat");
//            dgvEtkinlikler.Columns[5].Width = 80;

//            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
//            editColumn.HeaderText = "";
//            editColumn.Text = "✏️";
//            editColumn.UseColumnTextForButtonValue = true;
//            editColumn.Width = 40;
//            dgvEtkinlikler.Columns.Add(editColumn);

//            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
//            deleteColumn.HeaderText = "";
//            deleteColumn.Text = "🗑️";
//            deleteColumn.UseColumnTextForButtonValue = true;
//            deleteColumn.Width = 40;
//            dgvEtkinlikler.Columns.Add(deleteColumn);

//            // Excel'e Aktar Butonu
//            btnExcel = new Guna2Button()
//            {
//                Text = "Excel'e Aktar",
//                Location = new Point(30, 460),
//                Size = new Size(150, 40),
//                BorderRadius = 10,
//                FillColor = Color.Black,
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 9, FontStyle.Bold),
//                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
//            };
//            this.Controls.Add(btnExcel);

//            // Örnek Veriler
//            dgvEtkinlikler.Rows.Add("Rock Festivali", "Konser", "120 dk", "afis.jpg", "15.07.2025", "20:00");
//            dgvEtkinlikler.Rows.Add("Aile Arasında", "Tiyatro", "100 dk", "aile.jpg", "17.07.2025", "18:30");
//        }

//        // HEADER RENKLENDİRME METODU
//        private void DgvEtkinlikler_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
//            {
//                e.PaintBackground(e.ClipBounds, true);
//                e.PaintContent(e.ClipBounds);

//                Color bgColor = Color.White;

//                switch (e.ColumnIndex)
//                {
//                    case 0: bgColor = Color.FromArgb(255, 64, 129); break;     // Etkinlik Adı
//                    case 1: bgColor = Color.FromArgb(33, 150, 243); break;     // Türü
//                    case 2: bgColor = Color.FromArgb(255, 152, 0); break;      // Süresi
//                    case 3: bgColor = Color.FromArgb(156, 39, 176); break;     // Görsel
//                    case 4: bgColor = Color.FromArgb(0, 200, 83); break;       // Tarih
//                    case 5: bgColor = Color.FromArgb(121, 85, 72); break;      // Saat
//                    default: bgColor = Color.Gray; break;                      // Edit/Sil
//                }

//                using (SolidBrush brush = new SolidBrush(bgColor))
//                {
//                    e.Graphics.FillRectangle(brush, e.CellBounds);
//                }

//                TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(),
//                    dgvEtkinlikler.ColumnHeadersDefaultCellStyle.Font,
//                    e.CellBounds, Color.White,
//                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

//                e.Handled = true;
//            }
//        }private void EtkinliklerKontrol_Load(object sender, EventArgs e)
//        {
//            if (TemaYonetici.AktifTema == "Koyu")
//            {
//                this.BackColor = Color.FromArgb(120, 120, 120);
//            }
//            else
//            {
//                this.BackColor = Color.White;
//            }
//        }
//    }
//}