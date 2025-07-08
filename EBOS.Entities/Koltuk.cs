using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBOS.Entities
{
    public class Koltuk
    {
        public int KoltukID { get; set; }
        public int SalonID { get; set; }
        public int Satir { get; set; }  // Örnek: 1, 2, 3...
        public int Sutun { get; set; }  // Örnek: 1, 2, 3...

        // İlişki
        public Salon Salon { get; set; }
        public ICollection<Bilet> Biletler { get; set; }
    }
}
