using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    static class KoltukRenkleri
    {
        public static readonly Color Bos = Color.FromArgb(59, 201, 170);   // turkuaz
        public static readonly Color Dolu = Color.FromArgb(231, 76, 60);   // kırmızı
        public static readonly Color Secili = Color.FromArgb(241, 196, 15); // sarı
    }

    public partial class KoltukSecimForm : Form
    {
        private readonly int seansId;
        private readonly string kullaniciEposta;
        private readonly string etkinlikAdi;

        private TableLayoutPanel salonPanel;
        private Button seciliBtn;
        private Koltuk seciliKoltuk;

        public KoltukSecimForm(int seansId, string etkinlikAdi, string eposta)
        {


            this.seansId = seansId;
            this.etkinlikAdi = etkinlikAdi;
            this.kullaniciEposta = eposta;
            InitializeComponent();
            InitLayout();
            OlusturLegend();
            KoltuklariYukle();
        }

        private void InitLayout()
        {
            Text = "Koltuk Seçimi";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 800);
            BackColor = Color.FromArgb(22, 33, 62); // Koyu mavi arka plan

            // Legend paneli yukarıya
            OlusturLegend();

            // Salon koltukları için tablo
            salonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(40, 20, 40, 10),
                RowCount = 10,
                ColumnCount = 12,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            for (int i = 0; i < salonPanel.RowCount; i++)
                salonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / salonPanel.RowCount));
            for (int i = 0; i < salonPanel.ColumnCount; i++)
                salonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / salonPanel.ColumnCount));

            var salonDisPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(22, 33, 62)
            };
            salonDisPanel.Controls.Add(salonPanel);
            Controls.Add(salonDisPanel);

            // Perde label en alta
            var perdeLabel = new Label
            {
                Text = "PERDE",
                Dock = DockStyle.Bottom,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(44, 62, 80)
            };
            Controls.Add(perdeLabel);
        }

        private void OlusturLegend()
        {
            var legend = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(22, 33, 62)
            };

            void Add(Color color, string text)
            {
                legend.Controls.Add(new Panel
                {
                    Width = 20,
                    Height = 20,
                    BackColor = color,
                    Margin = new Padding(5)
                });

                legend.Controls.Add(new Label
                {
                    Text = text,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.White,
                    Margin = new Padding(0, 2, 20, 0)
                });
            }

            Add(KoltukRenkleri.Dolu, "Dolu koltuklar");
            Add(KoltukRenkleri.Bos, "Boş koltuklar");
            Add(KoltukRenkleri.Secili, "Seçiminiz");

            Controls.Add(legend);
            legend.BringToFront();
        }

        private void KoltuklariYukle()
        {
            using var db = new AppDbContext();

            var doluKoltuklar = db.Biletler
                                  .Where(b => b.SeansID == seansId)
                                  .Select(b => b.KoltukID)
                                  .ToHashSet();

            var koltuklar = db.Koltuklar
                              .OrderBy(k => k.Satir)
                              .ThenBy(k => k.Sutun)
                              .ToList();

            salonPanel.Controls.Clear();

            foreach (var k in koltuklar)
            {
                var btn = new Button
                {
                    Text = k.KoltukNo,
                    Tag = k,

                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = doluKoltuklar.Contains(k.KoltukID) ? KoltukRenkleri.Dolu : KoltukRenkleri.Bos,
                    Enabled = !doluKoltuklar.Contains(k.KoltukID),
                    ForeColor = Color.White,
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += Koltuk_Click;

                int row = k.Satir - 1;
                int col = k.Sutun - 1;
                if (row < salonPanel.RowCount && col < salonPanel.ColumnCount)
                    salonPanel.Controls.Add(btn, col, row);
            }
        }

        private void Koltuk_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var secilenKoltuk = (Koltuk)btn.Tag;

            if (btn.BackColor == KoltukRenkleri.Secili)
            {
                btn.BackColor = KoltukRenkleri.Bos;
                seciliBtn = null;
                seciliKoltuk = null;
                return;
            }
            if (seciliBtn != null)
                seciliBtn.BackColor = KoltukRenkleri.Bos;

            btn.BackColor = KoltukRenkleri.Secili;
            seciliBtn = btn;
            seciliKoltuk = secilenKoltuk;

            DialogResult sonuc = MessageBox.Show(
                $"{secilenKoltuk.KoltukNo} koltuğunu seçtiniz.\nÖdemeye geçmek ister misiniz?",
                "Koltuk Seçildi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {


                using var odeme = new OdemeForm(etkinlikAdi, kullaniciEposta, seciliKoltuk.KoltukID);
                odeme.ShowDialog();

                this.Close();


            }
        }
    }
}