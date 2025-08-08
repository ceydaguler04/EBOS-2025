using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EBOS.Entities
{
    public class Etkinlik

    {
        public int KullaniciID { get; set; }

        [Key]
        public int EtkinlikID { get; set; }

        [Required]
        [MaxLength(200)]
        public string EtkinlikAdi { get; set; } = null!;

        [Column(TypeName = "TEXT")]
        public string Aciklama { get; set; } = string.Empty;

        [Required]
        public int TurID { get; set; }

        [ForeignKey("TurID")]
        public EtkinlikTuru EtkinlikTuru { get; set; } = null!;

        [Column(TypeName= "TEXT")]  // veya daha yüksek
        public string GorselYolu { get; set; }


        [Range(1, 500)]
        public int SureDakika { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public TimeSpan Saat { get; set; }

        public int? IlceID { get; set; }

        [ForeignKey("IlceID")]
        public Ilce? Ilce { get; set; }

        public int? MekanID { get; set; }

        [ForeignKey("MekanID")]
        public Mekan? Mekan { get; set; }

        public ICollection<Seans> Seanslar { get; set; } = new List<Seans>();

        [ForeignKey("KullaniciID")]
        public Kullanici Kullanici { get; set; } = null!;

        public ICollection<Degerlendirme> Degerlendirmeler { get; set; } = new List<Degerlendirme>();
    }

}
