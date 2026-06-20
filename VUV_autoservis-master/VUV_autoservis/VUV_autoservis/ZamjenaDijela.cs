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

        public ZamjenaDijela( string naziv, int cijenaDijela)
            : base(naziv)
        {
            this.cijenaDijela = cijenaDijela;
        }

        public override int IzracunajCijenu()
        {
            return cijenaDijela + cijenaUsluge;
        }

    }
}
