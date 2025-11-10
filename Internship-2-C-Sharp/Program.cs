using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("\n\t\t\t\t\tAPLIKACIJA ZA EVIDENCIJU GORIVA\n");
        var trips = new Dictionary<int, Tuple<string, DateTime, double, double, double, double>>()
        {
            { 1,new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Split", new DateTime(2024, 7, 15), 410.0, 0.08, 50.0, whole_price(410.0, 0.08, 50.0) ) },
            { 2, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Dubrovnik", new DateTime(2024, 8, 10), 600.0, 0.09, 70.0, whole_price(600.0, 0.09, 70.0) ) },
            { 3, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Rijeka", new DateTime(2024, 9, 5), 160.0, 0.07, 30.0, whole_price(160.0, 0.07, 30.0) ) }
        };
        var users = new Dictionary<int, Tuple<string, string, DateTime, List<int>>>()
        {
            { 1, new Tuple<string, string, DateTime,List<int>>("Ivan"," Horvat", new DateTime(1990, 1, 1),new List<int>{ 1, 2 }) },
            { 2, new Tuple<string, string, DateTime,List<int>>("Ana"," Novak", new DateTime(1992, 2, 2), new List<int>{ 2 }) },
            { 3, new Tuple<string, string, DateTime,List<int>>("Marko"," Petric", new DateTime(1995, 3, 3), new List<int>{ 3 }) }
        };
        var input = -1;
       
        while (input != 0)
        {
            Console.WriteLine("1 - Korisnici\n2 - Putovanja\n0 - Izlaz iz aplikacije");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    choice_users(users,trips);
                    break;
                case 2:
                    trips_choice();
                    break;
                case 0:
                    Console.WriteLine("Izlaz iz aplikacije");
                    input = 0;
                    break;
                default:
                    Console.WriteLine("Pogresan unos, pokusajte ponovo.");
                    break;
            }
            
        }

    }
    static double whole_price(double km, double gas, double price)
    {
        return km * gas*price;
    }
    static void choice_users(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        var input = -1;
        while (input != 0)
        {
            Console.WriteLine("1 - Unos novog korisnika\r\n2 - Brisanje korisnika\r\n3 - Uređivanje korisnika\r\n4 - Pregled svih korisnika\r\n0 - Povratak na glavni izbornik\r\n");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    user_input(users,trips);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 0:
                    Console.WriteLine("Povratak u početni izbornik");
                    input = 0;
                    break;
                default:
                    Console.WriteLine("Pogresan unos, pokusajte ponovo.");
                    break;
            }

        }
       

    }
    static void user_input(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        int id = users.Count+1;
        int input=-1;
        string name = null,last_name = null;
        DateTime birthday= DateTime.MinValue;
        List<int> chosen_trips = new List<int>();
        try
       {
           Console.Write("Unesi ime: ");
           name = Console.ReadLine();

           Console.Write("Unesi prezime: ");
           last_name = Console.ReadLine();
            
           Console.Write("Unesi datum rođenja (yyyy-mm-dd): ");
            while(birthday==DateTime.MinValue)
            {
                birthday = DateTime.Parse(Console.ReadLine());
                if (birthday.Year < 1925 || birthday.Year > 2025)
                {
                    Console.WriteLine("Unesite godinu veću od 1925 i manju od 2025");
                    birthday = DateTime.MinValue;
                }
            }

            choose_trips(chosen_trips,trips);
           users.Add(id, new Tuple<string, string, DateTime, List<int>>(name, last_name, birthday, new List<int>()));
           Console.WriteLine("Korisnik uspješno dodan.");
            Console.Clear();
       }
       catch (Exception ex)
       {
           Console.WriteLine("Greška: " + ex.Message);
       }

    }
    static int choose_trips(List<int> chosen_trips, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        var input = -1;
        var trip_input = -1;
        Console.WriteLine("Unesi ID putovanja s liste za dodavanje (0 za kraj): ");
        int i = 0;
        for(i=1;i<=trips.Count;i++)
        {
            Console.WriteLine($"{i} - {trips[i].Item1} ({trips[i].Item2.ToShortDateString()})");
        }
        while (input != 0)
        {
            trip_input = Convert.ToInt32(Console.ReadLine());
            if (trip_input != 0 && trips.ContainsKey(trip_input))
                chosen_trips.Add(trip_input);
            else if(trip_input != 0 && trips.ContainsKey(trip_input)==false)
                Console.WriteLine("Pogrešan unos, pokušajte ponovo");
            else
                                input = 0;
        }
        return 0;
    }
    static void trips_choice()
    {

    }
}