using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EBOS
{
    public partial class TiyatroKontrol : UserControl
    {
        private string kullaniciEposta;

        public TiyatroKontrol(string eposta)
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
                Text = "🎭 Tiyatro Etkinlikleri Burada Listelenecek",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 30),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            Guna2Button btnBiletAl = new Guna2Button()
            {
                Text = "Bilet Al",
                Size = new Size(160, 45),
                Location = new Point(30, 90),
                FillColor = Color.FromArgb(100, 40, 120),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderRadius = 8
            };
            btnBiletAl.Click += (s, e) =>
            {
                BiletAlForm form = new BiletAlForm("Tiyatro", kullaniciEposta);

                form.ShowDialog();
            };

            this.Controls.Add(btnBiletAl);
        }
    }
}
