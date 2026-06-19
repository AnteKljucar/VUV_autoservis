using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Mehanicar : Osoba
    {
        public string IDMehanicar { get;  set; }
        public bool Izbrisan { get;  set; }

        public Mehanicar(string idMehanicar, string ime, string prezime, DateTime datumRodjenja, string oib, bool izbrisan)
            : base( ime, prezime, datumRodjenja, oib)
        {
            IDMehanicar = idMehanicar;
            Izbrisan = izbrisan;
        }
    }
}
