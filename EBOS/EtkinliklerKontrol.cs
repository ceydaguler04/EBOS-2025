using System;
using System.Drawing;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class EtkinliklerKontrol : UserControl
    {
        /// <summary>
        
        
            private int? aktifKullaniciId;
            private int? rol;

            public EtkinliklerKontrol(int? kullaniciId = null, int? rol = null)
            {
                InitializeComponent();
                this.Dock = DockStyle.Fill;

                this.aktifKullaniciId = kullaniciId;
                this.rol = rol;
            }

            private void Yukle()
            {
                using (var db = new AppDbContext())
                {
                    var veriler = db.Etkinlikler.AsQueryable();

                    if (rol == 3) // organizatör ise
                    {
                        veriler = veriler.Where(x => x.KullaniciID == aktifKullaniciId);
                    }

                    dgvEtkinlikler.DataSource = veriler.ToList();
                }
            }
        

        /// </summary>
        private Label lblBaslik;
        private Guna2TextBox txtArama;
        private Guna2Button btnYeniEkle;
        private Guna2DataGridView dgvEtkinlikler;
        private Guna2Button btnExcel;

        public EtkinliklerKontrol()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            ArayuzOlustur();
        }

        private void ArayuzOlustur()
        {
            // Başlık
            lblBaslik = new Label()
            {
                Text = "ETKİNLİKLER",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            // Arama kutusu
            txtArama = new Guna2TextBox()
            {
                PlaceholderText = "Etkinlik adına göre ara",
                Size = new Size(320, 40),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                BorderRadius = 10
            };
            this.Controls.Add(txtArama);

            // Yeni Etkinlik Ekle Butonu
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

            // DataGridView
            dgvEtkinlikler = new Guna2DataGridView()
            {
                Location = new Point(30, 140),
                Size = new Size(this.Width - 60, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeight = 40,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both,
                RowTemplate = { Height = 40 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.WhiteSmoke }
            };
            dgvEtkinlikler.CellPainting += DgvEtkinlikler_CellPainting;
            this.Controls.Add(dgvEtkinlikler);

            // Kolonlar
            dgvEtkinlikler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
            dgvEtkinlikler.Columns[0].Width = 160;

            dgvEtkinlikler.Columns.Add("Turu", "Türü");
            dgvEtkinlikler.Columns[1].Width = 100;

            dgvEtkinlikler.Columns.Add("Sure", "Süresi");
            dgvEtkinlikler.Columns[2].Width = 80;

            dgvEtkinlikler.Columns.Add("Gorsel", "Görsel");
            dgvEtkinlikler.Columns[3].Width = 120;

            dgvEtkinlikler.Columns.Add("Tarih", "Tarih");
            dgvEtkinlikler.Columns[4].Width = 110;

            dgvEtkinlikler.Columns.Add("Saat", "Saat");
            dgvEtkinlikler.Columns[5].Width = 80;

            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
            editColumn.HeaderText = "";
            editColumn.Text = "✏️";
            editColumn.UseColumnTextForButtonValue = true;
            editColumn.Width = 40;
            dgvEtkinlikler.Columns.Add(editColumn);

            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "";
            deleteColumn.Text = "🗑️";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.Width = 40;
            dgvEtkinlikler.Columns.Add(deleteColumn);

            // Excel'e Aktar Butonu
            btnExcel = new Guna2Button()
            {
                Text = "Excel'e Aktar",
                Location = new Point(30, 460),
                Size = new Size(150, 40),
                BorderRadius = 10,
                FillColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.Controls.Add(btnExcel);

            // Örnek Veriler
            dgvEtkinlikler.Rows.Add("Rock Festivali", "Konser", "120 dk", "afis.jpg", "15.07.2025", "20:00");
            dgvEtkinlikler.Rows.Add("Aile Arasında", "Tiyatro", "100 dk", "aile.jpg", "17.07.2025", "18:30");
        }

        // HEADER RENKLENDİRME METODU
        private void DgvEtkinlikler_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                e.PaintContent(e.ClipBounds);

                Color bgColor = Color.White;

                switch (e.ColumnIndex)
                {
                    case 0: bgColor = Color.FromArgb(255, 64, 129); break;     // Etkinlik Adı
                    case 1: bgColor = Color.FromArgb(33, 150, 243); break;     // Türü
                    case 2: bgColor = Color.FromArgb(255, 152, 0); break;      // Süresi
                    case 3: bgColor = Color.FromArgb(156, 39, 176); break;     // Görsel
                    case 4: bgColor = Color.FromArgb(0, 200, 83); break;       // Tarih
                    case 5: bgColor = Color.FromArgb(121, 85, 72); break;      // Saat
                    default: bgColor = Color.Gray; break;                      // Edit/Sil
                }

                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(),
                    dgvEtkinlikler.ColumnHeadersDefaultCellStyle.Font,
                    e.CellBounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }private void EtkinliklerKontrol_Load(object sender, EventArgs e)
        {
            if (YoneticiPaneli.AktifTema == "Koyu")
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
//using Guna.UI2.WinForms;

//namespace EBOS
//{
//    public partial class EtkinliklerKontrol : UserControl
//    {
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
//                Size = new Size(800, 300),
//                ReadOnly = true,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                ColumnHeadersHeight = 40,
//                AllowUserToAddRows = false,
//                AllowUserToResizeRows = false,
//                RowTemplate = { Height = 40 },
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.WhiteSmoke }
//            };
//            dgvEtkinlikler.CellPainting += DgvEtkinlikler_CellPainting;
//            this.Controls.Add(dgvEtkinlikler);

//            // Kolonlar
//            dgvEtkinlikler.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
//            dgvEtkinlikler.Columns.Add("Turu", "Türü");
//            dgvEtkinlikler.Columns.Add("Sure", "Süresi");
//            dgvEtkinlikler.Columns.Add("Gorsel", "Görsel");

//            // Düzenle kolon (emoji)
//            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
//            editColumn.HeaderText = "";
//            editColumn.Text = "✏️";
//            editColumn.UseColumnTextForButtonValue = true;
//            editColumn.Width = 40;
//            dgvEtkinlikler.Columns.Add(editColumn);

//            // Sil kolon (emoji)
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
//                Font = new Font("Segoe UI", 9, FontStyle.Bold)
//            };
//            this.Controls.Add(btnExcel);
//            dgvEtkinlikler.Rows.Add("Rock Festivali", "Konser", "120 dk", "afis.jpg");
//            dgvEtkinlikler.Rows.Add("Aile Arasında", "Tiyatro", "100 dk", "aile.jpg");

//        }

//        // 🟣 HEADER RENKLENDİRME METODU
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
//                    default: bgColor = Color.Gray; break;
//                }

//                using (SolidBrush brush = new SolidBrush(bgColor))
//                {
//                    e.Graphics.FillRectangle(brush, e.CellBounds);
//                }

//                TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(), dgvEtkinlikler.ColumnHeadersDefaultCellStyle.Font,
//                    e.CellBounds, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

//                e.Handled = true;
//            }
//        }
//        private void EtkinliklerKontrol_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}