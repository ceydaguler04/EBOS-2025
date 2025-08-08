using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class BiletAlForm : Form
    {
        private readonly string etkinlikAdi;
        private readonly int etkinlikId;
        private readonly string kullaniciEposta;
        private List<Koltuk> koltuklar;
        private Guna2Panel koltukPanel;
        private Koltuk seciliKoltuk;

        public BiletAlForm(int etkinlikId, string etkinlikAdi, string kullaniciEposta)
        {
            InitializeComponent();
            this.etkinlikId = etkinlikId;
            this.etkinlikAdi = etkinlikAdi;
            this.kullaniciEposta = kullaniciEposta;


            this.Text = "Koltuk Seçimi";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            YukleKoltuklar();
            ArayuzOlustur();
        }

        private void YukleKoltuklar()
        {
            using (var db = new AppDbContext())
            {
                koltuklar = db.Koltuklar.ToList();
            }
        }

        private void ArayuzOlustur()
        {
            koltukPanel = new Guna2Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true,
                FillColor = Color.White
            };
            this.Controls.Add(koltukPanel);

            int satir = 10;
            int sutun = 10;
            int koltukGenislik = 60;
            int koltukYukseklik = 40;
            int aralik = 10;

            for (int i = 0; i < satir; i++)
            {
                for (int j = 0; j < sutun; j++)
                {
                    int index = i * sutun + j;
                    if (index >= koltuklar.Count) break;

                    var koltuk = koltuklar[index];
                    var btnKoltuk = new Guna2Button()
                    {
                        Text = koltuk.KoltukNo,
                        Size = new Size(koltukGenislik, koltukYukseklik),
                        Location = new Point(j * (koltukGenislik + aralik), i * (koltukYukseklik + aralik)),
                        Tag = koltuk,
                        BorderRadius = 5,
                        FillColor = Color.LightGreen
                    };

                    btnKoltuk.Click += (s, e) =>
                    {
                        seciliKoltuk = (Koltuk)((Guna2Button)s).Tag;
                        DialogResult sonuc = MessageBox.Show(
                            $"{seciliKoltuk.KoltukNo} koltuğunu seçtiniz. Ödemeye geçilsin mi?",
                            "Onay", MessageBoxButtons.YesNo);

                        if (sonuc == DialogResult.Yes)
                        {
                            using (var odeme = new OdemeForm(this.etkinlikId, this.etkinlikAdi, this.kullaniciEposta, seciliKoltuk.KoltukID))
                            {
                                if (odeme.ShowDialog() == DialogResult.OK)
                                    this.DialogResult = DialogResult.OK;
                            }
                            this.Close();
                        }
                    };

                    //btnKoltuk.Click += (s, e) =>
                    //{
                    //    seciliKoltuk = (Koltuk)((Guna2Button)s).Tag;
                    //    DialogResult sonuc = MessageBox.Show($"{seciliKoltuk.KoltukNo} koltuğunu seçtiniz. Ödemeye geçilsin mi?", "Onay", MessageBoxButtons.YesNo);
                    //    if (sonuc == DialogResult.Yes)
                    //    {
                    //        OdemeForm odeme = new OdemeForm(etkinlikId, etkinlikAdi, kullaniciEposta, seciliKoltuk.KoltukID);
                    //        odeme.ShowDialog();
                    //        this.Close();
                    //    }
                    //};

                    koltukPanel.Controls.Add(btnKoltuk);
                }
            }
        }
    }
}


