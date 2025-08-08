using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Linq;
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

            Label lblBaslik = new Label
            {
                Text = "Kampanya Yönetimi",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            btnYeniKampanya = new Guna2Button
            {
                Text = "+ Yeni Kampanya",
                Size = new Size(180, 40),
                FillColor = Color.FromArgb(232, 62, 140),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 15,
                Cursor = Cursors.Hand,
                Location = new Point(this.Width - 200, 25)
            };
            btnYeniKampanya.Click += BtnYeniKampanya_Click;
            this.Controls.Add(btnYeniKampanya);

            kampanyaGrid = new Guna2DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(this.Width - 40, this.Height - 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                ReadOnly = true,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };

            kampanyaGrid.Columns.Add("Ad", "Kampanya Adı");
            kampanyaGrid.Columns.Add("Kod", "Kampanya Kodu");
            kampanyaGrid.Columns.Add("MinTutar", "Minimum Tutar (₺)");
            kampanyaGrid.Columns.Add("Indirim", "İndirim (%)");
            kampanyaGrid.Columns.Add("Baslangic", "Başlangıç");
            kampanyaGrid.Columns.Add("Bitis", "Bitiş");
            kampanyaGrid.Columns.Add("Aktif", "Aktif Mi?");

            kampanyaGrid.CellPainting += KampanyaGrid_CellPainting;

            this.Controls.Add(kampanyaGrid);

            this.Resize += (s, e) =>
            {
                btnYeniKampanya.Location = new Point(this.Width - btnYeniKampanya.Width - 20, 25);
                kampanyaGrid.Size = new Size(this.Width - 40, this.Height - 120);
            };

            this.Load += KampanyalarKontrol_Load;
        }

        private void KampanyalarKontrol_Load(object sender, EventArgs e)
        {
            if (TemaYonetici.AktifTema == "Koyu")
                this.BackColor = Color.FromArgb(120, 120, 120);
            else
                this.BackColor = Color.White;

            KampanyalariYukle();
        }

        private void KampanyalariYukle()
        {
            kampanyaGrid.Rows.Clear();

            using (var db = new AppDbContext())
            {
                var kampanyalar = db.Kampanyalar.ToList();

                foreach (var k in kampanyalar)
                {
                    kampanyaGrid.Rows.Add(
                        k.KampanyaAdi,
                        k.KampanyaKodu,
                        k.MinTutar.HasValue ? $"{k.MinTutar.Value:N2}" : "-",
                        k.IndirimYuzdesi + "%",
                        k.BaslangicTarihi.ToString("dd.MM.yyyy"),
                        k.BitisTarihi.ToString("dd.MM.yyyy"),
                        k.AktifMi ? "✓" : ""
                    );
                }
            }
        }

        private void BtnYeniKampanya_Click(object sender, EventArgs e)
        {
            KampanyaEkleForm form = new KampanyaEkleForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                KampanyalariYukle();
            }
        }

        private void KampanyaGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, false);

                Color bgColor = e.ColumnIndex switch
                {
                    0 => Color.FromArgb(232, 62, 140),
                    1 => Color.FromArgb(0, 123, 255),
                    2 => Color.FromArgb(40, 167, 69),
                    3 => Color.FromArgb(255, 193, 7),
                    4 => Color.FromArgb(23, 162, 184),
                    5 => Color.FromArgb(111, 66, 193),
                    6 => Color.FromArgb(108, 117, 125),
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
    }
}
