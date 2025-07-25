﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBOS.Entities
{
    public class EtkinlikTuru
    {
        [Key]
        public int TurID { get; set; }
        public string TurAdi { get; set; } // Örnek: Sinema, Tiyatro, Konser

        // İlişki
        public ICollection<Etkinlik> Etkinlikler { get; set; }
    }
}

