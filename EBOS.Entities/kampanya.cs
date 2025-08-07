using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EBOS.Entities
{
    public class Kampanya
    {
        public int KampanyaID { get; set; }               // Otomatik ID
        public string KampanyaAdi { get; set; }           // Örn: Yaz İndirimi
        public string Aciklama { get; set; }              // Açıklama
        public int IndirimYuzdesi { get; set; }           // Örn: 10, 20
        public DateTime BaslangicTarihi { get; set; }     // Başlangıç
        public DateTime BitisTarihi { get; set; }         // Bitiş

        public string KampanyaKodu { get; set; }          // Örn: EBOS10
        public decimal? MinTutar { get; set; }            // Minimum tutar şartı (opsiyonel)

        // 🔄 Kampanya şu an aktif mi? Hesaplamalı property
        public bool AktifMi
        {
            get
            {
                return DateTime.Now >= BaslangicTarihi && DateTime.Now <= BitisTarihi;
            }

        }
    }
}
