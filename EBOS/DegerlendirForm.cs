using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class DegerlendirForm : Form
    {
        private string etkinlikAdi;
        private string kullaniciEposta;

        private NumericUpDown nudPuan;
        private TextBox txtYorum;
        private FlowLayoutPanel yorumPanel;

        public DegerlendirForm(string etkinlikAdi, string eposta)
        {
            this.etkinlikAdi = etkinlikAdi;
            this.kullaniciEposta = eposta;

            this.Text = "Etkinlik Değerlendirme";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = TemaYonetici.AktifTema == "Koyu" ? Color.FromArgb(60, 60, 60) : Color.White;

            ArayuzOlustur();
            YorumlariYukle();
        }

        private void ArayuzOlustur()
        {
            Label lblBaslik = new Label
            {
                Text = $"Etkinlik: {etkinlikAdi}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20),
                ForeColor = Color.Black
            };
            this.Controls.Add(lblBaslik);

            Label lblPuan = new Label
            {
                Text = "Puan (1-5):",
                Location = new Point(20, 70),
                AutoSize = true,
                ForeColor = Color.Black
            };
            this.Controls.Add(lblPuan);

            nudPuan = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 5,
                Location = new Point(100, 65),
                Size = new Size(60, 30)
            };
            this.Controls.Add(nudPuan);

            Label lblYorum = new Label
            {
                Text = "Yorumunuz:",
                Location = new Point(20, 115),
                AutoSize = true,
                ForeColor = Color.Black
            };
            this.Controls.Add(lblYorum);

            txtYorum = new TextBox
            {
                Multiline = true,
                Size = new Size(250, 150),
                Location = new Point(20, 140)
            };
            this.Controls.Add(txtYorum);

            var btnKaydet = new Guna2Button
            {
                Text = "Kaydet",
                Size = new Size(120, 40),
                Location = new Point(20, 310),
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 120, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnKaydet.Click += BtnKaydet_Click;
            this.Controls.Add(btnKaydet);

            // Sağ panel - yorumlar listesi
            yorumPanel = new FlowLayoutPanel
            {
                Location = new Point(300, 20),
                Size = new Size(320, 350),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.Controls.Add(yorumPanel);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtYorum.Text)) return;

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == kullaniciEposta);
                if (kullanici == null) return;

                var yeniYorum = new Yorum
                {
                    EtkinlikAdi = etkinlikAdi,
                    KullaniciID = kullanici.KullaniciID,
                    Icerik = txtYorum.Text.Trim(),
                    Tarih = DateTime.Now
                };

                db.Yorumlar.Add(yeniYorum);
                db.SaveChanges();
            }

            txtYorum.Clear();
            YorumlariYukle();
        }

        private void YorumlariYukle()
        {
            yorumPanel.Controls.Clear();

            using (var db = new AppDbContext())
            {
                var yorumlar = db.Yorumlar
                                 .Where(y => y.EtkinlikAdi == etkinlikAdi)
                                 .OrderByDescending(y => y.Tarih)
                                 .ToList();

                foreach (var yorum in yorumlar)
                {
                    var lblYorum = new Label
                    {
                        Text = yorum.Icerik + $" ({yorum.Tarih:g})",
                        AutoSize = false,
                        Width = 300,
                        Height = 60,
                        Padding = new Padding(5),
                        BackColor = Color.FromArgb(240, 240, 240),
                        Margin = new Padding(5)
                    };
                    yorumPanel.Controls.Add(lblYorum);
                }
            }
        }
    }
}

