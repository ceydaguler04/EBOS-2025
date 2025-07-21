using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public partial class KampanyalarKontrol : UserControl
    {
        private Guna2DataGridView kampanyaGrid;
        private Guna2Button btnYeniKampanya;

        public KampanyalarKontrol()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // Başlık
            Label lblBaslik = new Label();
            lblBaslik.Text = "Kampanya Yönetimi";
            lblBaslik.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblBaslik.Location = new Point(20, 20);
            lblBaslik.AutoSize = true;
            this.Controls.Add(lblBaslik);

            // + Yeni Kampanya Ekle Butonu
            btnYeniKampanya = new Guna2Button();
            btnYeniKampanya.Text = "+ Yeni Kampanya";
            btnYeniKampanya.Size = new Size(180, 40);
            btnYeniKampanya.FillColor = Color.FromArgb(232, 62, 140); // Pembe
            btnYeniKampanya.ForeColor = Color.White;
            btnYeniKampanya.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnYeniKampanya.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnYeniKampanya.BorderRadius = 15;
            btnYeniKampanya.Cursor = Cursors.Hand;
            btnYeniKampanya.Location = new Point(this.Width - 200, 25);
            btnYeniKampanya.Click += (s, e) =>
            {
                MessageBox.Show("Yeni kampanya ekleme işlemi burada olacak.");
            };
            this.Controls.Add(btnYeniKampanya);

            // Resize olayında konumu dinamik olarak güncelle
            this.Resize += (s, e) =>
            {
                btnYeniKampanya.Location = new Point(this.Width - btnYeniKampanya.Width - 20, 25);
                kampanyaGrid.Size = new Size(this.Width - 40, 280);
            };

            // DataGridView
            kampanyaGrid = new Guna2DataGridView();
            kampanyaGrid.Location = new Point(20, 80);
            kampanyaGrid.Size = new Size(this.Width - 40, 280);
            kampanyaGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            kampanyaGrid.ReadOnly = true;
            kampanyaGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            kampanyaGrid.AllowUserToAddRows = false;
            kampanyaGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            kampanyaGrid.RowHeadersVisible = false;
            kampanyaGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Başlık stili
            kampanyaGrid.EnableHeadersVisualStyles = false;
            kampanyaGrid.ColumnHeadersHeight = 40;
            kampanyaGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            kampanyaGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            kampanyaGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            kampanyaGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            kampanyaGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Kolonlar
            kampanyaGrid.Columns.Add("Ad", "Kampanya Adı");
            kampanyaGrid.Columns.Add("Baslangic", "Başlangıç Tarihi");
            kampanyaGrid.Columns.Add("Bitis", "Bitiş Tarihi");
            kampanyaGrid.Columns.Add("Aktif", "Aktif Mi?");

            // Tüm kolonları kilitle
            foreach (DataGridViewColumn col in kampanyaGrid.Columns)
                col.ReadOnly = true;

            // Satırlar
            kampanyaGrid.Rows.Add("Yaz İndirimi", "10.07.2025", "20.07.2025", "✓");
            kampanyaGrid.Rows.Add("Tiyatro 2 Al 1", "15.07.2025", "31.07.2025", "✓");

            // Her başlık için özel renk (Dashboard uyumlu)
            kampanyaGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray; // geçici, override ediliyor
            kampanyaGrid.CellPainting += KampanyaGrid_CellPainting;

            this.Controls.Add(kampanyaGrid);
        }

        private void KampanyaGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, false);

                Color bgColor = e.ColumnIndex switch
                {
                    0 => Color.FromArgb(232, 62, 140),   // pembe
                    1 => Color.FromArgb(0, 123, 255),    // mavi
                    2 => Color.FromArgb(255, 193, 7),    // sarı
                    3 => Color.FromArgb(111, 66, 193),   // mor
                    _ => Color.Gray
                };

                using (SolidBrush brush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(brush, e.CellBounds);

                TextRenderer.DrawText(e.Graphics, e.FormattedValue?.ToString() ?? "",
                    e.CellStyle.Font, e.CellBounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }
        private void KampanyalarKontrol_Load(object sender, EventArgs e)
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
