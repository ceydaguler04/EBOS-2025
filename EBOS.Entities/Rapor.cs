using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace EBOS.Entities
{
    public class Rapor
    {
        public int RaporID { get; set; }
        public string RaporAdi { get; set; }        // Örnek: “En Çok Satılan Etkinlikler”
        public string DosyaYolu { get; set; }       // PDF/Excel yolu (isteğe bağlı)
        public DateTime OlusturmaTarihi { get; set; }
    }
}
