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
        [Key]
        public int EtkinlikID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EtkinlikAdi { get; set; } = null!;

        [MaxLength(500)]
        public string Aciklama { get; set; } = string.Empty;

        [Required]
        public int TurID { get; set; }

        [ForeignKey("TurID")]
        public EtkinlikTuru EtkinlikTuru { get; set; } = null!;  // Navigation property düzeltildi

        [MaxLength(200)]
        public string GorselYolu { get ; set; } = string.Empty;

        [Range(1, 500)]
        public int SureDakika { get; set; } // Süre (dakika cinsinden)

        // Navigation Properties
        public ICollection<Seans> Seanslar { get; set; } // 1 Etkinlik → Çok Seans
        public ICollection<Degerlendirme> Degerlendirmeler { get; set; } // 1 Etkinlik → Çok Yorum
    }
}