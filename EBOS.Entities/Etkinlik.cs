using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Column(TypeName = "TEXT")]
        public string GorselYolu { get; set; } = string.Empty;

        [Range(1, 500)]
        public int SureDakika { get; set; } // Süre (dakika cinsinden)
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

        public ICollection<Seans> Seanslar { get; set; } // 1 Etkinlik → Çok Seans
        [ForeignKey("KullaniciID")]
        public Kullanici Kullanici { get; set; }

        public ICollection<Degerlendirme> Degerlendirmeler { get; set; } // 1 Etkinlik → Çok Yorum
    }
}