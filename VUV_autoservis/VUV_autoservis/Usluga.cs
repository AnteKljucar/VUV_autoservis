using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    abstract class Usluga : lObracunljivo
    {
        public string IDUsluga { get; private set; }
        public string Naziv { get; private set; }

        public Usluga(string id, string naziv)
        {
            IDUsluga = id;
            Naziv = naziv;
        }

        public abstract int IzracunajCijenu();
    }
}
