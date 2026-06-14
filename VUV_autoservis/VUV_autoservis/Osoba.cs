using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    abstract class Osoba
    {
        public string IDOsoba { get; private set; }
        public string Ime { get; private set; }
        public string Prezime { get; private set; }
        public DateTime DatumRodjenja { get; private set; }
        public string OIB { get; private set; }

        public Osoba(string idOsoba, string ime, string prezime, DateTime datumRodjenja, string oib)
        {
            IDOsoba = idOsoba;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            OIB = oib;
        }
    }
}
