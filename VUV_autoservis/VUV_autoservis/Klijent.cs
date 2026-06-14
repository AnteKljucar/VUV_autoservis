using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Klijent : Osoba
    {
        public string IDKlijenta { get; private set; }

        public Klijent(string idKlijenta, string idOsoba, string ime, string prezime, DateTime datumRodjenja, string oib)
            : base(idOsoba, ime, prezime, datumRodjenja, oib)
        {
            IDKlijenta = idKlijenta;
        }
    }
}
