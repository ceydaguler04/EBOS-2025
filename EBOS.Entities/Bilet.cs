using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace EBOS.Entities
{
    public class Bilet
    {
        public int BiletID { get; set; }

        public int KullaniciID { get; set; }
        public int SeansID { get; set; }
        public int KoltukID { get; set; }

        public decimal Fiyat { get; set; }
        public bool KampanyaUygulandiMi { get; set; }
        public DateTime SatinAlmaTarihi { get; set; }

        // İlişkiler
        public Kullanici Kullanici { get; set; }
        public Seans Seans { get; set; }
        public Koltuk Koltuk { get; set; }

    }
}
