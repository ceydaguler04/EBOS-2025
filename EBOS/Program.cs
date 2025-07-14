<<<<<<< HEAD
=======
﻿using System;
using System.Windows.Forms;

>>>>>>> ba5605a (GirisForm ve KayitForm kodla tasarlandı)
namespace EBOS
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
<<<<<<< HEAD
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new OrganisatorPaneli("Ahmet Y�lmaz"));
        }
    }
}
=======
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1()); // ✅ İlk açılan form: GirisForm
        }
    }
}
>>>>>>> ba5605a (GirisForm ve KayitForm kodla tasarlandı)
