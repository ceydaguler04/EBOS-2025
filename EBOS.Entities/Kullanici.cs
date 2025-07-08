using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBOS.Entities
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Eposta { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Sifre { get; set; }
        public string Rol { get; set; } // "Yönetici" veya "Kullanıcı"

        // İlişkiler
        public ICollection<Bilet> Biletler { get; set; }
        public ICollection<Degerlendirme> Degerlendirmeler { get; set; }
    }
}

