using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;



namespace VUV_autoservis
{
    class Program
    {
        static string basePath = Directory.GetCurrentDirectory();

        static string putanjaKlijenti = Path.Combine(basePath, "Klijent.xml");
        static string putanjaVozila = Path.Combine(basePath, "Vozila.xml");
        static string putanjaMehanicar = Path.Combine(basePath, "Mehanicar.xml");
        static string putanjaRadniNalog = Path.Combine(basePath, "RadniNalozi.xml");





        static List<Klijent> klijenti = new List<Klijent>();
        static List<Mehanicar> mehanicari = new List<Mehanicar>();
        static List<Vozilo> vozila = new List<Vozilo>();
        static List<RadniNalog> radniNalozi = new List<RadniNalog>();




        //Dodavanje Klijenta

        public static List<Klijent> dodavanjeKlijenata(List<Klijent> lKlijenta, List<Mehanicar> lMehanicara)
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

                long provjereniOIB;

                if (!long.TryParse(oib, out provjereniOIB))
                {
                    Console.WriteLine("OIB mora sadržavati samo brojeve.");
                    continue;
                }

                if (oib.Length != 11)
                {
                    Console.WriteLine("OIB mora imati točno 11 znamenki.");
                    continue;
                }

                bool postoji = false;

                foreach (Klijent k in lKlijenta)
                {
                    if (k.OIB == oib)
                    {
                        postoji = true;
                        break;
                    }
                }

                if (!postoji)
                {
                    foreach (Mehanicar m in lMehanicara)
                    {
                        if (m.OIB == oib)
                        {
                            postoji = true;
                            break;
                        }
                    }
                }

                if (postoji)
                {
                    Console.WriteLine("Osoba s tim OIB-om već postoji.");
                    continue;
                }

                break;
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
                    Console.WriteLine("--- 4. Izbrisan");
                    Console.WriteLine("--- 5. Povratak");
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
                            Console.Write("Izbrisan (true/false): ");
                            zaAzuriranje.izbrisan = bool.Parse(Console.ReadLine());
                            break;

                        case 5:
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

        public static List<Vozilo> azuriranVozila(List<Vozilo> lVozila)
        {
        ponovniOdabirKorisnika:
            Console.Clear();
            var tableDataVozilo = new List<List<object>>();
            var tableDataVoziloZaAzuriranje = new List<List<object>>();

            Vozilo zaAzuriranje = null;

            foreach (Vozilo k in lVozila)
            {
                if (k.Izbrisan == false)
                {
                    tableDataVozilo.Add(new List<object>() { k.IDVozila, k.IDKlijenta, k.RegistracijskaOznaka, k.Marka, k.Izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataVozilo)
                .WithTitle("Vozila")
                .WithColumn("ID Vozila", "ID Klijenta", "Registracijska oznaka", "Marka", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID vozila kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lVozila;
            }


            int brojObrisanih = 0;
            foreach (Vozilo k in lVozila)
            {
                if (k.IDVozila == odabir)
                {
                    zaAzuriranje = k;
                    tableDataVoziloZaAzuriranje.Add(new List<object>() { k.IDVozila, k.IDKlijenta, k.RegistracijskaOznaka, k.Marka, k.Izbrisan });
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.Clear();

                ConsoleTableBuilder
        .From(tableDataVoziloZaAzuriranje)
        .WithTitle("Odabrano vozilo")
        .WithColumn("ID Vozila", "ID Klijenta", "Registracijska oznaka", "Marka", "Izbrisan")
        .ExportAndWriteLine();


                bool izbornikZaAzuriranjeKorisnika = true;
                while (izbornikZaAzuriranjeKorisnika)
                {
                    Console.WriteLine("Odaberi koji dio klijenta zelis azurirati.");
                    Console.WriteLine("--- 1. Registracijska oznaka");
                    Console.WriteLine("--- 2. Marka");
                    Console.WriteLine("--- 3. Povratak");
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
                            zaAzuriranje.RegistracijskaOznaka = Console.ReadLine();
                            break;

                        case 2:
                            Console.Write("Unesi novo prezime: ");
                            zaAzuriranje.Marka = Console.ReadLine();
                            break;
                        case 3:
                            goto ponovniOdabirKorisnika;

                        default:
                            Console.WriteLine("Nevažeći odabir.");
                            break;
                    }
                }


                int index = lVozila.FindIndex(k => k.IDVozila == zaAzuriranje.IDVozila);

                if (index != -1)
                {
                    lVozila[index] = zaAzuriranje;
                }

                return lVozila;
            }
            else
            {
                Console.WriteLine("Vozilo sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }


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

            if (odabir.ToLower() == "povratak")
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

        public static List<Mehanicar> dodavanjeMehanicar(List<Mehanicar> lMehanicar, List<Klijent> lKlijenata)
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

                long provjereniOIB;

                if (!long.TryParse(oib, out provjereniOIB))
                {
                    Console.WriteLine("OIB mora sadržavati samo brojeve.");
                    continue;
                }

                if (oib.Length != 11)
                {
                    Console.WriteLine("OIB mora imati točno 11 znamenki.");
                    continue;
                }

                bool postoji = false;

                foreach (Klijent k in lKlijenata)
                {
                    if (k.OIB == oib)
                    {
                        postoji = true;
                        break;
                    }
                }

                if (!postoji)
                {
                    foreach (Mehanicar m in lMehanicar)
                    {
                        if (m.OIB == oib)
                        {
                            postoji = true;
                            break;
                        }
                    }
                }

                if (postoji)
                {
                    Console.WriteLine("Osoba s tim OIB-om već postoji.");
                    continue;
                }

                break;
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


        public static List<Mehanicar> azuriranjeMehanicara(List<Mehanicar> lMehanicara)
        {
        ponovniOdabirKorisnika:
            Console.Clear();
            var tableDataMehanicar = new List<List<object>>();
            var tableDataMehanicarZaAzuriranje = new List<List<object>>();

            Mehanicar zaAzuriranje = null;

            foreach (Mehanicar k in lMehanicara)
            {
                if (k.Izbrisan == false)
                {
                    tableDataMehanicar.Add(new List<object>() { k.IDMehanicar, k.Ime, k.Prezime, k.OIB, k.Izbrisan });
                }

            }

            ConsoleTableBuilder
                .From(tableDataMehanicar)
                .WithTitle("Vozila")
                .WithColumn("ID Mehanicara", "Ime", "Prezime", "OIB", "Izbrisan")
                .ExportAndWriteLine();

        ponovniUnosID:
            Console.WriteLine("Unesi ID mehaničara kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lMehanicara;
            }


            int brojObrisanih = 0;
            foreach (Mehanicar k in lMehanicara)
            {
                if (k.IDMehanicar == odabir)
                {
                    zaAzuriranje = k;
                    tableDataMehanicarZaAzuriranje.Add(new List<object>() { k.IDMehanicar, k.Ime, k.Prezime, k.OIB, k.Izbrisan });
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {
                Console.Clear();

                ConsoleTableBuilder
        .From(tableDataMehanicarZaAzuriranje)
        .WithTitle("Odabrani mehaničar")
        .WithColumn("ID Mehanicara", "Ime", "Prezime", "OIB", "Izbrisan")
        .ExportAndWriteLine();


                bool izbornikZaAzuriranjeKorisnika = true;
                while (izbornikZaAzuriranjeKorisnika)
                {
                    Console.WriteLine("Odaberi koji dio klijenta zelis azurirati.");
                    Console.WriteLine("--- 1. Ime");
                    Console.WriteLine("--- 2. Prezime");
                    Console.WriteLine("--- 3. Datum rođenja");
                    Console.WriteLine("--- 4. Izbrisan");
                    Console.WriteLine("--- 5. Povratak");
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
                            Console.Write("Izbrisan (true/false): ");
                            zaAzuriranje.Izbrisan = bool.Parse(Console.ReadLine());
                            break;

                        case 5:
                            goto ponovniOdabirKorisnika;

                        default:
                            Console.WriteLine("Nevažeći odabir.");
                            break;
                    }
                }


                int index = lMehanicara.FindIndex(k => k.IDMehanicar == zaAzuriranje.IDMehanicar);

                if (index != -1)
                {
                    lMehanicara[index] = zaAzuriranje;
                }

                return lMehanicara;
            }
            else
            {
                Console.WriteLine("Mehaničar sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }


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
                .WithColumn("IDMehanicara", "Ime", "Prezime", "Datum rodjenja", "OIB", "Izbrisan")
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
                Console.WriteLine("Mehanicar uspješno obirsan.");
                return lMehanicara;
            }
            else
            {
                Console.WriteLine("Mehanicar sa traženim ID-em nije pronađen.");
                goto ponovniUnosID;
            }

        }





        //Radni nalozi
        public static List<RadniNalog> otvaranjeRadnogNaloga(List<RadniNalog> lRadnihNaloga, List<Klijent> lKlijenta, List<Mehanicar> lMehnaicara, List<Vozilo> lVozila)
        {
            Console.Clear();
        ponovniOdabirKorisnika:
            Console.WriteLine("--- Unos Novog Naloga ---");
            var tableDataKlijent = new List<List<object>>();
            var tableDataKlijentZaAzuriranje = new List<List<object>>();


            string noviRadniNalogIDKlijenta = null;
            string noviRadniNalogIDVozila = null;
            string noviRadniNalogIDMehanicara = null;
            int noviRadniNalogCijena = 0;
            string noviRadniNalogUsluga = null;
            string noviRadniNalogStatus = null;
            DateTime Datum = DateTime.Now;

            string id = Convert.ToString("N" + (lRadnihNaloga.Count + 1));


            foreach (Klijent k in lKlijenta)
            {
                if (k.izbrisan == false)
                {
                    tableDataKlijent.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                }

            }

            ConsoleTableBuilder.From(tableDataKlijent).WithTitle("Klijenti").WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan").ExportAndWriteLine();

            Console.WriteLine("Unesi ID klijenta za kojeg želiš kreirati radni nalog. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lRadnihNaloga;
            }


            int brojObrisanih = 0;
            foreach (Klijent k in lKlijenta)
            {
                if (k.IDKlijenta == odabir)
                {
                    noviRadniNalogIDKlijenta = k.IDKlijenta;
                    tableDataKlijentZaAzuriranje.Add(new List<object>() { k.IDKlijenta, k.Ime, k.Prezime, k.OIB, k.izbrisan });
                    brojObrisanih++;
                }
            }
            if (brojObrisanih != 0)
            {



                Console.Clear();
            ponovniOdabiVozila:
                ConsoleTableBuilder.From(tableDataKlijentZaAzuriranje).WithTitle("Odabrani Klijent").WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan").ExportAndWriteLine();
                var tableDataVozilo = new List<List<object>>();
                var tableDataVoziloZaAzuriranje = new List<List<object>>();


                foreach (Vozilo k in lVozila)
                {
                    if (k.Izbrisan == false)
                    {
                        tableDataVozilo.Add(new List<object>() { k.IDVozila, k.IDKlijenta, k.RegistracijskaOznaka, k.Marka, k.Izbrisan });
                    }

                }

                ConsoleTableBuilder.From(tableDataVozilo).WithTitle("Vozila").WithColumn("ID Vozila", "ID Klijenta", "Registracijska oznaka", "Marka", "Izbrisan").ExportAndWriteLine();

                Console.WriteLine("Unesi ID vozila za koje želiš kreirati radni nalog. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
                odabir = Console.ReadLine();
                if (odabir.ToLower() == "povratak")
                {
                    goto ponovniOdabirKorisnika;
                }


                brojObrisanih = 0;
                foreach (Vozilo k in lVozila)
                {
                    if (k.IDVozila == odabir)
                    {
                        noviRadniNalogIDVozila = k.IDVozila;
                        tableDataVoziloZaAzuriranje.Add(new List<object>() { k.IDVozila, k.IDKlijenta, k.RegistracijskaOznaka, k.Marka, k.Izbrisan });
                        brojObrisanih++;
                    }
                }
                if (brojObrisanih != 0)
                {

                    Console.Clear();
                ponovniOdabirMehanicar:
                    ConsoleTableBuilder.From(tableDataKlijentZaAzuriranje).WithTitle("Odabrani Klijent").WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan").ExportAndWriteLine();
                    ConsoleTableBuilder.From(tableDataVoziloZaAzuriranje).WithTitle("Odabrano vozilo").WithColumn("ID Vozila", "ID Klijenta", "Registracijska oznaka", "Marka", "Izbrisan").ExportAndWriteLine();

                    var tableDataMehanicar = new List<List<object>>();
                    var tableDataMehanicarZaAzuriranje = new List<List<object>>();


                    foreach (Mehanicar k in lMehnaicara)
                    {
                        if (k.Izbrisan == false)
                        {
                            tableDataMehanicar.Add(new List<object>() { k.IDMehanicar, k.Ime, k.Prezime, k.OIB, k.Izbrisan });
                        }

                    }

                    ConsoleTableBuilder
                        .From(tableDataMehanicar)
                        .WithTitle("Vozila")
                        .WithColumn("ID Mehanicara", "Ime", "Prezime", "OIB", "Izbrisan")
                        .ExportAndWriteLine();

                    Console.WriteLine("Unesi ID mehaničara kojeg želiš izbrisati. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
                    odabir = Console.ReadLine();
                    if (odabir.ToLower() == "povratak")
                    {
                        goto ponovniOdabiVozila;
                    }


                    brojObrisanih = 0;
                    foreach (Mehanicar k in lMehnaicara)
                    {
                        if (k.IDMehanicar == odabir)
                        {
                            noviRadniNalogIDMehanicara = k.IDMehanicar;
                            tableDataMehanicarZaAzuriranje.Add(new List<object>() { k.IDMehanicar, k.Ime, k.Prezime, k.OIB, k.Izbrisan });
                            brojObrisanih++;
                        }
                    }
                    if (brojObrisanih != 0)
                    {
                        Console.Clear();
                        ConsoleTableBuilder.From(tableDataKlijentZaAzuriranje).WithTitle("Odabrani Klijent").WithColumn("ID", "Ime", "Prezime", "OIB", "Izbrisan").ExportAndWriteLine();
                        ConsoleTableBuilder.From(tableDataVoziloZaAzuriranje).WithTitle("Odabrano vozilo").WithColumn("ID Vozila", "ID Klijenta", "Registracijska oznaka", "Marka", "Izbrisan").ExportAndWriteLine();
                        ConsoleTableBuilder.From(tableDataMehanicarZaAzuriranje).WithTitle("Odabrani mehaničar").WithColumn("ID Mehanicara", "Ime", "Prezime", "OIB", "Izbrisan").ExportAndWriteLine();

                        bool izbornikZaOdabirPosla = true;

                        while (izbornikZaOdabirPosla)
                        {
                            Console.WriteLine("--- Idabir posla --- ");
                            Console.WriteLine("1. Dijagnostika");
                            Console.WriteLine("2. Popravak");
                            Console.WriteLine("3. Zamjena dijela");
                            Console.WriteLine("4. Povratak");

                            Console.WriteLine("Unesi svoj odabir. ");
                            odabir = Console.ReadLine();
                            int odabirProvjereno = 0;
                            if (!int.TryParse(odabir, out odabirProvjereno))
                            {
                                Console.WriteLine("Unos mora biti broj.");
                                odabirProvjereno = 0;
                            }
                            switch (odabirProvjereno)
                            {
                                case 1:
                                    Dijagnostika dijagnostika = new Dijagnostika("Dijagnostika kvara.");
                                    noviRadniNalogCijena = dijagnostika.IzracunajCijenu();
                                    noviRadniNalogUsluga = dijagnostika.Naziv;
                                    noviRadniNalogStatus = "Zaprimljen";
                                    izbornikZaOdabirPosla = false;
                                    break;
                                case 2:

                                    Console.WriteLine("Unesi broj radnih sati. ");
                                    odabir = Console.ReadLine();
                                    odabirProvjereno = 0;
                                    if (!int.TryParse(odabir, out odabirProvjereno))
                                    {
                                        Console.WriteLine("Unos mora biti broj.");
                                        odabirProvjereno = 0;
                                    }

                                    Popravak popravak = new Popravak("Popravak", odabirProvjereno);
                                    noviRadniNalogCijena = popravak.IzracunajCijenu();
                                    noviRadniNalogUsluga = popravak.Naziv;
                                    noviRadniNalogStatus = "Zaprimljen";
                                    izbornikZaOdabirPosla = false;
                                    break;
                                case 3:
                                    Console.WriteLine("Unesi cijenu zamjenskog dijela.");
                                    odabir = Console.ReadLine();
                                    odabirProvjereno = 0;
                                    if (!int.TryParse(odabir, out odabirProvjereno))
                                    {
                                        Console.WriteLine("Unos mora biti broj.");
                                        odabirProvjereno = 0;
                                    }

                                    ZamjenaDijela d = new ZamjenaDijela("Zamjena dijela.", odabirProvjereno);
                                    noviRadniNalogCijena = d.IzracunajCijenu();
                                    noviRadniNalogUsluga = d.Naziv;
                                    noviRadniNalogStatus = "Zaprimljen";
                                    izbornikZaOdabirPosla = false;
                                    break;
                                case 4:
                                    goto ponovniOdabirMehanicar;
                                default:
                                    Console.WriteLine("Nevažeći odabir.");
                                    break;
                            }

                        }

                    }
                    else
                    {
                        Console.WriteLine("Mehanicar sa traženim ID-em nije pronađen.");
                        goto ponovniOdabirMehanicar;
                    }
                }
                else
                {
                    Console.WriteLine("Vozilo sa traženim ID-em nije pronađen.");
                    goto ponovniOdabiVozila;
                }
            }
            else
            {
                Console.WriteLine("Klijent sa traženim ID-em nije pronađen.");
                goto ponovniOdabirKorisnika;
            }

            RadniNalog noviRadniNalog = new RadniNalog(
    id,
    noviRadniNalogIDVozila,
    noviRadniNalogIDMehanicara,
    noviRadniNalogIDKlijenta,
    Datum,
    noviRadniNalogStatus,
    noviRadniNalogCijena,
    noviRadniNalogUsluga
); ;

            lRadnihNaloga.Add(noviRadniNalog);
            return lRadnihNaloga;

        }




        public static List<RadniNalog> promjenaStatusaRadnogNaloga(List<RadniNalog> lRadnihNaloga)
        {
            Console.Clear();
        ponovniOdabirRadnogNaloga:
            Console.WriteLine("--- Azuriranje statusa naloga  ---");
            var tableDataRadniNalozi = new List<List<object>>();
            var tableDataRadniNalozitZaAzuriranje = new List<List<object>>();


            string id = Convert.ToString("N" + (lRadnihNaloga.Count + 1));

            RadniNalog noviRadniNalogIDKlijenta = null;


            foreach (RadniNalog k in lRadnihNaloga)
            {
                tableDataRadniNalozi.Add(new List<object>() { k.IDRadniNalog, k.IDVozila, k.IDKlijenta, k.IDMehanicara, k.Datum, k.Status, k.Cijena, k.Usluga });
            }



            ConsoleTableBuilder.From(tableDataRadniNalozi).WithTitle("Radni nalozi").WithColumn("ID radnog nalog", "ID vozila", "ID Klijenta", "ID Mehanicara", "Datum", "Status", "Cijena", "Usluga").ExportAndWriteLine();

            Console.WriteLine("Unesi ID naloga za kojeg želiš azurirati status. Upiši 'Povratak' za povratak na izbornik bez brisanje klijenta.");
            string odabir = Console.ReadLine();
            if (odabir.ToLower() == "povratak")
            {
                return lRadnihNaloga;
            }


            int brojObrisanih = 0;
            foreach (RadniNalog k in lRadnihNaloga)
            {
                if (k.IDRadniNalog == odabir)
                {
                    noviRadniNalogIDKlijenta = k;
                    tableDataRadniNalozitZaAzuriranje.Add(new List<object>() { k.IDRadniNalog, k.IDVozila, k.IDKlijenta, k.IDMehanicara, k.Datum, k.Status, k.Cijena, k.Usluga });
                    brojObrisanih++;
                }
            }

            ConsoleTableBuilder.From(tableDataRadniNalozitZaAzuriranje).WithTitle("Odabrani radni nalog.").WithColumn("ID radnog nalog", "ID vozila", "ID Klijenta", "ID Mehanicara", "Datum", "Status", "Cijena", "Usluga").ExportAndWriteLine();



            if (noviRadniNalogIDKlijenta.Status == "Zaprimljen")
            {

                bool izbornikStatusa = true;
                while (izbornikStatusa)
                {
                    Console.Clear();
                    Console.WriteLine("--- Odaberi novi status ---");
                    Console.WriteLine("--- 1. U_Radu ---");
                    Console.WriteLine("--- 2. Zavrsen ---");
                    Console.WriteLine("--- 3. Povratak ---");

                    odabir = Console.ReadLine();
                    int odabirProvjereno = 0;
                    if (!int.TryParse(odabir, out odabirProvjereno))
                    {
                        Console.WriteLine("Odabir smora biti broj.");
                        odabirProvjereno = 0;
                    }
                    switch (odabirProvjereno)
                    {
                        case 1:

                            noviRadniNalogIDKlijenta.Status = "U_Radu";
                            izbornikStatusa = false;
                            break;
                        case 2:

                            noviRadniNalogIDKlijenta.Status = "Zavrsen";
                            izbornikStatusa = false;

                            break;
                        case 3:
                            goto ponovniOdabirRadnogNaloga;
                        default:
                            Console.WriteLine("Uneseno nešto što nije opcija.");
                            break;

                    }
                }
            }
            else if (noviRadniNalogIDKlijenta.Status == "U_Radu")
            {

                bool izbornikStatusa = true;
                while (izbornikStatusa)
                {
                    Console.Clear();
                    Console.WriteLine("--- Odaberi novi status ---");
                    Console.WriteLine("--- 1. Zaprimljen ---");
                    Console.WriteLine("--- 2. Zavrsen ---");
                    Console.WriteLine("--- 3. Povratak ---");

                    odabir = Console.ReadLine();
                    int odabirProvjereno = 0;
                    if (!int.TryParse(odabir, out odabirProvjereno))
                    {
                        Console.WriteLine("Odabir smora biti broj.");
                        odabirProvjereno = 0;
                    }
                    switch (odabirProvjereno)
                    {
                        case 1:

                            noviRadniNalogIDKlijenta.Status = "Zaprimljen";
                            izbornikStatusa = false;
                            break;
                        case 2:

                            noviRadniNalogIDKlijenta.Status = "Zavrsen";
                            izbornikStatusa = false;

                            break;
                        case 3:
                            goto ponovniOdabirRadnogNaloga;
                        default:
                            Console.WriteLine("Uneseno nešto što nije opcija.");
                            break;

                    }
                }
            }
            else if (noviRadniNalogIDKlijenta.Status == "Zavrsen")
            {

                bool izbornikStatusa = true;
                while (izbornikStatusa)
                {
                    Console.Clear();
                    Console.WriteLine("--- Odaberi novi status ---");
                    Console.WriteLine("--- 1. Zapriljen ---");
                    Console.WriteLine("--- 2. U_Radu ---");
                    Console.WriteLine("--- 3. Povratak ---");

                    odabir = Console.ReadLine();
                    int odabirProvjereno = 0;
                    if (!int.TryParse(odabir, out odabirProvjereno))
                    {
                        Console.WriteLine("Odabir smora biti broj.");
                        odabirProvjereno = 0;
                    }
                    switch (odabirProvjereno)
                    {
                        case 1:

                            noviRadniNalogIDKlijenta.Status = "Zapriljen";
                            break;
                        case 2:

                            noviRadniNalogIDKlijenta.Status = "U_Radu";

                            break;
                        case 3:
                            goto ponovniOdabirRadnogNaloga;
                        default:
                            Console.WriteLine("Uneseno nešto što nije opcija.");
                            break;

                    }
                }
            }
            int index = lRadnihNaloga.FindIndex(k => k.IDRadniNalog == noviRadniNalogIDKlijenta.IDRadniNalog);

            if (index != -1)
            {
                lRadnihNaloga[index] = noviRadniNalogIDKlijenta;
            }

            return lRadnihNaloga;
        }













        //Pregled radnih naloga


        public static void pregledNalogaVozila(List<RadniNalog> lRadniNalozi, List<Vozilo> lVozila)
        {
            Console.Clear();


            var tableDataVozila = new List<List<object>>();
            var tableDataOdabranoVozilo = new List<List<object>>();
            var radniNaloziOdabranogVozila = new List<List<object>>();

            Vozilo odabranoVozilo = null;



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
            Console.WriteLine("Unesi ID vozila kojeg želiš odabrati. Ili upiši 'Povratak' za izlazak bez brisanja vozila.");
            string odabir = Console.ReadLine();

            if (odabir.ToLower() == "povratak")
            {

            }
            else {
                int brojObrisanih = 0;
                foreach (Vozilo v in lVozila)
                {
                    if (v.IDVozila == odabir)
                    {
                        odabranoVozilo = v;
                        tableDataOdabranoVozilo.Add(new List<object>() { v.IDVozila, v.RegistracijskaOznaka, v.Marka, v.IDKlijenta, v.Izbrisan });
                        brojObrisanih++;
                    }
                }
                if (brojObrisanih != 0)
                {
                    foreach (RadniNalog n in lRadniNalozi)
                    {
                        if (n.IDVozila == odabranoVozilo.IDVozila)
                        {
                            radniNaloziOdabranogVozila.Add(new List<object>() { n.IDRadniNalog, n.IDVozila, n.IDKlijenta, n.IDMehanicara, n.Datum, n.Status, n.Cijena, n.Usluga });
                        }

                    }
                    Console.Clear();
                    ConsoleTableBuilder.From(tableDataVozila).WithTitle("Odabrano Vozilo").WithColumn("IDVozila", "Registracija", "Marka", "IDKlijenta", "Izbrisan").ExportAndWriteLine();
                    ConsoleTableBuilder.From(radniNaloziOdabranogVozila).WithTitle("Radni nalozi").WithColumn("ID radnog nalog", "ID vozila", "ID Klijenta", "ID Mehanicara", "Datum", "Status", "Cijena", "Usluga").ExportAndWriteLine();
                    Console.WriteLine("Pritisnite ENTER za nastavak.");
                    Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Vozilo sa traženim ID-em nije pronađen.");
                    goto ponovniUnosID;
                }
            }
        }

        public static void statistika(List<Vozilo> lVozila, List<Klijent> lKlijenta, List<RadniNalog> lRadniNalozi, List<Mehanicar> lMehanicara)
        {

            // PRIHOD PO USLUGAMA
            var tableDataUsluge = new List<List<object>>();
            int sumaDia = 0;
            int sumaPopravak = 0;
            int sumaZamjenaDijela = 0;





            foreach (RadniNalog nalog in lRadniNalozi)
            {
                if (nalog.Status == "Zavrsen")
                {
                    if(nalog.Usluga == "Dijagnostika")
                    {
                        sumaDia += nalog.Cijena;
                    }else if(nalog.Usluga == "Popravak")
                    {
                        sumaPopravak += nalog.Cijena;
                    }else if(nalog.Usluga == "Dijagnostika kvara.")
                    {
                        sumaZamjenaDijela += nalog.Cijena;
                    }
                }
            }
            tableDataUsluge.Add(new List<object>() { "Dijagnostika", sumaDia });
            tableDataUsluge.Add(new List<object>() { "Popravak", sumaPopravak });
            tableDataUsluge.Add(new List<object>() { "Dijagnostika kvara.", sumaZamjenaDijela });

            for (int i = 0; i < tableDataUsluge.Count - 1; i++)
            {
                for (int j = 0; j < tableDataUsluge.Count - i - 1; j++)
                {
                    int trenutni = Convert.ToInt32(tableDataUsluge[j][1]);
                    int sljedeci = Convert.ToInt32(tableDataUsluge[j + 1][1]);

                    if (trenutni < sljedeci)
                    {
                        var temp = tableDataUsluge[j];
                        tableDataUsluge[j] = tableDataUsluge[j + 1];
                        tableDataUsluge[j + 1] = temp;
                    }
                }
            }

            ConsoleTableBuilder
    .From(tableDataUsluge)
    .WithTitle("Prihod po vrsti usluge (sortirano)")
    .WithColumn("Usluga", "Prihod")
    .ExportAndWriteLine();


            // TOP 5 MEHANIČARA

            var tableDataMehanicari = new List<List<object>>();

            foreach (Mehanicar m in lMehanicara)
            {
                int brojNaloga = 0;

                foreach (RadniNalog nalog in lRadniNalozi)
                {
                    if (nalog.IDMehanicara == m.IDMehanicar)
                    {
                        brojNaloga++;
                    }
                }

                tableDataMehanicari.Add(new List<object>()
        {
            m.IDMehanicar,
            m.Ime + " " + m.Prezime,
            brojNaloga
        });
            }

            // sortiranje po broju naloga
            for (int i = 0; i < tableDataMehanicari.Count - 1; i++)
            {
                for (int j = i + 1; j < tableDataMehanicari.Count; j++)
                {
                    if (Convert.ToInt32(tableDataMehanicari[j][2]) >
                        Convert.ToInt32(tableDataMehanicari[i][2]))
                    {
                        var temp = tableDataMehanicari[i];
                        tableDataMehanicari[i] = tableDataMehanicari[j];
                        tableDataMehanicari[j] = temp;
                    }
                }
            }

            var top5Mehanicara = new List<List<object>>();

            for (int i = 0; i < tableDataMehanicari.Count && i < 5; i++)
            {
                top5Mehanicara.Add(tableDataMehanicari[i]);
            }

            ConsoleTableBuilder
                .From(top5Mehanicara)
                .WithTitle("Top 5 mehanicara")
                .WithColumn("ID", "Ime i prezime", "Broj naloga")
                .ExportAndWriteLine();


            // PROSJEČNA CIJENA

            int ukupnaCijena = 0;

            foreach (RadniNalog nalog in lRadniNalozi)
            {
                if(nalog.Status == "Zavrsen") {
                    ukupnaCijena += nalog.Cijena;
                }
            }

            double prosjecnaCijena = 0;

            if (lRadniNalozi.Count > 0)
            {
                prosjecnaCijena = (double)ukupnaCijena / lRadniNalozi.Count;
            }

            var tableDataProsjek = new List<List<object>>();

            tableDataProsjek.Add(new List<object>()
    {
        Math.Round(prosjecnaCijena, 2)
    });

            ConsoleTableBuilder
                .From(tableDataProsjek)
                .WithColumn("Prosjecna cijena")
                .ExportAndWriteLine();
        }
























        static void Main(string[] args)
        {
            klijenti = UcitajKlijente();
            vozila = UcitajVozila();
            mehanicari = UcitajMehanicare();
            radniNalozi = UcitajRadniNalog();



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
                                        klijenti = dodavanjeKlijenata(klijenti, mehanicari);
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
                                        vozila = azuriranVozila(vozila);
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
                                        mehanicari = dodavanjeMehanicar(mehanicari, klijenti);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 2:
                                        mehanicari = azuriranjeMehanicara(mehanicari);
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
                                        radniNalozi = otvaranjeRadnogNaloga(radniNalozi, klijenti, mehanicari, vozila);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 2:
                                        radniNalozi = promjenaStatusaRadnogNaloga(radniNalozi);
                                        SpremiKlijente(klijenti);
                                        SpremiVozila(vozila);
                                        SpremiMehanicar(mehanicari);
                                        SpremiRadniNalog(radniNalozi);
                                        break;
                                    case 3:
                                        pregledNalogaVozila(radniNalozi, vozila);
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

                            statistika(vozila, klijenti, radniNalozi, mehanicari);

                            Console.WriteLine("Pritisni ENTER za povratak na izbornik.");
                            Console.ReadLine();
                            izbornikStatistika = false;
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
                string klijent = node["IDKlijenta"]?.InnerText;
                string datumStr = node["Datum"]?.InnerText;
                string status = node["Status"]?.InnerText;
                string cijenaStr = node["UkupnaCijena"]?.InnerText;
                string usluga = node["Usluga"]?.InnerText;

                if (id == null || vozilo == null || mehanicar == null ||
                    datumStr == null || status == null || cijenaStr == null)
                {
                    continue;
                }

                RadniNalog k = new RadniNalog(
                    id,
                    vozilo,
                    mehanicar,
                    klijent,
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
                XmlElement Mehanicar = doc.CreateElement("Mehanicar");

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

            XmlElement root = doc.CreateElement("RadniNalozi");
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

                XmlElement IDKlijenta = doc.CreateElement("IDKlijenta");
                IDKlijenta.InnerText = k.IDKlijenta;

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
                radniNalog.AppendChild(IDKlijenta);
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
