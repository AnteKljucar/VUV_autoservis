using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

namespace VUV_autoservis
{
    class Program
    {
        static List<Klijent> klijenti = new List<Klijent>();
        static List<Mehanicar> mehanicari = new List<Mehanicar>();
        static List<Vozilo> vozila = new List<Vozilo>();
        static List<RadniNalog> radniNalozi = new List<RadniNalog>();

        static void Main(string[] args)
        {

            UcitajSvePodatke();
            IspisiSvePodatke();


            try
            { 


                bool pocetniIzbornik = true;
            while (pocetniIzbornik)
            {
                    Console.Clear();
                    Console.WriteLine("--- Izbornik ---");
                    Console.WriteLine("--- 1. Upravljanje klijentima ---");
                    Console.WriteLine("--- 2. Upravljanje vozilima ---");
                    Console.WriteLine("--- 3. Upravljanje nalozima ---");
                    Console.WriteLine("--- 4. Statistika ---");
                    Console.WriteLine("--- 5. Izlaz ---");

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
                                Console.WriteLine("OIB smije sadržavati samo brojeve.");
                                odabirProvjereno = 0;
                            }
                            bool izbornikKlijent = true;

                        while (izbornikKlijent)
                        {
                            switch (odabirProvjereno)
                            {
                                case 1:
                                    break;
                                case 2:
                                    break;
                                case 3:
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
                                Console.WriteLine("OIB smije sadržavati samo brojeve.");
                                odabirProvjereno = 0;
                            }
                            bool izbornikVozila = true;

                        while (izbornikVozila)
                        {
                            switch (odabirProvjereno)
                            {
                                case 1:
                                    break;
                                case 2:
                                    break;
                                case 3:
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
                                Console.WriteLine("OIB smije sadržavati samo brojeve.");
                                odabirProvjereno = 0;
                            }
                            bool izbornikNaloga = true;

                        while (izbornikNaloga)
                        {
                            switch (odabirProvjereno)
                            {
                                case 1:
                                    break;
                                case 2:
                                    break;
                                case 3:
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
                    case 4:
                            Console.Clear();
                            Console.WriteLine("--- Izbornik ---");
                        Console.WriteLine("--- 4. Povratak ---");


                            odabir = Console.ReadLine();
                            odabirProvjereno = 0;
                            if (!int.TryParse(odabir, out odabirProvjereno))
                            {
                                Console.WriteLine("OIB smije sadržavati samo brojeve.");
                                odabirProvjereno = 0;
                            }
                            bool izbornikStatistika = true;
                        while (izbornikStatistika)
                        {
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
                    case 5:
                        pocetniIzbornik = false;
                        break;
                    default:
                        Console.WriteLine("Odabrano nešto što nije opcija.");
                        break;

                }
            }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }//Main

        static void UcitajSvePodatke()
        {
            UcitajKlijente();
            UcitajMehanicare();
            UcitajVozila();

        }

        static void UcitajKlijente()
        {
            if (!File.Exists("Klijent.xml"))
                return;

            XDocument xml = XDocument.Load("Klijent.xml");

            foreach (var k in xml.Root.Elements("Klijent"))
            {
                klijenti.Add(new Klijent(
                    k.Element("IDKlijenta").Value,
                    k.Element("IDOsoba").Value,
                    k.Element("Ime").Value,
                    k.Element("Prezime").Value,
                    DateTime.Parse(k.Element("DatumRodjenja").Value),
                    k.Element("OIB").Value
                ));
            }
        }
        static void UcitajMehanicare()
        {
            if (!File.Exists("Mehanicar.xml"))
                return;

            XDocument xml = XDocument.Load("Mehanicar.xml");

            foreach (var m in xml.Root.Elements("Mehanicar"))
            {
                mehanicari.Add(new Mehanicar(
                    m.Element("IDMehanicar").Value,
                    m.Element("IDOsoba").Value,
                    m.Element("Ime").Value,
                    m.Element("Prezime").Value,
                    DateTime.Parse(m.Element("DatumRodjenja").Value),
                    m.Element("OIB").Value
                ));
            }
        }

        static void UcitajVozila()
        {
            if (!File.Exists("Vozila.xml"))
                return;

            XDocument xml = XDocument.Load("Vozila.xml");

            foreach (var v in xml.Root.Elements("Vozilo"))
            {
                string idKlijenta = v.Element("IDKlijenta").Value;

                Klijent vlasnik = null;

                foreach (Klijent k in klijenti)
                {
                    if (k.IDKlijenta == idKlijenta)
                    {
                        vlasnik = k;
                        break;
                    }
                }

                if (vlasnik != null)
                {
                    vozila.Add(new Vozilo(
                        v.Element("IDVozila").Value,
                        v.Element("Registracija").Value,
                        v.Element("Marka").Value,
                        vlasnik
                    ));
                }
            }
        }


        static void IspisiSvePodatke()
        {
            Console.WriteLine("===== KLIJENTI =====");
            foreach (var k in klijenti)
            {
                Console.WriteLine(k.IDKlijenta + " | " + k.Ime + " " + k.Prezime + " | OIB: " + k.OIB);
            }

            Console.WriteLine("\n===== MEHANIČARI =====");
            foreach (var m in mehanicari)
            {
                Console.WriteLine(m.IDMehanicar + " | " + m.Ime + " " + m.Prezime + " | OIB: " + m.OIB);
            }

            Console.WriteLine("\n===== VOZILA =====");
            foreach (var v in vozila)
            {
                Console.WriteLine(v.IDVozila + " | " + v.Marka + " | Vlasnik: " +
                                  v.Vlasnik.Ime + " " + v.Vlasnik.Prezime);
            }

            Console.WriteLine("\n======================\n");
            Console.WriteLine("Pritisni ENTER za nastavak...");
            Console.ReadLine();
        }


    }
}
