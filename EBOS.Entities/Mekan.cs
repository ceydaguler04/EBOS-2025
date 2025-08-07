using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBOS.Entities
{
    public class Mekan
    {
        

        [Key]
        public int MekanID { get; set; }
        public int IlceID { get; set; }
        public int? MekanApiId { get; set; }  // ← API'den gelen ID

        [Required]
        [MaxLength(200)]
        public string Ad { get; set; } = null!;

        [MaxLength(300)]
        public string Adres { get; set; }

        [MaxLength(100)]
        public string Sehir { get; set; }

        [MaxLength(100)]
        public string Ilce { get; set; }

        [MaxLength(100)]
        public string Semt { get; set; }

        public double? Enlem { get; set; }  // Konum için
        public double? Boylam { get; set; }

        // Etkinliklerle bire-çok ilişki
        public ICollection<Etkinlik> Etkinlikler { get; set; } = new List<Etkinlik>();
    }
}
