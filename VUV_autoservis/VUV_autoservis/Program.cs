using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using ConsoleTableExt;



namespace VUV_autoservis
{
    class Program
    {
        static string putanjaKlijenti = @"C:\Users\AK\OneDrive\Dokumenti\VUV_autoservis\VUV_autoservis\Klijent.xml";
        static string putanjaVozila = @"C:\Users\AK\OneDrive\Dokumenti\VUV_autoservis\VUV_autoservis\Vozila.xml";
        static string putanjaMehanicar = @"C:\Users\AK\OneDrive\Dokumenti\VUV_autoservis\VUV_autoservis\Mehanicar.xml";
        static string putanjaRadniNalog = @"C:\Users\AK\OneDrive\Dokumenti\VUV_autoservis\VUV_autoservis\RadniNalog.xml";

        static List<Klijent> klijenti = new List<Klijent>();
        static List<Mehanicar> mehanicari = new List<Mehanicar>();
        static List<Vozilo> vozila = new List<Vozilo>();
        static List<RadniNalog> radniNalozi = new List<RadniNalog>();




        //Dodavanje Klijenta

        public static List<Klijent> dodavanjeKlijenata(List<Klijent> lKlijenta)
        {
            Console.Clear();
            Console.WriteLine("===== UNOS NOVOG KLIJENTA =====\n");

            string id = Convert.ToString("K" + (lKlijenta.Count + 1));
            bool izbris = false;

            Console.Write("Ime: ");
            string ime = Console.ReadLine();

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine();

            DateTime datumRodjenja;
            while (true)
            {
                Console.Write("Datum rođenja (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out datumRodjenja))
                    break;

                Console.WriteLine("Neispravan format datuma!");
            }

            string oib;
            while (true)
            {
                Console.Write("OIB: ");
                oib = Console.ReadLine();

                if (oib.Length == 11)
                    break;

                Console.WriteLine("OIB mora imati 11 znamenki!");
            }

            Klijent novi = new Klijent(
                id,
                ime,
                prezime,
                datumRodjenja,
                oib,
                izbris

            ); ;

            lKlijenta.Add(novi);
            Console.WriteLine("\nKlijent uspješno dodan!");
            return lKlijenta;




        }


        //Azuriranje klijenta


        public static List<Klijent> azuriranjeKlijenata(List<Klijent> lKlijenta)
        {
            ponovniOdabirKorisnika:
            Console.Clear();
            var tableDataKlijent = new List<List<object>>();
            var tableDataKlijentZaAzuriranje = new List<List<object>>();

            Klijent zaAzuriranje = null;

            foreach (Klijent k in lKlijenta)
            {
                if (k.izbrisan == false)
                {
                    tableDataKlijent.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataKlijent)
                .WithTitle("Klijenti")
                .WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID klijenta kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lKlijenta;
            }


            int brojObrisanih = 0;
            foreach (Klijent k in lKlijenta)
            {
                if (k.IDKlijenta == odabir)
                {
                    zaAzuriranje = k;
                    tableDataKlijentZaAzuriranje.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.Clear();

                ConsoleTableBuilder
        .From(tableDataKlijentZaAzuriranje)
        .WithTitle("Odabrani Klijent")
        .WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan")
        .ExportAndWriteLine();






                bool izbornikZaAzuriranjeKorisnika = true;
                while (izbornikZaAzuriranjeKorisnika)
                {
                    Console.WriteLine("Odaberi koji dio klijenta zelis azurirati.");
                    Console.WriteLine("--- 1. Ime");
                    Console.WriteLine("--- 2. Prezime");
                    Console.WriteLine("--- 3. Datum rođenja");
                    Console.WriteLine("--- 4. OIB");
                    Console.WriteLine("--- 5. Izbrisan");
                    Console.WriteLine("--- 6. Povratak");
                    odabir = Console.ReadLine();
                    int odabirProvjereno = 0;
                    if (!int.TryParse(odabir, out odabirProvjereno))
                    {
                        Console.WriteLine("Unos mora biti broj");
                        odabirProvjereno = 0;
                    }
                    switch (odabirProvjereno)
                    {
                        case 1:
                            Console.Write("Unesi novo ime: ");
                            zaAzuriranje.Ime = Console.ReadLine();
                            break;

                        case 2:
                            Console.Write("Unesi novo prezime: ");
                            zaAzuriranje.Prezime = Console.ReadLine();
                            break;

                        case 3:
                            Console.Write("Unesi datum rođenja (yyyy-MM-dd): ");
                            zaAzuriranje.DatumRodjenja = DateTime.Parse(Console.ReadLine());
                            break;

                        case 4:
                            Console.Write("Unesi OIB: ");
                            zaAzuriranje.OIB = Console.ReadLine();
                            break;

                        case 5:
                            Console.Write("Izbrisan (true/false): ");
                            zaAzuriranje.izbrisan = bool.Parse(Console.ReadLine());
                            break;

                        case 6:
                            goto ponovniOdabirKorisnika;

                        default:
                            Console.WriteLine("Nevažeći odabir.");
                            break;
                    }
                }


                int index = lKlijenta.FindIndex(k => k.IDKlijenta == zaAzuriranje.IDKlijenta);

                if (index != -1)
                {
                    lKlijenta[index] = zaAzuriranje;
                }

                return lKlijenta;
            }
            else
            {
                Console.WriteLine("Korisnik sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }

        }











        /* Klijent zaAzuriranje = null;

         foreach (Klijent k in lKlijenta)
         {
             if (k.izbrisan == false)
             {
                 tableDataKlijent.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
             }

         }

         ConsoleTableBuilder
             .From(tableDataKlijent)
             .WithTitle("Klijenti")
             .WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan")
             .ExportAndWriteLine();

     ponovniUnosID:
         Console.WriteLine("Unesi ID klijenta kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
         string odabir = Console.ReadLine();
         if (odabir.ToLower() == "povratak")
         {
             return lKlijenta;
         }
         int pronadjen = 0;
         foreach (Klijent k in lKlijenta)
         {
             if (k.IDKlijenta == odabir)
             {
                 zaAzuriranje = k;
                 tableDataKlijentZaAzuriranje.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                 pronadjen++;
             }
             else if(pronadjen == 0)
             {
                 Console.WriteLine("Korisnik sa traženim ID-em nije pronađen.");
                 goto ponovniUnosID;
             }*/
         
         

        





        //Brisanje Klijenta

        public static List<Klijent> brisanjeKlijenata(List<Klijent> lKlijenta)
        {
            Console.Clear();
            var tableDataKlijent = new List<List<object>>();
            foreach (Klijent k in lKlijenta)
            {
                if (k.izbrisan == false)
                {
                    tableDataKlijent.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataKlijent)
                .WithTitle("Klijenti")
                .WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID klijenta kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lKlijenta;
            }


            int brojObrisanih = 0;
            foreach (Klijent k in lKlijenta)
            {
                if (k.IDKlijenta == odabir)
                {
                    k.izbrisan = true;
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.WriteLine("Klijent uspješno obirsan.");
                return lKlijenta;
            }
            else
            {
                Console.WriteLine("Korisnik sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }

        }




        //Dodavanje vozila

        public static List<Vozilo> dodavanjeVozila(List<Vozilo> lVozila, List<Klijent> lKlijenta)
        {

            Console.Clear();
            Console.WriteLine("===== UNOS NOVOG KLIJENTA =====\n");

            string id = Convert.ToString("V" + (lVozila.Count + 1));

            Console.Write("Registracija: ");
            string registracija = Console.ReadLine();

            Console.Write("Marka: ");
            string Marka = Console.ReadLine();


            var tableDataVozila = new List<List<object>>();
            foreach (Klijent k in lKlijenta)
            {
                if (k.izbrisan == false)
                {
                    tableDataVozila.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataVozila)
                .WithTitle("Klijenti")
                .WithColumn("IDKlijenta", "Ime", "Prezime", "OIB", "Izbrisan")
                .ExportAndWriteLine();


        ponovniUnosID:
            Console.WriteLine("Unesi ID klijenta kojeg želiš izbrisati.");
            string odabir = Console.ReadLine();


            int brojObrisanih = 0;
            foreach (Klijent k in lKlijenta)
            {
                if (k.IDKlijenta == odabir)
                {
                    bool izbrisan = false;
                    Vozilo novo = new Vozilo(id, registracija, Marka, odabir, izbrisan);
                    lVozila.Add(novo);
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.WriteLine("\nVozilo uspješno dodan!");
                return lVozila;
            }
            else
            {
                Console.WriteLine("Korisnik sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }
        }



        //AzuriranjeVozila

        public static void azuriranVozila(List<Vozilo> lVozila)
        {
            Console.Clear();
            var tableDataKlijent = new List<List<object>>();

        }





        //Brisanje vozila


        public static List<Vozilo> brisanjeVozila(List<Vozilo> lVozila)
        {
            Console.Clear();


            var tableDataVozila = new List<List<object>>();
            foreach (Vozilo v in lVozila)
            {
                if (v.Izbrisan == false)
                {
                    tableDataVozila.Add(new List<object>() { v.IDVozila, v.RegistracijskaOznaka, v.Marka, v.IDKlijenta, v.Izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataVozila)
                .WithTitle("Vozila")
                .WithColumn("IDVozila", "Registracija", "Marka", "IDKlijenta", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID vozila kojeg želiš izbrisati. Ili upiši 'Povratak' za izlazak bez brisanja vozila.");
            string odabir = Console.ReadLine();

            if(odabir.ToLower() == "povratak")
            {
                return lVozila;
            }

            int brojObrisanih = 0;
            foreach (Vozilo v in lVozila)
            {
                if (v.IDVozila == odabir)
                {
                    v.Izbrisan = true;
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.WriteLine("Vozilo uspješno obirsan.");
                return lVozila;
            }
            else
            {
                Console.WriteLine("Vozilo sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }

        }



        //Dodavanje mehanicar

        public static List<Mehanicar> dodavanjeMehanicar(List<Mehanicar> lMehanicar)
        {
            Console.Clear();
            Console.WriteLine("===== UNOS NOVOG Mehanicara =====\n");

            string id = Convert.ToString("M" + (lMehanicar.Count + 1));
            bool izbris = false;

            Console.Write("Ime: ");
            string ime = Console.ReadLine();

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine();

            DateTime datumRodjenja;
            while (true)
            {
                Console.Write("Datum rođenja (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out datumRodjenja))
                    break;

                Console.WriteLine("Neispravan format datuma!");
            }

            string oib;
            while (true)
            {
                Console.Write("OIB: ");
                oib = Console.ReadLine();

                if (oib.Length == 11)
                    break;

                Console.WriteLine("OIB mora imati 11 znamenki!");
            }

            Mehanicar novi = new Mehanicar(
                id,
                ime,
                prezime,
                datumRodjenja,
                oib,
                izbris

            ); ;

            lMehanicar.Add(novi);
            Console.WriteLine("\nMehaničar uspješno dodan!");
            return lMehanicar;




        }


        //Azuriranje mehanicara


        public static void azuriranjeMehanicara(List<Mehanicar> lMehanicara)
        {
            Console.Clear();
            var tableDataKlijent = new List<List<object>>();

        }




        //Brisanje mehanicara

        public static List<Mehanicar> brisanjeMehanicara(List<Mehanicar> lMehanicara)
        {
            Console.Clear();
            var tableDataMehanicar = new List<List<object>>();
            foreach (Mehanicar m in lMehanicara)
            {
                if (m.Izbrisan == false)
                {
                    tableDataMehanicar.Add(new List<object>() { m.IDMehanicar, m.Ime, m.Prezime, m.DatumRodjenja, m.OIB, m.Izbrisan });
                }
            }

            ConsoleTableBuilder
                .From(tableDataMehanicar)
                .WithTitle("Mehanicar")
                .WithColumn("IDMehanicara", "Ime", "Prezime","Datum rodjenja", "OIB", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID mehanicara kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lMehanicara;
            }


            int brojObrisanih = 0;
            foreach (Mehanicar m in lMehanicara)
            {
                if (m.IDMehanicar == odabir)
                {
                    m.Izbrisan = true;
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.WriteLine("Klijent uspješno obirsan.");
                return lMehanicara;
            }
            else
            {
                Console.WriteLine("Korisnik sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }

        }





        //Radni nalozi
        /*public static List<RadniNalog> otvaranjeRadnogNaloga(List<RadniNalog> lRadnihNaloga)
        {

        }
        */







        static void Main(string[] args)
        {
            klijenti = UcitajKlijente();
            vozila = UcitajVozila();
            mehanicari = UcitajMehanicare();
            radniNalozi = UcitajRadniNalog();
            try
            {


                bool pocetniIzbornik = true;
                while (pocetniIzbornik)
                {

                    Console.Clear();
                    Console.WriteLine("--- Izbornik ---");
                    Console.WriteLine("--- 1. Upravljanje klijentima ---");
                    Console.WriteLine("--- 2. Upravljanje vozilima ---");
                    Console.WriteLine("--- 3. Upravljanje Mehaničarima ---");
                    Console.WriteLine("--- 4. Upravljanje nalozima ---");
                    Console.WriteLine("--- 5. Statistika ---");
                    Console.WriteLine("--- 6. Izlaz ---");

                    string odabir = Console.ReadLine();
                    int odabirProvjereno = 0;
                    if (!int.TryParse(odabir, out odabirProvjereno))
                    {
                        Console.WriteLine("OIB smije sadržavati samo brojeve.");
                        odabirProvjereno = 0;
                    }
                    switch (odabirProvjereno)
                    {
                        case 1:
                            bool izbornikKlijent = true;
                            while (izbornikKlijent)
                            {
                                Console.Clear();
                                Console.WriteLine("--- Izbornik ---");
                                Console.WriteLine("--- 1. Dodavanje Klijenata ---");
                                Console.WriteLine("--- 2. Ažuriranje Klijenata ---");
                                Console.WriteLine("--- 3. Brisanje Klijenata ---");
                                Console.WriteLine("--- 4. Povratak ---");

                                odabir = Console.ReadLine();
                                odabirProvjereno = 0;
                                if (!int.TryParse(odabir, out odabirProvjereno))
                                {
                                    Console.WriteLine("Odabir smora biti broj.");
                                    odabirProvjereno = 0;
                                }
                                switch (odabirProvjereno)
                                {
                                    case 1:
                                        klijenti = dodavanjeKlijenata(klijenti);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);

                                        break;
                                    case 2:

                                        klijenti = azuriranjeKlijenata(klijenti);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 3:
                                        klijenti = brisanjeKlijenata(klijenti);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 4:
                                        izbornikKlijent = false;
                                        break;
                                    default:
                                        Console.WriteLine("Uneseno nešto što nije opcija.");
                                        break;

                                }
                            }
                            break;
                        case 2:
                            bool izbornikVozila = true;

                            while (izbornikVozila)
                            {
                                Console.Clear();
                                Console.WriteLine("--- Izbornik ---");
                                Console.WriteLine("--- 1. Dodavanje Vozila ---");
                                Console.WriteLine("--- 2. Ažuriranje Vozila ---");
                                Console.WriteLine("--- 3. Brisanje Vozila ---");
                                Console.WriteLine("--- 4. Povratak ---");

                                odabir = Console.ReadLine();
                                odabirProvjereno = 0;
                                if (!int.TryParse(odabir, out odabirProvjereno))
                                {
                                    Console.WriteLine("Odabir smora biti broj.");
                                    odabirProvjereno = 0;
                                }
                                switch (odabirProvjereno)
                                {
                                    case 1:
                                        vozila = dodavanjeVozila(vozila, klijenti);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 2:
                                        //vozila = azuriranjeVozila(vozila);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 3:
                                        vozila = brisanjeVozila(vozila);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 4:
                                        izbornikVozila = false;
                                        break;
                                    default:
                                        Console.WriteLine("Uneseno nešto što nije opcija.");
                                        break;

                                }
                            }
                            break;
                        case 3:
                            bool izbornikMehanicar = true;

                            while (izbornikMehanicar)
                            {
                                Console.Clear();
                                Console.WriteLine("--- Izbornik ---");
                                Console.WriteLine("--- 1. Dodavanje Mehaničara ---");
                                Console.WriteLine("--- 2. Ažuriranje Mehaničara ---");
                                Console.WriteLine("--- 3. Brisanje Mehaničara ---");
                                Console.WriteLine("--- 4. Povratak ---");

                                odabir = Console.ReadLine();
                                odabirProvjereno = 0;
                                if (!int.TryParse(odabir, out odabirProvjereno))
                                {
                                    Console.WriteLine("Odabir smora biti broj.");
                                    odabirProvjereno = 0;
                                }
                                switch (odabirProvjereno)
                                {
                                    case 1:
                                        mehanicari = dodavanjeMehanicar(mehanicari);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 2:
                                        //mehanicar = azuriranjeMehanicar(mehanicar);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 3:
                                        mehanicari = brisanjeMehanicara(mehanicari);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 4:
                                        izbornikMehanicar = false;
                                        break;
                                    default:
                                        Console.WriteLine("Uneseno nešto što nije opcija.");
                                        break;

                                }
                            }
                            break;
                        case 4:
                            bool izbornikNaloga = true;

                            while (izbornikNaloga)
                            {
                                Console.Clear();
                                Console.WriteLine("--- Izbornik ---");
                                Console.WriteLine("--- 1. Otvaranje Radnog Naloga ---");
                                Console.WriteLine("--- 2. Promjena Statusa Naloga ---");
                                Console.WriteLine("--- 3. Pregled Naloga Vozila ---");
                                Console.WriteLine("--- 4. Povratak ---");

                                odabir = Console.ReadLine();
                                odabirProvjereno = 0;
                                if (!int.TryParse(odabir, out odabirProvjereno))
                                {
                                    Console.WriteLine("Odabir smora biti broj.");
                                    odabirProvjereno = 0;
                                }
                                switch (odabirProvjereno)
                                {
                                    case 1:
                                        //otvaranjeRadnogNaloga();
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 2:
                                        //promjenaStatusaRadnogNaloga();
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 3:
                                        //pregledNalogaVozila();
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 4:
                                        izbornikNaloga = false;
                                        break;
                                    default:
                                        Console.WriteLine("Uneseno nešto što nije opcija.");
                                        break;

                                }
                            }
                            break;
                        case 5:
                            bool izbornikStatistika = true;
                            while (izbornikStatistika)
                            {
                                Console.Clear();
                                Console.WriteLine("--- Statistika ---");
                                Console.WriteLine("--- 4. Povratak ---");


                                odabir = Console.ReadLine();
                                odabirProvjereno = 0;
                                if (!int.TryParse(odabir, out odabirProvjereno))
                                {
                                    Console.WriteLine("Odabir smora biti broj.");
                                    odabirProvjereno = 0;
                                }
                                switch (odabirProvjereno)
                                {
                                    case 1:
                                        break;
                                    case 2:
                                        break;
                                    case 3:
                                        break;
                                    case 4:
                                        izbornikStatistika = false;
                                        break;
                                    default:
                                        Console.WriteLine("Uneseno nešto što nije opcija.");
                                        break;

                                }
                            }


                            break;
                        case 6:
                            pocetniIzbornik = false;
                            break;
                        default:
                            Console.WriteLine("Odabrano nešto što nije opcija.");
                            break;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            SpremiKlijente(klijenti);
            SpremiVozila(vozila);
            SpremiMehanicar(mehanicari);
            SpremiRadniNalog(radniNalozi);
        }//Main



        static List<Vozilo> UcitajVozila()
        {
            List<Vozilo> lista = new List<Vozilo>();

            if (!File.Exists(putanjaVozila))
                return lista;

            XmlDocument doc = new XmlDocument();
            doc.Load(putanjaVozila);

            foreach (XmlNode node in doc.SelectNodes("//Vozilo"))
            {
                Vozilo v = new Vozilo(
                    node["ID"].InnerText,
                    node["Registracija"].InnerText,
                    node["Marka"].InnerText,
                    node["IDKlijenta"].InnerText,
                    bool.Parse(node["Izbrisan"].InnerText));

                lista.Add(v);
            }

            return lista;
        }

        static List<Klijent> UcitajKlijente()
        {
            List<Klijent> lista = new List<Klijent>();

            if (!File.Exists(putanjaKlijenti))
                return lista;

            XmlDocument doc = new XmlDocument();
            doc.Load(putanjaKlijenti);

            foreach (XmlNode node in doc.SelectNodes("//Klijent"))
            {
                Klijent k = new Klijent(
                    node["ID"].InnerText,
                    node["Ime"].InnerText,
                    node["Prezime"].InnerText,
                    DateTime.Parse(node["DatumRodjenja"].InnerText),
                    node["OIB"].InnerText,
                    bool.Parse(node["Izbrisan"].InnerText)
                );

                lista.Add(k);
            }

            return lista;
        }

        static List<Mehanicar> UcitajMehanicare()
        {
            List<Mehanicar> lista = new List<Mehanicar>();

            if (!File.Exists(putanjaMehanicar))
                return lista;

            XmlDocument doc = new XmlDocument();
            doc.Load(putanjaMehanicar);

            foreach (XmlNode node in doc.SelectNodes("//Mehanicar"))
            {
                Mehanicar m = new Mehanicar(
                    node["IDMehanicar"].InnerText,
                    node["Ime"].InnerText,
                    node["Prezime"].InnerText,
                    DateTime.Parse(node["DatumRodjenja"].InnerText),
                    node["OIB"].InnerText,
                    bool.Parse(node["Izbrisan"].InnerText)
                );

                lista.Add(m);
            }

            return lista;
        }


        static List<RadniNalog> UcitajRadniNalog()
        {
            List<RadniNalog> lista = new List<RadniNalog>();

            if (!File.Exists(putanjaRadniNalog))
                return lista;

            XmlDocument doc = new XmlDocument();
            doc.Load(putanjaRadniNalog);

            foreach (XmlNode node in doc.SelectNodes("//RadniNalog"))
            {
                string id = node["IDNaloga"]?.InnerText;
                string vozilo = node["IDVozila"]?.InnerText;
                string mehanicar = node["IDMehanicara"]?.InnerText;
                string datumStr = node["Datum"]?.InnerText;
                string status = node["Status"]?.InnerText;
                string cijenaStr = node["UkupnaCijena"]?.InnerText;
                string usluga = node["Usluga"]?.InnerText;

                if (id == null || vozilo == null || mehanicar == null ||
                    datumStr == null || status == null || cijenaStr == null)
                {
                    continue; // preskoči neispravan zapis
                }

                RadniNalog k = new RadniNalog(
                    id,
                    vozilo,
                    mehanicar,
                    DateTime.Parse(datumStr),
                    status,
                    int.Parse(cijenaStr),
                    usluga ?? ""
                );

                lista.Add(k);
            }

            return lista;
        }



        static void SpremiKlijente(List<Klijent> lista)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("Klijenti");
            doc.AppendChild(root);

            foreach (Klijent k in lista)
            {
                XmlElement klijent = doc.CreateElement("Klijent");

                XmlElement id = doc.CreateElement("ID");
                id.InnerText = k.IDKlijenta;

                XmlElement ime = doc.CreateElement("Ime");
                ime.InnerText = k.Ime;

                XmlElement prezime = doc.CreateElement("Prezime");
                prezime.InnerText = k.Prezime;

                XmlElement datum = doc.CreateElement("DatumRodjenja");
                datum.InnerText = k.DatumRodjenja.ToString("yyyy-MM-dd");

                XmlElement oib = doc.CreateElement("OIB");
                oib.InnerText = k.OIB;

                XmlElement izbrisan = doc.CreateElement("Izbrisan");
                izbrisan.InnerText = k.izbrisan.ToString();

                klijent.AppendChild(id);
                klijent.AppendChild(ime);
                klijent.AppendChild(prezime);
                klijent.AppendChild(datum);
                klijent.AppendChild(oib);
                klijent.AppendChild(izbrisan);

                root.AppendChild(klijent);
            }

            doc.Save(putanjaKlijenti);
        }

        static void SpremiVozila(List<Vozilo> lista)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("Vozila");
            doc.AppendChild(root);

            foreach (Vozilo v in lista)
            {
                XmlElement vozilo = doc.CreateElement("Vozilo");

                XmlElement id = doc.CreateElement("ID");
                id.InnerText = v.IDVozila;

                XmlElement registracija = doc.CreateElement("Registracija");
                registracija.InnerText = v.RegistracijskaOznaka;

                XmlElement marka = doc.CreateElement("Marka");
                marka.InnerText = v.Marka;

                XmlElement idklijent = doc.CreateElement("IDKlijenta");
                idklijent.InnerText = v.IDKlijenta;

                XmlElement izbrisan = doc.CreateElement("Izbrisan");
                izbrisan.InnerText = v.Izbrisan.ToString();

                vozilo.AppendChild(id);
                vozilo.AppendChild(registracija);
                vozilo.AppendChild(marka);
                vozilo.AppendChild(idklijent);
                vozilo.AppendChild(izbrisan);

                root.AppendChild(vozilo);
            }

            doc.Save(putanjaVozila);
        }


        static void SpremiMehanicar(List<Mehanicar> lista)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("Mehanicari");
            doc.AppendChild(root);

            foreach (Mehanicar m in lista)
            {
                XmlElement Mehanicar = doc.CreateElement("Klijent");

                XmlElement id = doc.CreateElement("IDMehanicar");
                id.InnerText = m.IDMehanicar;

                XmlElement ime = doc.CreateElement("Ime");
                ime.InnerText = m.Ime;

                XmlElement prezime = doc.CreateElement("Prezime");
                prezime.InnerText = m.Prezime;

                XmlElement datum = doc.CreateElement("DatumRodjenja");
                datum.InnerText = m.DatumRodjenja.ToString("yyyy-MM-dd");

                XmlElement oib = doc.CreateElement("OIB");
                oib.InnerText = m.OIB;

                XmlElement izbrisan = doc.CreateElement("Izbrisan");
                izbrisan.InnerText = m.Izbrisan.ToString();

                Mehanicar.AppendChild(id);
                Mehanicar.AppendChild(ime);
                Mehanicar.AppendChild(prezime);
                Mehanicar.AppendChild(datum);
                Mehanicar.AppendChild(oib);
                Mehanicar.AppendChild(izbrisan);

                root.AppendChild(Mehanicar);
            }

            doc.Save(putanjaMehanicar);
        }


        static void SpremiRadniNalog(List<RadniNalog> lista)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("RadniNalog");
            doc.AppendChild(root);

            foreach (RadniNalog k in lista)
            {
                XmlElement radniNalog = doc.CreateElement("RadniNalog");

                XmlElement IDNaloga = doc.CreateElement("IDNaloga");
                IDNaloga.InnerText = k.IDRadniNalog;

                XmlElement IDVozila = doc.CreateElement("IDVozila");
                IDVozila.InnerText = k.IDVozila;

                XmlElement IDMehanicara = doc.CreateElement("IDMehanicara");
                IDMehanicara.InnerText = k.IDMehanicara;

                XmlElement Datum = doc.CreateElement("Datum");
                Datum.InnerText = k.Datum.ToString("yyyy-MM-dd");

                XmlElement Status = doc.CreateElement("Status");
                Status.InnerText = k.Status;

                XmlElement UkupnaCijena = doc.CreateElement("UkupnaCijena");
                UkupnaCijena.InnerText = k.Cijena.ToString();

                XmlElement Usluga = doc.CreateElement("Usluga");
                Usluga.InnerText = k.Usluga;


                radniNalog.AppendChild(IDNaloga);
                radniNalog.AppendChild(IDVozila);
                radniNalog.AppendChild(IDMehanicara);
                radniNalog.AppendChild(Datum);
                radniNalog.AppendChild(Status);
                radniNalog.AppendChild(UkupnaCijena);
                radniNalog.AppendChild(Usluga);

                root.AppendChild(radniNalog);
            }

            doc.Save(putanjaRadniNalog);
        }
    } 
}
