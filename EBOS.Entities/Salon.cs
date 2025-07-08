using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace EBOS.Entities
{
    public class Salon
    {
        public int SalonID { get; set; }
        public string SalonAdi { get; set; }
        public int SatirSayisi { get; set; }     // Kaç sıra var
        public int SutunSayisi { get; set; }     // Her sırada kaç koltuk var

        // İlişkiler
        public ICollection<Koltuk> Koltuklar { get; set; }
        public ICollection<Seans> Seanslar { get; set; }
    }
}

