using System;

namespace EBOS.Entities
{
    public class Yorum
    {
        public int YorumID { get; set; }
        public string EtkinlikAdi { get; set; }
        public int KullaniciID { get; set; }
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }

        public Kullanici Kullanici { get; set; }
    }
}
