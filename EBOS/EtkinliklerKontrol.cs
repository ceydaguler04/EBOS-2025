using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EBOS.DataAccess;
using EBOS.Entities;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;

namespace EBOS
{
    public partial class EtkinliklerKontrol : UserControl
    {
        private int? aktifKullaniciId;
        private string rol;

        private Label lblBaslik;
        private Guna2TextBox txtArama;
        private Guna2Button btnYeniEkle;
        private Guna2Button btnApiVeriGetir;
        private FlowLayoutPanel flpKartlar;
        private Guna2Button btnSinemaVeriCek;

        private int _skip = 0;
        private const int _take = 24;
        private bool _tumVeriYuklendi = false;
        private List<Etkinlik> _tumEtkinlikler = new List<Etkinlik>();

        public EtkinliklerKontrol(int? kullaniciId = null, string rol = null)
        {
            InitializeComponent();
            aktifKullaniciId = kullaniciId;
            if (rol == null && kullaniciId != null)
            {
                using (var db = new AppDbContext())
                {
                    var kullanici = db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == kullaniciId);
                    if (kullanici != null)
                        rol = kullanici.Rol; 
                }
            }
            this.rol = rol.ToLower();

            this.Dock = DockStyle.Fill;

            ArayuzOlustur();
            this.Load += async (s, e) => await EtkinlikKartlariniOlustur();
        }

        private void ArayuzOlustur()
        {
            this.BackColor = TemaYonetici.AktifTema == "Koyu" ? Color.FromArgb(120, 120, 120) : Color.White;

            lblBaslik = new Label()
            {
                Text = "ETKİNLİKLER",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblBaslik);

            txtArama = new Guna2TextBox()
            {
                PlaceholderText = "Etkinlik adına göre ara",
                Size = new Size(320, 40),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                BorderRadius = 10
            };
            txtArama.TextChanged += (s, e) =>
            {
                _skip = 0;
                _tumVeriYuklendi = false;
                flpKartlar.Controls.Clear();
                EtkinlikKartlariniOlustur(txtArama.Text);
            };

            this.Controls.Add(txtArama);

            btnYeniEkle = new Guna2Button()
            {
                Text = "+ Yeni Etkinlik Ekle",
                Size = new Size(200, 40),
                Location = new Point(370, 80),
                BorderRadius = 10,
                FillColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnYeniEkle.Click += btnYeniEtkinlikEkle_Click;
            this.Controls.Add(btnYeniEkle);
            
            if (!string.IsNullOrEmpty(rol) && rol.ToLower() == "yönetici")
            {
                btnApiVeriGetir = new Guna2Button()
                {
                    Text = "API'den Getir",
                    Size = new Size(150, 40),
                    Location = new Point(580, 80), // Yeni Etkinlik Ekle'nin sağında olsun
                    BorderRadius = 10,
                    FillColor = Color.DarkOrange,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btnApiVeriGetir.Click += BtnApiVeriGetir_Click;
                this.Controls.Add(btnApiVeriGetir);
            }


            flpKartlar = new FlowLayoutPanel()
            {
                Location = new Point(5, 140),
                Size = new Size(this.Width - 60, this.Height - 180),
                AutoScroll = true,
                WrapContents = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(flpKartlar);
            flpKartlar.FlowDirection = FlowDirection.LeftToRight;
            flpKartlar.WrapContents = true;
        }
        private async Task EtkinlikKartlariniOlustur(string filtre = "")
        {
            // Skeleton göster
            for (int i = 0; i < 6; i++)
                flpKartlar.Controls.Add(OlusturSkeletonKart());

            await Task.Delay(3000); // Simülasyon amacıyla 1 sn bekle (gerçek yükleme yerine)
            flpKartlar.Controls.Clear(); // Skeletonları temizle

            using (var db = new AppDbContext())
            {
                var query = db.Etkinlikler
                    .Include(e => e.EtkinlikTuru)
                              .Include(e => e.Mekan)
                    .OrderByDescending(e => e.Tarih)
                              .AsQueryable();

                if ((rol == "organisator" || rol == "organizatör") && aktifKullaniciId != null)
                {
                    query = query.Where(e => e.KullaniciID == aktifKullaniciId.Value);
                    MessageBox.Show($"Organizatör filtresi uygulandı. KullaniciID: {aktifKullaniciId}");
                }


                if (!string.IsNullOrWhiteSpace(filtre))
                {
                    var temizFiltre = filtre.Trim().ToLower();
                    query = query.Where(e =>
                        EF.Functions.Like(e.EtkinlikAdi.ToLower(), $"%{temizFiltre}%"));

                }

                List<Etkinlik> getirilecekler;

                if (!string.IsNullOrWhiteSpace(filtre))
                {
                    // Arama varsa tüm kayıtları getir (pagination yok)
                    getirilecekler = query
                        .GroupBy(e => new { e.EtkinlikAdi, e.Tarih })
                        .Select(g => g.First())
                        .ToList();

                    _tumVeriYuklendi = true; // Arama yapıldıysa daha fazla veri butonunu gösterme
                }
                else
                {
                    // Normal pagination
                    getirilecekler = query
                        .GroupBy(e => new { e.EtkinlikAdi, e.Tarih })
                        .Select(g => g.First())
                        .Skip(_skip)
                        .Take(_take)
                        .ToList();

                    if (getirilecekler.Count < _take)
                        _tumVeriYuklendi = true;

                    if (getirilecekler.Count > 0)
                        _skip += _take;
                }

                foreach (var etkinlik in getirilecekler)
                {
                    Guna2Panel kart = new Guna2Panel();
                    kart.Size = new Size(240, 330);
                    kart.BorderRadius = 15;
                    kart.FillColor = Color.White;
                    kart.ShadowDecoration.Enabled = true;
                    kart.ShadowDecoration.Depth = 10;
                    kart.Margin = new Padding(8);

                    PictureBox pb = new PictureBox();
                    try { pb.Load(etkinlik.GorselYolu); } catch { pb.Image = null; }
                    pb.Size = new Size(230, 140);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Location = new Point(10, 10);
                    kart.Controls.Add(pb);

                    Label lblAd = new Label() { Text = etkinlik.EtkinlikAdi, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 170), AutoSize = true };
                    Label lblTur = new Label() { Text = $"Tür: {etkinlik.EtkinlikTuru.TurAdi} | Süre: {etkinlik.SureDakika} dk", Font = new Font("Segoe UI", 9), Location = new Point(10, 200), AutoSize = true };
                    Label lblTarih = new Label() { Text = $"📅 {etkinlik.Tarih:dd.MM.yyyy} ⏰ {etkinlik.Saat:hh\\:mm}", Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(10, 230), AutoSize = true };
                    Label lblKonum = new Label()
                    {
                        Text = $"📍 {etkinlik.Mekan?.Sehir}, {etkinlik.Mekan?.Ad}", // veritabanında varsa MekanAdi, yoksa sabit yazı test için
                        Font = new Font("Segoe UI", 9),
                        ForeColor = Color.Black,
                        Location = new Point(10, 255),
                        AutoSize = true,
                        Cursor = Cursors.Hand
                    };
                    // Gelecekte tıklama ile harita formu açmak için
                    lblKonum.Click += (s, e) =>
                    {
                        if (etkinlik.Mekan != null)
                        {
                            string adres = $"{etkinlik.Mekan.Ad}, {etkinlik.Mekan.Adres}, {etkinlik.Mekan.Semt}, {etkinlik.Mekan.Ilce}, {etkinlik.Mekan.Sehir}";
                            string encodedAdres = Uri.EscapeDataString(adres);
                            string url = $"https://www.google.com/maps/search/?api=1&query={encodedAdres}";

                            var form = new HaritaForm(url);
                            form.ShowDialog();
                    }
                       
                        else
                    {
                            MessageBox.Show("Bu etkinlik için konum bilgisi yok.");
                    }
                    };

                    Guna2Button btnDuzenle = new Guna2Button() { Text = "Düzenle", Size = new Size(100, 30), FillColor = Color.DodgerBlue, ForeColor = Color.White, Location = new Point(10, 285) };
                    btnDuzenle.Click += (s, e) => DuzenleEtkinlik(etkinlik);

                    Guna2Button btnSil = new Guna2Button() { Text = "Sil", Size = new Size(100, 30), FillColor = Color.Crimson, ForeColor = Color.White, Location = new Point(120, 285) };
                    btnSil.Click += (s, e) => SilEtkinlik(etkinlik);

                    kart.Controls.Add(lblAd);

                    Label lblTur = new Label();
                    lblTur.Text = $"Tür: {etkinlik.EtkinlikTuru.TurAdi} | Süre: {etkinlik.SureDakika} dk";
                    lblTur.Font = new Font("Segoe UI", 9);
                    lblTur.Location = new Point(10, 200);
                    lblTur.AutoSize = true;
                    kart.Controls.Add(lblTur);
                    kart.Controls.Add(lblKonum);
                    kart.Controls.Add(lblTarih);
                    kart.Controls.Add(btnDuzenle);
                    kart.Controls.Add(btnSil);

                    flpKartlar.Controls.Add(kart);
                }
                var eskiBtn = flpKartlar.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Text == "Daha Fazla");
                if (eskiBtn != null)
                    flpKartlar.Controls.Remove(eskiBtn);

                if (!_tumVeriYuklendi && string.IsNullOrWhiteSpace(filtre))
                {
                    Guna2Button yeniBtn = new Guna2Button()
                    {
                        Text = "Daha Fazla",
                        Size = new Size(180, 40),
                        FillColor = Color.FromArgb(0, 123, 255),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BorderRadius = 8,
                        Margin = new Padding(305, 20, 0, 20)
                    };
                    yeniBtn.Click += BtnDahaFazla_Click;
                    flpKartlar.Controls.Add(yeniBtn);
                }
            }
            }
        private Guna2Panel OlusturSkeletonKart()
        {
            Guna2Panel skeleton = new Guna2Panel();
            skeleton.Size = new Size(240, 330);
            skeleton.BorderRadius = 15;
            skeleton.FillColor = Color.FromArgb(220, 220, 220);
            skeleton.ShadowDecoration.Enabled = true;
            skeleton.ShadowDecoration.Depth = 5;
            skeleton.Margin = new Padding(8);

            Panel imagePlaceholder = new Panel() { BackColor = Color.Silver, Location = new Point(10, 10), Size = new Size(220, 130) };
            Panel line1 = new Panel() { BackColor = Color.Gray, Location = new Point(10, 160), Size = new Size(160, 20) };
            Panel line2 = new Panel() { BackColor = Color.Gray, Location = new Point(10, 190), Size = new Size(180, 15) };
            Panel line3 = new Panel() { BackColor = Color.Gray, Location = new Point(10, 215), Size = new Size(180, 15) };
            Panel btn1 = new Panel() { BackColor = Color.DarkGray, Location = new Point(10, 260), Size = new Size(100, 30) };
            Panel btn2 = new Panel() { BackColor = Color.DarkGray, Location = new Point(120, 260), Size = new Size(100, 30) };

            skeleton.Controls.Add(imagePlaceholder);
            skeleton.Controls.Add(line1);
            skeleton.Controls.Add(line2);
            skeleton.Controls.Add(line3);
            skeleton.Controls.Add(btn1);
            skeleton.Controls.Add(btn2);

            return skeleton;
        }

        private void btnYeniEtkinlikEkle_Click(object sender, EventArgs e)
        {
            var form = new EtkinlikEkleForm(aktifKullaniciId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _skip = 0;
                _tumVeriYuklendi = false;
                flpKartlar.Controls.Clear();
                EtkinlikKartlariniOlustur();
            }
        }
        private void DuzenleEtkinlik(Etkinlik etkinlik)
        {
            var form = new EtkinlikEkleForm(etkinlik, aktifKullaniciId);  // Forma Etkinlik gönderiyoruz

            if (form.ShowDialog() == DialogResult.OK)
            {
                _skip = 0;
                _tumVeriYuklendi = false;
                flpKartlar.Controls.Clear();
                EtkinlikKartlariniOlustur(txtArama.Text);
            }
        }

        private void SilEtkinlik(Etkinlik etkinlik)
        {
            var onay = MessageBox.Show($"{etkinlik.EtkinlikAdi} adlı etkinliği silmek istediğinize emin misiniz?",
                                        "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay == DialogResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var silinecek = db.Etkinlikler.FirstOrDefault(e => e.EtkinlikID == etkinlik.EtkinlikID);
                    if (silinecek != null)
                    {
                        db.Etkinlikler.Remove(silinecek);
                        db.SaveChanges();
                        MessageBox.Show("Etkinlik silindi.");

                        _skip = 0;
                        _tumVeriYuklendi = false;
                        flpKartlar.Controls.Clear();
                        EtkinlikKartlariniOlustur(txtArama.Text);
                    }
                }
            }
        }

        private async void BtnApiVeriGetir_Click(object sender, EventArgs e)
        {
            var eslesmeyenKategoriler = new HashSet<string>();
            var eslesmeyenMekanlar = new List<string>();

            var tumEtkinlikler = await Helpers.EtkinlikApiYardimcisi.TumEtkinlikleriGetirAsync();

            if (tumEtkinlikler.Count == 0)
            {
                MessageBox.Show("API'den hiç etkinlik verisi gelmedi.");
                return;
            }

            int skipTur = 0, skipMekan = 0, skipIlce = 0, eklendi = 0, zatenVardi = 0;

            var kategoriMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "klasik müzik", "konser" }, { "pop müzik", "konser" }, { "alternatif müzik", "konser" },
                { "parti & canlı müzik", "konser" }, { "caz müzik", "konser" },
                { "aşçılık ve mutfak", "workshop" },
                { "tiyatro ve gösteriler", "tiyatro" }, { "dans ve müzikal gösteriler", "konser" },
                { "çocuk", "seminer" }, { "çocuk gelişimi", "seminer" },
                { "diğer", "seminer" }, { "sanat", "seminer" },
                { "sinema", "sinema" }, { "konser", "konser" },
                { "workshop", "workshop" }, { "seminer", "seminer" }, { "tiyatro", "tiyatro" },
                { "rock müzik", "konser" },
                { "dünya müzik", "konser" },
                { "türk sanat - halk müziği", "konser" },
                { "çocuk tiyatrosu", "tiyatro" },
                { "turizm", "seminer" },
                { "eğitim - öğretim", "seminer" }

            };

            using (var db = new AppDbContext())
            {
                foreach (var item in tumEtkinlikler)
                {
                    if (!DateTime.TryParse(item.Start, out DateTime parsedTarih))
                        continue;
                    var gelenAd = item.EtkinlikAdi?.Trim().ToLower();
                    DateTime tarih = parsedTarih.Date;

                    if (string.IsNullOrWhiteSpace(gelenAd)) continue;

                    if (db.Etkinlikler.Any(x => x.EtkinlikAdi.ToLower() == gelenAd && x.Tarih == tarih && x.Saat == parsedTarih.TimeOfDay))
                    {
                        zatenVardi++;
                        continue;
                    }
                    var gelenKategori = (item.Category?.Name ?? "").Trim().ToLower();
                    kategoriMap.TryGetValue(gelenKategori, out string eslesenKategori);

                    if (string.IsNullOrWhiteSpace(eslesenKategori))
                    {
                        eslesmeyenKategoriler.Add(gelenKategori);
                        skipTur++;
                        continue;
                    }

                    var tur = db.EtkinlikTurleri.FirstOrDefault(t => t.TurAdi.Trim().ToLower() == eslesenKategori.ToLower());
                    if (tur == null) { skipTur++; continue; }
                    int turId = tur.TurID;

                    int venueId = item.Venue?.Id ?? 0;
                    if (venueId == 0) { skipMekan++; continue; }

                    var apiMekan = await Helpers.EtkinlikApiYardimcisi.MekanGetirAsync(venueId);
                    if (apiMekan == null)
                    {
                        eslesmeyenMekanlar.Add($"ID: {venueId} - Etkinlik: {item.EtkinlikAdi}");
                        skipMekan++;
                        continue;
                    }

                    var mekan = db.Mekanlar.FirstOrDefault(m => m.MekanApiId == apiMekan.Id);
                    if (mekan == null)
                    {
                        var yeniMekan = new Mekan
                        {
                            Ad = apiMekan.Name,
                            Adres = apiMekan.Address,
                            Sehir = apiMekan.City?.Name,
                            Ilce = apiMekan.District?.Name,
                            Semt = apiMekan.Neighborhood?.Name ?? "-",
                            Enlem = double.TryParse(apiMekan.Lat, out double lat) ? lat : null,
                            Boylam = double.TryParse(apiMekan.Lng, out double lng) ? lng : null,
                            MekanApiId = apiMekan.Id
                        };

                        db.Mekanlar.Add(yeniMekan);
                        db.SaveChanges();
                        mekan = yeniMekan;
                    }
                    int? mekanId = mekan?.MekanID;

                    int? ilceId = null;
                    var ilceAdi = apiMekan?.District?.Name;
                    if (!string.IsNullOrWhiteSpace(ilceAdi))
                    {
                        var ilce = db.Ilceler.FirstOrDefault(i => i.IlceAdi.ToLower() == ilceAdi.ToLower());
                        if (ilce != null)
                            ilceId = ilce.IlceID;
                        else
                            skipIlce++;
                    }
                    else skipIlce++;

                    int sureDakika = 120;
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Start) && !string.IsNullOrEmpty(item.End))
                        {
                            var baslangic = DateTime.Parse(item.Start);
                            var bitis = DateTime.Parse(item.End);
                            sureDakika = (int)(bitis - baslangic).TotalMinutes;
                        }
                    }
                    catch { }

                    db.Etkinlikler.Add(new Etkinlik
                    {
                        EtkinlikAdi = item.EtkinlikAdi,
                        Aciklama = item.Aciklama,
                        SureDakika = sureDakika,
                        TurID = turId,
                        Tarih = parsedTarih,
                        Saat = parsedTarih.TimeOfDay,
                        GorselYolu = item.GorselUrl ?? "",
                        IlceID = ilceId,
                        MekanID = mekanId,
                        KullaniciID = aktifKullaniciId ?? 2
                    });

                    eklendi++;
                }

                var kayit = db.SaveChanges();
                MessageBox.Show(
                    $"Kaydedilen: {eklendi}\n" +
                    $"SaveChanges dönen: {kayit}\n" +
                    $"Zaten vardı: {zatenVardi}\n" +
                    $"Tür bulunamadı: {skipTur}\n" +
                    $"Mekan bulunamadı: {skipMekan}\n" +
                    $"İlçe bulunamadı/boş: {skipIlce}"
                );

                if (eslesmeyenKategoriler.Count > 0)
                    MessageBox.Show("Eşleşmeyen Kategoriler:\n\n" + string.Join("\n", eslesmeyenKategoriler));

                if (eslesmeyenMekanlar.Count > 0)
                    MessageBox.Show("Eşleşmeyen Mekanlar:\n\n" + string.Join("\n", eslesmeyenMekanlar));
            }
            _skip = 0;
            _tumVeriYuklendi = false;
            flpKartlar.Controls.Clear();
            EtkinlikKartlariniOlustur();
        }
        private void BtnDahaFazla_Click(object sender, EventArgs e)
        {
            EtkinlikKartlariniOlustur(txtArama.Text);
        }

        private async void EtkinliklerKontrol_Load(object sender, EventArgs e)
        {
            if (TemaYonetici.AktifTema == "Koyu")
            {
                this.BackColor = Color.FromArgb(120, 120, 120);
            }
            else
            {
                this.BackColor = Color.White;
            }
            EtkinlikKartlariniOlustur();
        }
    }
}