using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QRCoder;
using EBOS.DataAccess;
using EBOS.Entities;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System.Collections.Generic;

namespace EBOS
{
    public partial class OdemeForm : Form
    {
        private readonly string _etkinlikAdi;
        private readonly string _kullaniciEposta;
        private readonly int _koltukID;

        private Guna2Panel anaPanel;
        private Label lblBaslik;
        private Guna2Button btnOdemeYap;

        public OdemeForm(string etkinlikAdi, string kullaniciEposta, int koltukID)
        {
            _etkinlikAdi = etkinlikAdi;
            _kullaniciEposta = kullaniciEposta;
            _koltukID = koltukID;

            InitializeComponent();
            this.Text = "İyzico Ödeme Simülasyonu";
            this.Size = new Size(420, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ArayuzOlustur();
        }

        private void ArayuzOlustur()
        {
            anaPanel = new Guna2Panel()
            {
                Dock = DockStyle.Fill,
                BorderRadius = 15,
                Padding = new Padding(20),
                FillColor = Color.FromArgb(90, 115, 47)
            };
            this.Controls.Add(anaPanel);

            lblBaslik = new Label()
            {
                Text = "Kart Bilgilerinizi Girin",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            anaPanel.Controls.Add(lblBaslik);

            var txtKartSahibi = new Guna2TextBox { Name = "txtKartSahibi", PlaceholderText = "Kart Sahibi Ad Soyad", Location = new Point(20, 60), Width = 360 };
            var txtKartNo = new Guna2TextBox { Name = "txtKartNo", PlaceholderText = "Kart Numarası", Location = new Point(20, 100), Width = 360 };
            var txtAy = new Guna2TextBox { Name = "txtAy", PlaceholderText = "Ay (MM)", Location = new Point(20, 140), Width = 110 };
            var txtYil = new Guna2TextBox { Name = "txtYil", PlaceholderText = "Yıl (YY)", Location = new Point(140, 140), Width = 110 };
            var txtCvc = new Guna2TextBox { Name = "txtCvc", PlaceholderText = "CVC", Location = new Point(260, 140), Width = 120, PasswordChar = '*' };

            var chk3DSecure = new Guna2CheckBox { Name = "chk3DSecure", Text = "3D Secure ile ödeme yap", Location = new Point(20, 180) };
            var txt3DKod = new Guna2TextBox { Name = "txt3DKod", PlaceholderText = "3D Kod (örnek: 123456)", Location = new Point(20, 210), Width = 200, Visible = false };

            chk3DSecure.CheckedChanged += (s, e) => txt3DKod.Visible = chk3DSecure.Checked;

            anaPanel.Controls.Add(txtKartSahibi);
            anaPanel.Controls.Add(txtKartNo);
            anaPanel.Controls.Add(txtAy);
            anaPanel.Controls.Add(txtYil);
            anaPanel.Controls.Add(txtCvc);
            anaPanel.Controls.Add(chk3DSecure);
            anaPanel.Controls.Add(txt3DKod);

            btnOdemeYap = new Guna2Button()
            {
                Text = "Ödemeyi Tamamla",
                Size = new Size(200, 45),
                Location = new Point(100, 270),
                FillColor = Color.FromArgb(40, 120, 80),
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };
            btnOdemeYap.Click += BtnOdemeYap_Click;
            anaPanel.Controls.Add(btnOdemeYap);
        }

        private async void BtnOdemeYap_Click(object sender, EventArgs e)
        {
            try
            {
                var txtKartSahibi = anaPanel.Controls.Find("txtKartSahibi", true).FirstOrDefault() as Guna2TextBox;
                var txtKartNo = anaPanel.Controls.Find("txtKartNo", true).FirstOrDefault() as Guna2TextBox;
                var txtAy = anaPanel.Controls.Find("txtAy", true).FirstOrDefault() as Guna2TextBox;
                var txtYil = anaPanel.Controls.Find("txtYil", true).FirstOrDefault() as Guna2TextBox;
                var txtCvc = anaPanel.Controls.Find("txtCvc", true).FirstOrDefault() as Guna2TextBox;
                var chk3DSecure = anaPanel.Controls.Find("chk3DSecure", true).FirstOrDefault() as Guna2CheckBox;
                var txt3DKod = anaPanel.Controls.Find("txt3DKod", true).FirstOrDefault() as Guna2TextBox;

                if (chk3DSecure.Checked && txt3DKod.Text.Trim() != "123456")
                {
                    MessageBox.Show("3D Secure doğrulama başarısız! Lütfen doğru kodu girin.");
                    return;
                }

                Options options = new Options
                {
                    ApiKey = "sandbox-8oPWcX6By4azhI8eI7Da0tqwarUnLWD7",
                    SecretKey = "6SiXc2csLGy7HR5YwevlN50n3ovbNUGe",
                    BaseUrl = "https://sandbox-api.iyzipay.com"
                };

                CreatePaymentRequest request = new CreatePaymentRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = Guid.NewGuid().ToString(),
                    Price = "100",
                    PaidPrice = "100",
                    Currency = Currency.TRY.ToString(),
                    Installment = 1,
                    BasketId = "B67832",
                    PaymentChannel = PaymentChannel.WEB.ToString(),
                    PaymentGroup = PaymentGroup.PRODUCT.ToString()
                };

                request.PaymentCard = new PaymentCard
                {
                    CardHolderName = txtKartSahibi.Text.Trim(),
                    CardNumber = txtKartNo.Text.Trim(),
                    ExpireMonth = txtAy.Text.Trim(),
                    ExpireYear = txtYil.Text.Trim(),
                    Cvc = txtCvc.Text.Trim(),
                    RegisterCard = 0
                };

                request.Buyer = new Buyer
                {
                    Id = "BY789",
                    Name = "Sibel",
                    Surname = "Yağmur",
                    GsmNumber = "+905350000000",
                    Email = _kullaniciEposta,
                    IdentityNumber = "11111111111",
                    RegistrationAddress = "Akasya Mah. 1. Cad. No:2",
                    Ip = "85.34.78.112",
                    City = "Düzce",
                    Country = "Turkey"
                };

                request.BasketItems = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = "BI101",
                        Name = _etkinlikAdi,
                        Category1 = "Etkinlik",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Price = "100"
                    }
                };

                // ✅ Gerekli Shipping & Billing address eklendi!
                request.ShippingAddress = new Address
                {
                    ContactName = "Sibel Yağmur",
                    City = "Düzce",
                    Country = "Türkiye",
                    Description = "Akasya Mah. 1. Cad. No:2",
                    ZipCode = "81620"
                };

                request.BillingAddress = new Address
                {
                    ContactName = "Sibel Yağmur",
                    City = "Düzce",
                    Country = "Türkiye",
                    Description = "Akasya Mah. 1. Cad. No:2",
                    ZipCode = "81620"
                };

                Iyzipay.Model.Payment payment = await Iyzipay.Model.Payment.Create(request, options);

                if (payment.Status == "success")
                {
                    using (var db = new AppDbContext())
                    {
                        var koltuk = db.Koltuklar.FirstOrDefault(k => k.KoltukID == _koltukID);
                        int seansID = db.Seanslar.FirstOrDefault(s => s.SalonID == koltuk.SalonID)?.SeansID ?? 0;
                        int kullaniciID = db.Kullanicilar.FirstOrDefault(k => k.Eposta == _kullaniciEposta)?.KullaniciID ?? 0;

                        var bilet = new Bilet
                        {
                            KullaniciID = kullaniciID,
                            SeansID = seansID,
                            KoltukID = _koltukID,
                            SatinAlmaTarihi = DateTime.Now,
                            KampanyaUygulandiMi = false,
                            Fiyat = 100
                        };

                        db.Biletler.Add(bilet);
                        db.SaveChanges();

                        QrVeEpostaGonder(_kullaniciEposta, $"Etkinlik: {_etkinlikAdi}\nKoltuk No: {_koltukID}\nTarih: {DateTime.Now.ToShortDateString()}");

                        MessageBox.Show("Ödeme başarılı ve biletiniz kaydedildi!");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show($"Ödeme başarısız: {payment.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void QrVeEpostaGonder(string eposta, string biletBilgi)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(biletBilgi, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrBitmap = qrCode.GetGraphic(20);

                string tempPath = Path.Combine(Path.GetTempPath(), "biletQR.png");
                qrBitmap.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("yagmurs841@gmail.com");
                mail.To.Add(eposta);
                mail.Subject = "EBOS - Bilet Bilginiz";
                mail.Body = $"Merhaba,\n\nBilet bilgileriniz aşağıdadır:\n{biletBilgi}";
                mail.Attachments.Add(new Attachment(tempPath));

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("yagmurs841@gmail.com", "ppmwyzkukiegdpwc");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderme hatası: " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

