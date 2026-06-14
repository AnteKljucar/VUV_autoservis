using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Mehanicar : Osoba
    {
        public string IDMehanicar { get; private set; }

        public Mehanicar(string idMehanicar, string idOsoba, string ime, string prezime, DateTime datumRodjenja, string oib)
            : base(idOsoba, ime, prezime, datumRodjenja, oib)
        {
            IDMehanicar = idMehanicar;
        }
    }
}
