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
        public Klijent Vlasnik { get; private set; }

        public Vozilo(string idvozila, string registracijskaOznaka, string marka, Klijent vlasnik)
        {
            IDVozila = idvozila;
            RegistracijskaOznaka = registracijskaOznaka;
            Marka = marka;
            Vlasnik = vlasnik;
        }
    }
}
