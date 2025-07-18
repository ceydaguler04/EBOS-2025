// EBOS.Entities/Etkinlik.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBOS.Entities
{
    public class Etkinlik

    {
        public int IlceID { get; set; }

        [ForeignKey("IlceID")]
        public Ilce Ilce { get; set; }

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
        public int SureDakika { get; set; }

        // İlişkili tablolar
        public ICollection<Seans> Seanslar { get; set; } = new List<Seans>();
        public ICollection<Degerlendirme> Degerlendirmeler { get; set; } = new List<Degerlendirme>();
    }
}

