using System;
using System.Collections.Generic;


namespace VUV_autoservis
{
    class RadniNalog
    {
        public string IDRadniNalog { get; set; }
        public string IDVozila { get; set; }
        public string IDMehanicara { get; set; }
        public string IDKlijenta { get; set; }
        public DateTime Datum { get; set; }
        public string Status { get; set; }
        public int Cijena { get; set; }
        public string Usluga { get; set; }


        public RadniNalog(string id, string idvozila, string idmehanicara, string idklijenta, DateTime datum, string status, int cijena, string usluga)
    
        {
            IDRadniNalog = id;
            IDVozila = idvozila;
            IDMehanicara = idmehanicara;
            IDKlijenta = idklijenta;
            Datum = datum;
            Status = status;
            Cijena = cijena;
            Usluga = usluga;
        }
    }
}
