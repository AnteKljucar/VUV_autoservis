using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Dijagnostika : Usluga, lObracunljivo
    {
        private const int cijena = 50;

        public Dijagnostika(string naziv)
            : base(naziv)
        {
            
        }
        public override int IzracunajCijenu()
        {
            return cijena;
        }
    }
}
