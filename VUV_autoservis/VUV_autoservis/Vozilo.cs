using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Vozilo
    {
        public string IDVozila { get; private set;}
        public string RegistracijskaOznaka { get; private set; }
        public string Marka { get; private set; }
        public string IDKlijenta { get; private set; }

        public bool Izbrisan { get;  set; }

        public Vozilo(string idvozila, string registracijskaOznaka, string marka, string vlasnik, bool izbrisan)
        {
            IDVozila = idvozila;
            RegistracijskaOznaka = registracijskaOznaka;
            Marka = marka;
            IDKlijenta = vlasnik;
            Izbrisan = izbrisan;
        }
    }
}
