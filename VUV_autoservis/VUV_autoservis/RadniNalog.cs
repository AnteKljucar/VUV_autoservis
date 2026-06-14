using System;
using System.Collections.Generic;


namespace VUV_autoservis
{
    public enum StatusRadnogNaloga
    {
        Zaprimljen = 1,
        U_Radu = 2,
        Završen = 3,
        Preuzet = 4
    }
    class RadniNalog
    {
        public Vozilo Vozilo { get; private set; }
        public Mehanicar Mehanicar { get; private set; }
        public DateTime Datum { get; private set; }

        public List<string> ObavljeneUsluge { get; private set; }

        public decimal UkupnaCijena { get; private set; }

        private StatusRadnogNaloga status;

        public StatusRadnogNaloga Status
        {
            get { return status; }
            private set
            {
                if (value < status)
                    throw new InvalidOperationException("Status se ne može vratiti unazad.");

                status = value;
            }
        }

        public RadniNalog(Vozilo vozilo, Mehanicar mehanicar)
        {
            Vozilo = vozilo;
            Mehanicar = mehanicar;
            Datum = DateTime.Now;

            ObavljeneUsluge = new List<string>();
            UkupnaCijena = 0;
            status = StatusRadnogNaloga.Zaprimljen;
        }

        public void DodajUslugu(string usluga, decimal cijena)
        {
            ObavljeneUsluge.Add(usluga);
            UkupnaCijena += cijena;
        }

        public void PromijeniStatus(StatusRadnogNaloga noviStatus)
        {
            Status = noviStatus;
        }
    }
}
