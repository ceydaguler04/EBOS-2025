using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public partial class KodDogrulamaForm : Form
    {
        public string GirilenKod { get; private set; }
        public event EventHandler KodYenidenGonderildi;

        private TextBox txtKod;
        private Button btnOnayla;
        private Button btnYenidenGonder;
        private Label lblSure;
        private System.Windows.Forms.Timer geriSayimTimer;
        private DateTime kodBitisZamani;

        public KodDogrulamaForm()
        {
            this.Text = "Doğrulama Kodu";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(320, 230);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.White;

            Label lblAciklama = new Label
            {
                Text = "6 haneli doğrulama kodunu giriniz:",
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(lblAciklama);

            txtKod = new TextBox
            {
                Location = new Point(20, 50),
                Width = 260,
                Font = new Font("Segoe UI", 11),
                MaxLength = 6,
                TextAlign = HorizontalAlignment.Center
            };
            this.Controls.Add(txtKod);

            btnOnayla = new Button
            {
                Text = "Onayla",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(160, 90),
                Width = 120,
                Height = 35
            };
            btnOnayla.FlatAppearance.BorderSize = 0;
            btnOnayla.Click += BtnOnayla_Click;
            this.AcceptButton = btnOnayla;
            this.Controls.Add(btnOnayla);

            btnYenidenGonder = new Button
            {
                Text = "Yeniden Gönder",
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(160, 135),
                Width = 120,
                Height = 35,
                Enabled = false
            };
            btnYenidenGonder.FlatAppearance.BorderSize = 0;
            btnYenidenGonder.BackColor = Color.Gray;
            btnYenidenGonder.Click += BtnYenidenGonder_Click;
            this.Controls.Add(btnYenidenGonder);

            lblSure = new Label
            {
                Text = "Kalan süre:",
                AutoSize = true,
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkRed,
                Location = new Point(20, 100),
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            this.Controls.Add(lblSure);

            kodBitisZamani = DateTime.Now.AddMinutes(3);
            geriSayimTimer = new System.Windows.Forms.Timer();
            geriSayimTimer.Interval = 1000;
            geriSayimTimer.Tick += GeriSayimTimer_Tick;
            geriSayimTimer.Start();
        }

        private void GeriSayimTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan kalan = kodBitisZamani - DateTime.Now;

            if (kalan.TotalSeconds <= 0)
            {
                geriSayimTimer.Stop();
                btnYenidenGonder.Enabled = true;
                btnYenidenGonder.BackColor = Color.Orange;
                lblSure.Text = "Süre doldu!";
                return;
            }
            else
            {
                lblSure.Text = $"Kalan süre: {kalan.Minutes:D2}:{kalan.Seconds:D2}";
            }
        }

        private void BtnOnayla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKod.Text) || txtKod.Text.Length != 6)
            {
                MessageBox.Show("Lütfen 6 haneli bir kod giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DateTime.Now > kodBitisZamani)
            {
                MessageBox.Show("Kodun süresi dolmuş. Lütfen yeni kod alın.", "Süre Doldu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GirilenKod = txtKod.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnYenidenGonder_Click(object sender, EventArgs e)
        {
            kodBitisZamani = DateTime.Now.AddMinutes(3);
            btnYenidenGonder.Enabled = false;
            btnYenidenGonder.BackColor = Color.Gray;
            geriSayimTimer.Start();
            KodYenidenGonderildi?.Invoke(this, EventArgs.Empty);
        }
        private void KodDogrulamaForm_Load(object sender, EventArgs e)
        {

        }
    }
}