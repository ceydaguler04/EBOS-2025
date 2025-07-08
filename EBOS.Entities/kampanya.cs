using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace EBOS.Entities
{
    public class Kampanya
    {
        public int KampanyaID { get; set; }
        public string KampanyaAdi { get; set; }
        public string Aciklama { get; set; }
        public int IndirimYuzdesi { get; set; } // Örn: 10, 20

        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}

