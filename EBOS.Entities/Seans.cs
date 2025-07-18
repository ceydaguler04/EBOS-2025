using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EBOS.Entities
{
    public class Seans
    {
        public int SeansID { get; set; }
        public string SeansAdi { get; set; }
        public int EtkinlikID { get; set; }
        public int SalonID { get; set; }

        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }

        // İlişkiler
        public Etkinlik Etkinlik { get; set; } 
        public Salon Salon { get; set; }

        public ICollection<Bilet> Biletler { get; set; }
    }
}
