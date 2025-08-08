using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace EBOS.Entities
{
  

    public class Bilet
    {
        [Key]
        public int BiletID { get; set; }

        // Kullanici ilişkisi
        public int KullaniciID { get; set; }
        [ForeignKey(nameof(KullaniciID))]
        public Kullanici Kullanici { get; set; } = null!;

        // Seans ilişkisi
        public int? SeansID { get; set; }
        [ForeignKey(nameof(SeansID))]
        public Seans? Seans { get; set; } = null!;

        // Koltuk ilişkisi
        public int KoltukID { get; set; }
        [ForeignKey(nameof(KoltukID))]
        public Koltuk Koltuk { get; set; } = null!;

        // Diğer alanlar
        public decimal Fiyat { get; set; }
        public bool KampanyaUygulandiMi { get; set; }
        public DateTime SatinAlmaTarihi { get; set; }

        public bool AktifMi { get; set; } = true; // Varsayılan olarak aktif

    }
}
