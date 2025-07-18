using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBOS.Entities
{
    public class Ilce
    {
        public int IlceID { get; set; }

        [Required, MaxLength(100)]
        public string IlceAdi { get; set; }

        public int SehirID { get; set; }

        [ForeignKey("SehirID")]
        public Sehir Sehir { get; set; }

        public ICollection<Etkinlik> Etkinlikler { get; set; }
    }
}
