using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_autoservis
{
    class Popravak : Usluga, lObracunljivo
    {
        private int brojSati;
        private const int cijenaPoSatu = 30;

        public Popravak(string id, string naziv, int brojSati)
            : base(id, naziv)
        {
            this.brojSati = brojSati;
        }

        public override int IzracunajCijenu()
        {
            return brojSati * cijenaPoSatu;
        }
    }
}
