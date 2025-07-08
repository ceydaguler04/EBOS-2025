using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using System;

using System;
using System.ComponentModel.DataAnnotations;

namespace EBOS.Entities
{
    public class Degerlendirme
    {
        [Key]  // ← BURASI ÖNEMLİ
        public int YorumID { get; set; }

        public int KullaniciID { get; set; }
        public int EtkinlikID { get; set; }

        public int Puan { get; set; }           // 1-5 arası
        public string Yorum { get; set; }       // Yorum metni
        public DateTime Tarih { get; set; }

        // İlişkiler
        public Kullanici Kullanici { get; set; }
        public Etkinlik Etkinlik { get; set; }
    }
}


