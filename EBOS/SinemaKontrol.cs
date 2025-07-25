using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class SinemaKontrol : UserControl
    {
        private string kullaniciEposta;

        public SinemaKontrol(string eposta)
        {
            kullaniciEposta = eposta;
            ArayuzOlustur();
        }

        private void ArayuzOlustur()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            Label lbl = new Label()
            {
                Text = "🎮 Sinema Etkinlikleri Burada Listelenecek",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            // Bilet Al butonu
            Guna2Button btnBiletAl = new Guna2Button()
            {
                Text = "Bilet Al",
                Size = new Size(160, 45),
                Location = new Point(30, 90),
                FillColor = Color.FromArgb(40, 120, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 8
            };
            btnBiletAl.Click += (s, e) =>
            {
                BiletAlForm form = new BiletAlForm("Sinema", kullaniciEposta);
                form.ShowDialog();
            };

            this.Controls.Add(btnBiletAl);
        }
    }
}
