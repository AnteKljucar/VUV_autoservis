using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    abstract class Usluga : lObracunljivo
    {
        public string Naziv { get; set; }

        public Usluga(string naziv)
        {
            Naziv = naziv;
        }

        public abstract int IzracunajCijenu();
    }
}
