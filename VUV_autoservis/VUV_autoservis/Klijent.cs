using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Klijent : Osoba
    {
        public string IDKlijenta { get; set; }
        public bool izbrisan { get; set; }

    public Klijent(string idKlijenta, string ime, string prezime, DateTime datumRodjenja, string oib, bool izbis)
            : base(ime, prezime, datumRodjenja, oib)
        {
            IDKlijenta = idKlijenta;
            izbrisan = izbis;

        }
    }
}
