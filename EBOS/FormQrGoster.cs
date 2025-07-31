using System;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace EBOS
{
    public partial class FormQrGoster : Form
    {
        private PictureBox picQr;

        public FormQrGoster(string etkinlikAdi, string koltuk)
        {
            this.Text = "Bilet QR Kodu";
            this.Size = new Size(320, 370);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // QR Kod resmi
            picQr = new PictureBox()
            {
                Size = new Size(250, 250),
                Location = new Point(25, 20),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(picQr);

            // Kapat butonu
            var btnKapat = new Button()
            {
                Text = "Kapat",
                Width = 100,
                Height = 35,
                Location = new Point(100, 290),
                BackColor = Color.FromArgb(90, 115, 47),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnKapat.Click += (s, e) => this.Close();
            this.Controls.Add(btnKapat);

            string veri = $"Etkinlik: {etkinlikAdi}\nKoltuk: {koltuk}";
            QrCodeUret(veri);
        }

        private void QrCodeUret(string veri)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData qrData = qrGen.CreateQrCode(veri, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            picQr.Image = qrCode.GetGraphic(20);
        }
    }
}
