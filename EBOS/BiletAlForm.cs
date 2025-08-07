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
        private readonly string _etkinlikAdi;
        private readonly string _kullaniciEposta;
        private List<Koltuk> _koltuklar;
        private Guna2Panel _koltukPanel;
        private Koltuk _seciliKoltuk;

        public BiletAlForm(string etkinlikAdi, string kullaniciEposta)
        {
            _etkinlikAdi = etkinlikAdi;
            _kullaniciEposta = kullaniciEposta;

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
                _koltuklar = db.Koltuklar.ToList();
            }
        }

        private void ArayuzOlustur()
        {
            _koltukPanel = new Guna2Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true,
                FillColor = Color.White
            };
            this.Controls.Add(_koltukPanel);

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
                    if (index >= _koltuklar.Count) break;

                    var koltuk = _koltuklar[index];
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
                        _seciliKoltuk = (Koltuk)((Guna2Button)s).Tag;
                        DialogResult sonuc = MessageBox.Show($"{_seciliKoltuk.KoltukNo} koltuğunu seçtiniz. Ödemeye geçilsin mi?", "Onay", MessageBoxButtons.YesNo);
                        if (sonuc == DialogResult.Yes)
                        {
                            OdemeForm odeme = new OdemeForm(_etkinlikAdi, _kullaniciEposta, _seciliKoltuk.KoltukID);
                            odeme.ShowDialog();
                            this.Close();
                        }
                    };

                    _koltukPanel.Controls.Add(btnKoltuk);
                }
            }
        }
    }
}


