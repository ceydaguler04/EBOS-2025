using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using Guna.UI2.WinForms;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace EBOS
{
    public partial class KullanicilarKontrol : UserControl
    {
        private Guna2DataGridView dgvKullanicilar;
        private TextBox txtArama;
        private Button btnYeniKullanici, btnSil, btnExcelAktar;
        private Label lblSayac;

        public KullanicilarKontrol()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // Arama kutusu
            txtArama = new TextBox();
            txtArama.PlaceholderText = "Ad, Soyad veya E-posta ara...";
            txtArama.Dock = DockStyle.Top;
            txtArama.Font = new Font("Segoe UI", 10);
            txtArama.TextChanged += TxtArama_TextChanged;

            // DataGridView
            dgvKullanicilar = new Guna2DataGridView();
            dgvKullanicilar.Dock = DockStyle.Fill;
            dgvKullanicilar.ReadOnly = true;
            dgvKullanicilar.AllowUserToAddRows = false;
            dgvKullanicilar.AllowUserToDeleteRows = false;
            dgvKullanicilar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKullanicilar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKullanicilar.EnableHeadersVisualStyles = false;
            dgvKullanicilar.BackgroundColor = Color.White;

            dgvKullanicilar.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvKullanicilar.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            dgvKullanicilar.ThemeStyle.RowsStyle.BackColor = Color.WhiteSmoke;
            dgvKullanicilar.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(203, 228, 222);

            // Excel'e Aktar butonu
            btnExcelAktar = new Button();
            btnExcelAktar.Text = "Excel'e Aktar";
            btnExcelAktar.BackColor = Color.SteelBlue;
            btnExcelAktar.ForeColor = Color.White;
            btnExcelAktar.Dock = DockStyle.Bottom;
            btnExcelAktar.Height = 40;
            btnExcelAktar.Click += BtnExcelAktar_Click;

            // Yeni kullanıcı butonu
            btnYeniKullanici = new Button();
            btnYeniKullanici.Text = "Yeni Kullanıcı Ekle";
            btnYeniKullanici.BackColor = Color.DarkSeaGreen;
            btnYeniKullanici.ForeColor = Color.White;
            btnYeniKullanici.Dock = DockStyle.Bottom;
            btnYeniKullanici.Height = 40;

            // Sil butonu
            btnSil = new Button();
            btnSil.Text = "Seçili Kullanıcıyı Sil";
            btnSil.BackColor = Color.IndianRed;
            btnSil.ForeColor = Color.White;
            btnSil.Dock = DockStyle.Bottom;
            btnSil.Height = 40;
            btnSil.Click += BtnSil_Click;

            // Sayaç etiketi
            lblSayac = new Label();
            lblSayac.Text = "";
            lblSayac.Dock = DockStyle.Bottom;
            lblSayac.TextAlign = ContentAlignment.MiddleLeft;
            lblSayac.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblSayac.Height = 30;

            // Kontrolleri sırayla ekle
            this.Controls.Add(dgvKullanicilar);
            this.Controls.Add(txtArama);
            this.Controls.Add(lblSayac);
            this.Controls.Add(btnExcelAktar);
            this.Controls.Add(btnYeniKullanici);
            this.Controls.Add(btnSil);

            this.Load += KullanicilarKontrol_Load;
        }

        private void KullanicilarKontrol_Load(object? sender, EventArgs e)
        {
            KullaniciListesiniYukle();
        }

        private void KullaniciListesiniYukle(string? filtre = null)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var liste = context.Kullanicilar
                        .Select(k => new
                        {
                            k.KullaniciID,
                            k.AdSoyad,
                            k.Eposta,
                            k.Rol,
                            YorumSayisi = k.Degerlendirmeler.Count,
                            KatildigiEtkinlikSayisi = k.Biletler
                                .Select(b => b.Seans.EtkinlikID)
                                .Distinct()
                                .Count()
                        })
                        .Where(x => filtre == null ||
                                    x.AdSoyad.ToLower().Contains(filtre) ||
                                    x.Eposta.ToLower().Contains(filtre))
                        .ToList();

                    dgvKullanicilar.DataSource = liste;
                    BasliklariRenklendir();
                    RolRenkleriUygula();
                    SayacGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message);
            }
        }

        private void TxtArama_TextChanged(object sender, EventArgs e)
        {
            string arama = txtArama.Text.ToLower();
            KullaniciListesiniYukle(arama);
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dgvKullanicilar.SelectedRows.Count > 0)
            {
                int id = (int)dgvKullanicilar.SelectedRows[0].Cells["KullaniciID"].Value;
                using (var context = new AppDbContext())
                {
                    var silinecek = context.Kullanicilar.Find(id);
                    if (silinecek != null)
                    {
                        context.Kullanicilar.Remove(silinecek);
                        context.SaveChanges();
                        KullaniciListesiniYukle();
                    }
                }
            }
        }

        private void BtnExcelAktar_Click(object sender, EventArgs e)
        {
            if (dgvKullanicilar.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Dosyası|*.xlsx";
                sfd.Title = "Excel'e Aktar";
                sfd.FileName = "Kullanicilar_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var wb = new XLWorkbook())
                        {
                            DataTable dt = new DataTable();

                            foreach (DataGridViewColumn col in dgvKullanicilar.Columns)
                                dt.Columns.Add(col.HeaderText);

                            foreach (DataGridViewRow row in dgvKullanicilar.Rows)
                            {
                                dt.Rows.Add(row.Cells.Cast<DataGridViewCell>()
                                    .Select(c => c.Value?.ToString()).ToArray());
                            }

                            wb.Worksheets.Add(dt, "Kullanicilar");
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Excel dosyası başarıyla oluşturuldu.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Excel aktarım hatası:\n" + ex.Message);
                    }
                }
            }
        }

        private void BasliklariRenklendir()
        {
            Color[] renkler = {
                Color.FromArgb(255, 179, 186), // KullaniciID
                Color.FromArgb(255, 223, 186), // AdSoyad
                Color.FromArgb(255, 255, 186), // Eposta
                Color.FromArgb(186, 255, 201), // Rol
                Color.LightSkyBlue,            // YorumSayisi
                Color.LightCyan                // KatildigiEtkinlikSayisi
            };

            for (int i = 0; i < dgvKullanicilar.Columns.Count && i < renkler.Length; i++)
            {
                dgvKullanicilar.Columns[i].HeaderCell.Style.BackColor = renkler[i];
            }
        }

        private void RolRenkleriUygula()
        {
            foreach (DataGridViewRow row in dgvKullanicilar.Rows)
            {
                if (row.Cells["Rol"].Value?.ToString() == "Yönetici")
                    row.DefaultCellStyle.BackColor = Color.LightSalmon;
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void SayacGuncelle()
        {
            int toplam = dgvKullanicilar.Rows.Count;
            int yonetici = dgvKullanicilar.Rows.Cast<DataGridViewRow>()
                .Count(r => r.Cells["Rol"].Value?.ToString() == "Yönetici");

            lblSayac.Text = $"Toplam Kullanıcı: {toplam} | Yönetici: {yonetici}";
        }
    }
}

//using System;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using EBOS.DataAccess;
//using Guna.UI2.WinForms;

//namespace EBOS
//{
//    public partial class KullanicilarKontrol : UserControl
//    {
//        private Guna2DataGridView dgvKullanicilar;
//        private TextBox txtArama;
//        private Button btnYeniKullanici, btnSil;
//        private Label lblSayac;
//        private Button btnExcelAktar;

//        public KullanicilarKontrol()
//        {
//            this.Dock = DockStyle.Fill;
//            this.BackColor = Color.White;

//            // Arama kutusu
//            txtArama = new TextBox();
//            txtArama.PlaceholderText = "Ad, Soyad veya E-posta ara...";
//            txtArama.Dock = DockStyle.Top;
//            txtArama.Font = new Font("Segoe UI", 10);
//            txtArama.TextChanged += TxtArama_TextChanged;

//            // DataGridView
//            dgvKullanicilar = new Guna2DataGridView();
//            dgvKullanicilar.Dock = DockStyle.Fill;
//            dgvKullanicilar.ReadOnly = true;
//            dgvKullanicilar.AllowUserToAddRows = false;
//            dgvKullanicilar.AllowUserToDeleteRows = false;
//            dgvKullanicilar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvKullanicilar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgvKullanicilar.EnableHeadersVisualStyles = false;
//            dgvKullanicilar.BackgroundColor = Color.White;

//            dgvKullanicilar.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//            dgvKullanicilar.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
//            dgvKullanicilar.ThemeStyle.RowsStyle.BackColor = Color.WhiteSmoke;
//            dgvKullanicilar.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(203, 228, 222);

//            // Sil butonu
//            btnSil = new Button();
//            btnSil.Text = "Seçili Kullanıcıyı Sil";
//            btnSil.BackColor = Color.IndianRed;
//            btnSil.ForeColor = Color.White;
//            btnSil.Dock = DockStyle.Bottom;
//            btnSil.Height = 40;
//            btnSil.Click += BtnSil_Click;

//            // Yeni kullanıcı butonu
//            btnYeniKullanici = new Button();
//            btnYeniKullanici.Text = "Yeni Kullanıcı Ekle";
//            btnYeniKullanici.BackColor = Color.DarkSeaGreen;
//            btnYeniKullanici.ForeColor = Color.White;
//            btnYeniKullanici.Dock = DockStyle.Bottom;
//            btnYeniKullanici.Height = 40;

//            // Sayaç etiketi
//            lblSayac = new Label();
//            lblSayac.Text = "";
//            lblSayac.Dock = DockStyle.Bottom;
//            lblSayac.TextAlign = ContentAlignment.MiddleLeft;
//            lblSayac.Font = new Font("Segoe UI", 9, FontStyle.Italic);
//            lblSayac.Height = 30;

//            // Kontrolleri sırayla ekle
//            this.Controls.Add(dgvKullanicilar);
//            this.Controls.Add(txtArama);
//            this.Controls.Add(lblSayac);
//            this.Controls.Add(btnYeniKullanici);
//            this.Controls.Add(btnSil);

//            this.Load += KullanicilarKontrol_Load;
//        }

//        private void KullanicilarKontrol_Load(object? sender, EventArgs e)
//        {
//            KullaniciListesiniYukle();
//        }

//        private void KullaniciListesiniYukle(string? filtre = null)
//        {
//            try
//            {
//                using (var context = new AppDbContext())
//                {
//                    var liste = context.Kullanicilar
//                        .Select(k => new
//                        {
//                            k.KullaniciID,
//                            k.AdSoyad,
//                            k.Eposta,
//                            k.Rol,
//                            YorumSayisi = k.Degerlendirmeler.Count,
//                            KatildigiEtkinlikSayisi = k.Biletler
//                                .Select(b => b.Seans.EtkinlikID)
//                                .Distinct()
//                                .Count()
//                        })
//                        .Where(x => filtre == null ||
//                                    x.AdSoyad.ToLower().Contains(filtre) ||
//                                    x.Eposta.ToLower().Contains(filtre))
//                        .ToList();

//                    dgvKullanicilar.DataSource = liste;
//                    BasliklariRenklendir();
//                    RolRenkleriUygula();
//                    SayacGuncelle();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Listeleme hatası: " + ex.Message);
//            }
//        }

//        private void TxtArama_TextChanged(object sender, EventArgs e)
//        {
//            string arama = txtArama.Text.ToLower();
//            KullaniciListesiniYukle(arama);
//        }

//        private void BtnSil_Click(object sender, EventArgs e)
//        {
//            if (dgvKullanicilar.SelectedRows.Count > 0)
//            {
//                int id = (int)dgvKullanicilar.SelectedRows[0].Cells["KullaniciID"].Value;
//                using (var context = new AppDbContext())
//                {
//                    var silinecek = context.Kullanicilar.Find(id);
//                    if (silinecek != null)
//                    {
//                        context.Kullanicilar.Remove(silinecek);
//                        context.SaveChanges();
//                        KullaniciListesiniYukle();
//                    }
//                }
//            }
//        }

//        private void BasliklariRenklendir()
//        {
//            Color[] renkler = {
//                Color.FromArgb(255, 179, 186), // KullaniciID
//                Color.FromArgb(255, 223, 186), // AdSoyad
//                Color.FromArgb(255, 255, 186), // Eposta
//                Color.FromArgb(186, 255, 201), // Rol
//                Color.LightSkyBlue,            // YorumSayisi
//                Color.LightCyan                // KatildigiEtkinlikSayisi
//            };

//            for (int i = 0; i < dgvKullanicilar.Columns.Count && i < renkler.Length; i++)
//            {
//                dgvKullanicilar.Columns[i].HeaderCell.Style.BackColor = renkler[i];
//            }
//        }

//        private void RolRenkleriUygula()
//        {
//            foreach (DataGridViewRow row in dgvKullanicilar.Rows)
//            {
//                if (row.Cells["Rol"].Value?.ToString() == "Yönetici")
//                    row.DefaultCellStyle.BackColor = Color.LightSalmon;
//                else
//                    row.DefaultCellStyle.BackColor = Color.White;
//            }
//        }

//        private void SayacGuncelle()
//        {
//            int toplam = dgvKullanicilar.Rows.Count;
//            int yonetici = dgvKullanicilar.Rows.Cast<DataGridViewRow>()
//                .Count(r => r.Cells["Rol"].Value?.ToString() == "Yönetici");

//            lblSayac.Text = $"Toplam Kullanıcı: {toplam} | Yönetici: {yonetici}";
//        }
//    }
//}