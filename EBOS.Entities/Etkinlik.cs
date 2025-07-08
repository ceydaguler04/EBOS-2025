using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
>>>>>>> ba5605a (GirisForm ve KayitForm kodla tasarlandı)

namespace EBOS.Entities
{
    public class Etkinlik
    {
<<<<<<< HEAD
        [Key]
        public int EtkinlikID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EtkinlikAdi { get; set; }

        [MaxLength(500)]
        public string Aciklama { get; set; }

        [Required]
        public int TurID { get; set; } // Foreign Key alanı

        [ForeignKey("TurID")]
        public EtkinlikTuru EtkinlikTuru { get; set; } // Navigation: Etkinlik bir türe ait

        [MaxLength(200)]
        public string GorselYolu { get; set; } // Etkinliğin afiş görseli dosya yolu

        [Range(1, 500)]
        public int SureDakika { get; set; } // Süre (dakika cinsinden)

        // Navigation Properties
        public ICollection<Seans> Seanslar { get; set; } // 1 Etkinlik → Çok Seans
        public ICollection<Degerlendirme> Degerlendirmeler { get; set; } // 1 Etkinlik → Çok Yorum
    }
}
=======
        public int EtkinlikID { get; set; }
        public string EtkinlikAdi { get; set; }
        public string Aciklama { get; set; }
        public int TurID { get; set; }
        public string GorselYolu { get; set; } // Görsel dosya yolu
        public int SureDakika { get; set; }

        // İlişkiler
        public EtkinlikTuru EtkinlikTuru { get; set; }
        public ICollection<Seans> Seanslar { get; set; }
        public ICollection<Degerlendirme> Degerlendirmeler { get; set; }
    }
}

>>>>>>> ba5605a (GirisForm ve KayitForm kodla tasarlandı)
