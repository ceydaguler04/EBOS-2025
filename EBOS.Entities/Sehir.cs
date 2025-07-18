using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EBOS.Entities
{
    public class Sehir
    {
        public int SehirID { get; set; }

        [Required, MaxLength(100)]
        public string SehirAdi { get; set; }

        public ICollection<Ilce> Ilceler { get; set; }
    }
}
