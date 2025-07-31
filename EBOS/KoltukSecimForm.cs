using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using EBOS.DataAccess;
using EBOS.Entities;

namespace EBOS
{
    public partial class KoltukSecimForm : Form
    {
        private string kullaniciEposta;
        private string etkinlikAdi;
        private TableLayoutPanel salonPanel;
        private Button seciliKoltukBtn;
        private Koltuk seciliKoltuk;

        public KoltukSecimForm(string etkinlikAdi, string eposta)
        {
            this.etkinlikAdi = etkinlikAdi;
            this.kullaniciEposta = eposta;
            this.Size = new Size(1000, 750);
            this.Text = "Koltuk Seçimi";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Perde başlık
            Label perdeLabel = new Label()
            {
                Text = "PERDE",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };
            this.Controls.Add(perdeLabel);

            // Salon paneli
            salonPanel = new TableLayoutPanel()
            {
                RowCount = 5,
                ColumnCount = 11, // 10 koltuk + 1 boşluk
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.White,
            };

            for (int i = 0; i < salonPanel.RowCount; i++)
                salonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));

            for (int i = 0; i < salonPanel.ColumnCount; i++)
                salonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / salonPanel.ColumnCount));

            this.Controls.Add(salonPanel);

            this.Load += KoltukSecimForm_Load;
        }

        private void KoltukSecimForm_Load(object sender, EventArgs e)
        {
            KoltuklariYukle();
        }

        private void KoltuklariYukle()
        {
            using (var db = new AppDbContext())
            {
                var koltuklar = db.Koltuklar.Take(100).ToList();
                var doluKoltuklar = db.Biletler.Select(b => b.KoltukID).ToList();

                salonPanel.Controls.Clear();

                foreach (var k in koltuklar)
                {
                    int sutunIndex = k.Sutun >= 6 ? k.Sutun + 1 : k.Sutun; // Ortada 1 boşluk
                    Button btn = new Button()
                    {
                        Text = k.KoltukNo,
                        Tag = k,
                        Width = 60,
                        Height = 50,
                        Dock = DockStyle.Fill,
                        BackColor = doluKoltuklar.Contains(k.KoltukID) ? Color.IndianRed : Color.LightGreen,
                        Enabled = !doluKoltuklar.Contains(k.KoltukID),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };

                    btn.Click += (s, e) =>
                    {
                        if (seciliKoltukBtn != null && seciliKoltukBtn.BackColor != Color.IndianRed)
                            seciliKoltukBtn.BackColor = Color.LightGreen;

                        seciliKoltukBtn = (Button)s;
                        seciliKoltukBtn.BackColor = Color.Orange;
                        seciliKoltuk = (Koltuk)seciliKoltukBtn.Tag;
                    };

                    // Sadece salon boyutundaki alanlara ekle
                    if (k.Satir <= 5 && k.Sutun <= 10)
                        salonPanel.Controls.Add(btn, sutunIndex - 1, k.Satir - 1); // 0-based index
                }

                // Ödeme butonu
                var odemeBtn = new Guna2Button()
                {
                    Text = "Ödemeye Geç",
                    Size = new Size(220, 50),
                    Location = new Point((this.Width - 220) / 2, this.Height - 100),
                    Anchor = AnchorStyles.Bottom,
                    FillColor = Color.DarkGreen,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    BorderRadius = 10
                };

                odemeBtn.Click += (s, e) =>
                {
                    if (seciliKoltuk == null)
                    {
                        MessageBox.Show("Lütfen bir koltuk seçin.");
                        return;
                    }

                    OdemeForm odeme = new OdemeForm(etkinlikAdi, kullaniciEposta, seciliKoltuk.KoltukID);
                    odeme.ShowDialog();
                    this.Close();
                };

                this.Controls.Add(odemeBtn);
                odemeBtn.BringToFront();
            }
        }
    }
}


//using System;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using Guna.UI2.WinForms;
//using EBOS.DataAccess;
//using EBOS.Entities;

//namespace EBOS
//{
//    public partial class KoltukSecimForm : Form
//    {
//        private string kullaniciEposta;
//        private string etkinlikAdi;
//        private FlowLayoutPanel panel;
//        private Button seciliKoltukBtn;
//        private Koltuk seciliKoltuk;

//        public KoltukSecimForm(string etkinlikAdi, string eposta)
//        {
//            this.etkinlikAdi = etkinlikAdi;
//            this.kullaniciEposta = eposta;
//            this.Size = new Size(700, 600);
//            this.Text = "Koltuk Seçimi";
//            this.StartPosition = FormStartPosition.CenterScreen;

//            panel = new FlowLayoutPanel()
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(20),
//                AutoScroll = true,
//                WrapContents = true
//            };
//            this.Controls.Add(panel);

//            this.Load += KoltukSecimForm_Load;
//        }

//        private void KoltukSecimForm_Load(object sender, EventArgs e)
//        {
//            KoltuklariYukle();
//        }

//        private void KoltuklariYukle()
//        {
//            using (var db = new AppDbContext())
//            {
//                var koltuklar = db.Koltuklar.Take(100).ToList();
//                foreach (var k in koltuklar)
//                {
//                    var btn = new Button()
//                    {
//                        Text = k.KoltukNo,
//                        Size = new Size(80, 50),
//                        BackColor = Color.LightGray,
//                        Tag = k
//                    };

//                    btn.Click += (s, e) =>
//                    {
//                        if (seciliKoltukBtn != null)
//                            seciliKoltukBtn.BackColor = Color.LightGray;

//                        seciliKoltukBtn = (Button)s;
//                        seciliKoltukBtn.BackColor = Color.Green;
//                        seciliKoltuk = (Koltuk)seciliKoltukBtn.Tag;
//                    };

//                    panel.Controls.Add(btn);
//                }

//                var odemeBtn = new Guna2Button()
//                {
//                    Text = "Ödemeye Geç",
//                    Size = new Size(200, 45),
//                    Location = new Point((this.Width - 200) / 2, this.Height - 100),
//                    Anchor = AnchorStyles.Bottom,
//                    FillColor = Color.SeaGreen,
//                    ForeColor = Color.White,
//                    BorderRadius = 8
//                };

//                odemeBtn.Click += (s, e) =>
//                {
//                    if (seciliKoltuk == null)
//                    {
//                        MessageBox.Show("Lütfen bir koltuk seçin.");
//                        return;
//                    }

//                    OdemeForm odeme = new OdemeForm(etkinlikAdi, kullaniciEposta, seciliKoltuk.KoltukID);
//                    odeme.ShowDialog();
//                    this.Close();
//                };

//                panel.Controls.Add(odemeBtn);
//            }
//        }
//    }
//}

