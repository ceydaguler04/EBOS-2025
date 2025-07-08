using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace EBOS.Entities
{
    public class Etkinlik
    {
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

