using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class ZamjenaDijela : Usluga, lObracunljivo
    {
        private int cijenaDijela;
        private const int cijenaUsluge = 15;

        public ZamjenaDijela(string id, string naziv, int cijenaDijela)
            : base(id, naziv)
        {
            this.cijenaDijela = cijenaDijela;
        }

        public override int IzracunajCijenu()
        {
            return cijenaDijela + cijenaUsluge;
        }

        public void Ispis()
        {
            Console.WriteLine("ID: " + IDUsluga);
            Console.WriteLine("Naziv: " + Naziv);
            Console.WriteLine("Cijena zamjene: " + IzracunajCijenu() + " €");
        }
    }
}
