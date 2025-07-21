using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EBOS
{
    public partial class SeanslarimKontrol : UserControl
    {
        private Guna2TextBox txtArama, txtSaat;
        private Guna2ComboBox cmbEtkinlik, cmbSalon;
        private Guna2DateTimePicker dtpTarih;
        private Guna2ToggleSwitch swFiltre;
        private Guna2DataGridView dgvSeanslar;
        private Guna2Button btnKaydet, btnTemizle;
        private Guna2CheckBox chkAktif;

        public SeanslarimKontrol()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // SOL PANEL 
            Panel solPanel = new Panel()
            {
                Location = new Point(5, 60),
                Size = new Size(525, 450),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(solPanel);

            // Başlık
            Label lblBaslik = new Label()
            {
                Text = "SEANSLARIM",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 5),
                AutoSize = true

            }; 
            this.Controls.Add(lblBaslik);

            //Arama kutusu
            txtArama = new Guna2TextBox()
            {
                PlaceholderText = "Etkinlik adına göre ara...",
                Location = new Point(8, 10),//(50, 100),
                Size = new Size(320, 36),
                BorderRadius = 8
            };
            this.Controls.Add(txtArama);
            solPanel.Controls.Add(txtArama);

            //// Durum filtreleme toggle
            //swFiltre = new Guna2ToggleSwitch()
            //{
            //    Location = new Point(370, 20),
            //    Checked = true
            //};
            //Label lblAktif = new Label() { Text = "Aktif", Location = new Point(400, 23), AutoSize = true };
            //Label lblPasif = new Label() { Text = "Pasif", Location = new Point(450, 23), AutoSize = true };

            //// solPanel içine eklenmeli
            //solPanel.Controls.Add(swFiltre);
            //solPanel.Controls.Add(lblAktif);
            //solPanel.Controls.Add(lblPasif);
            // Toggle + Aktif/Pasif etiketlerini taşıyacak panel
            Panel panelFiltre = new Panel()
            {
                Size = new Size(200, 30),
                Location = new Point(solPanel.Width - 200 - 10, 10), // Sağdan 10px boşluk bırak
                BackColor = Color.Transparent
            };

            // Toggle
            swFiltre = new Guna2ToggleSwitch()
            {
                Location = new Point(123, 10),
                Checked = true
            };

            // Etiketler
            Label lblAktif = new Label()
            {
                Text = "Aktif",
                Location = new Point(80, 10),
                AutoSize = true,
                ForeColor = Color.Black
            };

            Label lblPasif = new Label()
            {
                Text = "Pasif",
                Location = new Point(160, 10),
                AutoSize = true,
                ForeColor = Color.Black
            };

            // Ekleme
            panelFiltre.Controls.Add(swFiltre);
            panelFiltre.Controls.Add(lblAktif);
            panelFiltre.Controls.Add(lblPasif);
            solPanel.Controls.Add(panelFiltre);


            // DataGridView
            dgvSeanslar = new Guna2DataGridView()
            {
                Location = new Point(3, 50),//50, 150),
                Size = new Size(525, 400),//700, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvSeanslar.Columns.Add("SeansID", "Seans ID");
            dgvSeanslar.Columns.Add("EtkinlikAdi", "Etkinlik Adı");
            dgvSeanslar.Columns.Add("Tarih", "Tarih");
            dgvSeanslar.Columns.Add("Saat", "Saat");
            dgvSeanslar.Columns.Add("Salon", "Salon");
            dgvSeanslar.Columns.Add("Durum", "Durum");
            DataGridViewImageColumn colEdit = new DataGridViewImageColumn();
            colEdit.Image = new Bitmap(16, 16); // geçici ikon
            colEdit.HeaderText = "İşlem";
            dgvSeanslar.Columns.Add(colEdit);
            this.Controls.Add(dgvSeanslar);

            dgvSeanslar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            solPanel.Controls.Add(dgvSeanslar);
           

            // SAĞ PANEL
            Panel sagPanel = new Panel()
            {
                Location = new Point(540, 60),
                Size = new Size(300, 450),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(sagPanel);

            Label lblFormBaslik = new Label()
            {
                Text = "SEANS EKLE / GÜNCELLE",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 20),
                AutoSize = true
            };
            sagPanel.Controls.Add(lblFormBaslik);

            // Etkinlik
            Label lblEtkinlik = new Label() { Text = "Etkinlik Seç:", Location = new Point(10, 60), AutoSize = true };
            cmbEtkinlik = new Guna2ComboBox() { Location = new Point(10, 85), Size = new Size(280, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            sagPanel.Controls.Add(lblEtkinlik);
            sagPanel.Controls.Add(cmbEtkinlik);

            // Tarih
            Label lblTarih = new Label() { Text = "Tarih:", Location = new Point(10, 134), AutoSize = true };
            dtpTarih = new Guna2DateTimePicker()
            {
                Location = new Point(10, 160),
                Size = new Size(280, 30),
                Format = DateTimePickerFormat.Short,
                FillColor = Color.WhiteSmoke,
                BorderColor = Color.Silver
            };
            sagPanel.Controls.Add(lblTarih);
            sagPanel.Controls.Add(dtpTarih);

            // Saat
            Label lblSaat = new Label() { Text = "Saat:", Location = new Point(10, 206), AutoSize = true };
            //SAAT
            Guna2ComboBox cmbSaat = new Guna2ComboBox()
            {
                Location = new Point(10, 230),
                Size = new Size(80, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                MaxDropDownItems = 10,
                IntegralHeight = false,
                ItemHeight = 30,
                DropDownWidth = 95
            };

            for (int i = 0; i < 24; i++)
                cmbSaat.Items.Add(i.ToString("D2"));

            cmbSaat.SelectedIndex = 12;

            // DAKİKA 
            Guna2ComboBox cmbDakika = new Guna2ComboBox()
            {
                Location = new Point(100, 230),
                Size = new Size(80, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                MaxDropDownItems =10,
                IntegralHeight = false,
                ItemHeight = 30,
                DropDownWidth = 95

            };

            for (int i = 0; i < 60; i += 5)
                cmbDakika.Items.Add(i.ToString("D2"));

            cmbDakika.SelectedIndex = 0;

            // Eklemek
            sagPanel.Controls.Add(lblSaat);
            sagPanel.Controls.Add(cmbSaat);
            sagPanel.Controls.Add(cmbDakika);

            // Salon
            Label lblSalon = new Label() { Text = "Salon:", Location = new Point(10, 276), AutoSize = true };
            cmbSalon = new Guna2ComboBox() { Location = new Point(10, 300), Size = new Size(280, 30), DropDownStyle = ComboBoxStyle.DropDownList, Enabled = true };
            sagPanel.Controls.Add(lblSalon);
            sagPanel.Controls.Add(cmbSalon);

            // Durum (Aktif)
            chkAktif = new Guna2CheckBox() { Text = "Aktif", Location = new Point(10, 350), Checked = true };
            sagPanel.Controls.Add(chkAktif);

            // Butonlar
            btnKaydet = new Guna2Button() { Text = "Kaydet", Location = new Point(8, 395), Size = new Size(140, 36), FillColor = Color.SeaGreen, ForeColor = Color.White };
            btnTemizle = new Guna2Button() { Text = "Temizle", Location = new Point(155, 395), Size = new Size(140, 36), FillColor = Color.LightSteelBlue };
            btnTemizle.Click += (s, e) =>
            {
                cmbEtkinlik.SelectedIndex = -1;
                dtpTarih.Value = DateTime.Now;
                cmbSaat.SelectedIndex = -1;
                cmbDakika.SelectedIndex = -1;
                cmbSalon.SelectedIndex = -1;
                chkAktif.Checked = true;
            };

            sagPanel.Controls.Add(btnKaydet);
            sagPanel.Controls.Add(btnTemizle);
        }

        private void SeanslarimKontrol_Load(object sender, EventArgs e)
        {
            // sayfa yüklendiğinde yapılacak işlemler
        }
    }
}
