using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml.Linq;
class Program
{
    static void Main(string[] args)
    {

        var trips = new Dictionary<int, Tuple<string, DateTime, double, double, double, double>>()
        {
            { 1,new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Split", new DateTime(2024, 7, 15), 410.0, 25, 50.0, Whole_price(0.08, 50.0) ) },
            { 2, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Dubrovnik", new DateTime(2024, 8, 10), 600.0, 36, 70.0, Whole_price(0.09, 70.0) ) },
            { 3, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Rijeka", new DateTime(2024, 9, 5), 160.0, 7, 30.0, Whole_price(0.07, 30.0) ) }
        };
        var users = new Dictionary<int, Tuple<string, string, DateTime, List<int>>>()
        {
            { 1, new Tuple<string, string, DateTime,List<int>>("Nika","Vukadin", new DateTime(1990, 1, 1),new List<int>{ 1, 2 }) },
            { 2, new Tuple<string, string, DateTime,List<int>>("Ana","Novak", new DateTime(1992, 2, 2), new List<int>{ 2 }) },
            { 3, new Tuple<string, string, DateTime,List<int>>("Marko","Petric", new DateTime(1995, 3, 3), new List<int>{ 3 }) }
        };
        var input = -1;

        while (input != 0)
        {
            Console.Clear();
            Console.WriteLine("\n\t\t\t\t\tAPLIKACIJA ZA EVIDENCIJU GORIVA\n");

            Console.WriteLine("1 - Korisnici\n2 - Putovanja\n0 - Izlaz iz aplikacije");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Choice_users(users, trips);
                    break;
                case 2:
                    Trips_choice(users,trips);
                    break;
                case 0:
                    Console.WriteLine("Izlaz iz aplikacije");
                    input = 0;
                    break;
                default:
                    Console.WriteLine("Pogresan unos, pokusajte ponovo.");
                    Thread.Sleep(2000);
                    break;
            }

        }

    }
    static double Whole_price(double gas, double price)
    {
        return gas * price;
    }
    static void Choice_users(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        var input = -1;
        while (input != 0)
        {
            Console.Clear();
            Console.WriteLine("1 - Unos novog korisnika\r\n2 - Brisanje korisnika\r\n3 - Uređivanje korisnika\r\n4 - Pregled svih korisnika\r\n0 - Povratak na glavni izbornik\r\n");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    User_input(users, trips);
                    break;
                case 2:
                    User_remove(users, trips);
                    break;
                case 3:
                    User_edit(users, trips);
                    break;
                case 4:
                    Users_list(users, trips);
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
    static void User_input(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        int id = users.Count + 1;
        int input = -1;
        string name = null, last_name = null;
        DateTime birthday = DateTime.MinValue;
        List<int> chosen_trips = new List<int>();
        try
        {
            Console.Write("Unesi ime: ");
            name = Console.ReadLine();

            Console.Write("Unesi prezime: ");
            last_name = Console.ReadLine();

            Console.Write("Unesi datum rođenja (yyyy-mm-dd): ");
            while (birthday == DateTime.MinValue)
            {
                birthday = DateTime.Parse(Console.ReadLine());
                if (birthday.Year < 1925 || birthday.Year > 2025)
                {
                    Console.WriteLine("Unesite godinu veću od 1925 i manju od 2025");
                    birthday = DateTime.MinValue;
                }
            }

            Choose_trips(chosen_trips, trips);
            users.Add(id, new Tuple<string, string, DateTime, List<int>>(name, last_name, birthday, new List<int>(chosen_trips)));
            Console.WriteLine("Korisnik uspješno dodan.");
            Thread.Sleep(2000);
            Console.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Greška: " + ex.Message);
        }

    }
    static int Choose_trips(List<int> chosen_trips, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        var input = -1;
        var trip_input = -1;
        Console.WriteLine("Unesi ID putovanja s liste za dodavanje (0 za kraj): ");
        int i = 0;
        for (i = 1; i <= trips.Count; i++)
        {
            Console.WriteLine($"{i} - {trips[i].Item1} ({trips[i].Item2.ToShortDateString()})");
        }
        while (input != 0)
        {
            trip_input = Convert.ToInt32(Console.ReadLine());
            if (trip_input != 0 && trips.ContainsKey(trip_input))
                chosen_trips.Add(trip_input);
            else if (trip_input != 0 && trips.ContainsKey(trip_input) == false)
                Console.WriteLine("Pogrešan unos, pokušajte ponovo");
            else
                input = 0;
        }
        return 0;
    }
    static int User_remove(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = 'o';
        Console.WriteLine("Brisanje po:\na)ID-u\nb)Imenu i Prezimenu\n");
        while (input == 'o')
        {
            input = char.Parse(Console.ReadLine());
            char.ToLower(input);
            if (input != 'a' && input != 'b')
            {
                Console.WriteLine("Krivi unos, unesite opciju a ili b kako biste nastavili");
                input = 'o';

            }


        }
        if (input == 'a')
            User_remove_id(users);
        else
            User_remove_name(users);
        return 0;

    }
    static int User_remove_id(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        int user_id = 0;
        Console.WriteLine("Upišite ID korisnika kojeg želite maknuti:");
        user_id = int.Parse(Console.ReadLine());


        if (users.ContainsKey(user_id))
        {
            users.Remove(user_id);
            Console.WriteLine("Korisnik uspješno izbrisan");
        }
        else
        {
            Console.WriteLine("Korisnik s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
        return 0;
    }
    static int User_remove_name(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {

        Console.Write("Unesite ime i prezime korisnika kojeg zelite izbrisati(Ime Prezime),za odustajanje unesite 0: ");
        string user_name = Console.ReadLine();
        if (user_name == "0")
        {
            Console.WriteLine("Povratak u izbornik korisnika\n");
            Thread.Sleep(2000);
            return 0;
        }
        string names = null;
        int key_delete = 0;
        foreach (var user in users)
        {
            names = user.Value.Item1 + " " + user.Value.Item2;
            if (string.Compare(names, user_name) == 0)
            {
                key_delete = user.Key;
            }

        }
        if (key_delete == 0)
        {
            Console.WriteLine("Osoba nije pronađena");
            Thread.Sleep(2000);
            return 0;
        }
        users.Remove(key_delete);
        Console.WriteLine("Korisnik izbrisan, povratak u izbornik korisnik");
        Thread.Sleep(2000);
        return 0;
    }
    static int User_edit(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Unesite ID korisnika kojeg želite urediti:");
        int user_id = int.Parse(Console.ReadLine());
        char input = '0';
        string name = null, last_name = null;
        DateTime birthday = DateTime.MinValue;
        List<int> chosen_trips = new List<int>();

        if (users.ContainsKey(user_id))
        {
            Console.WriteLine("Odaberite šta želite promijeniti:\r\n\ra)Ime\r\n\rb)Prezime\r\n\rc)Datum rođenja\r\n\rd)Listu putovanja\r\n\re)Sve od navedeno\r\n\r0)Odustani");
            while (input == '0')
            {
                input = char.Parse(Console.ReadLine());
                char.ToLower(input);
                switch (input)
                {

                    case 'a':
                        Console.Write($"Staro ime: {users[user_id].Item1}\nNovo ime: ");
                        name = Console.ReadLine();
                        var updated_user = new Tuple<string, string, DateTime, List<int>>(name, users[user_id].Item2, users[user_id].Item3, users[user_id].Item4);
                        users[user_id] = updated_user;

                        break;
                    case 'b':
                        Console.Write($"Staro prezime: {users[user_id].Item2}\nNovo prezime: ");
                        last_name = Console.ReadLine();
                        updated_user = new Tuple<string, string, DateTime, List<int>>(users[user_id].Item1, last_name, users[user_id].Item3, users[user_id].Item4);
                        users[user_id] = updated_user;

                        break;
                    case 'c':
                        Console.Write($"Stari datum rođenja: {users[user_id].Item3}\nNovi datum rođenja: ");

                        while (birthday == DateTime.MinValue)
                        {
                            birthday = DateTime.Parse(Console.ReadLine());
                            if (birthday.Year < 1925 || birthday.Year > 2025)
                            {
                                Console.Write("Unesite godinu veću od 1925 i manju od 2025:");
                                birthday = DateTime.MinValue;
                            }
                        }
                        updated_user = new Tuple<string, string, DateTime, List<int>>(users[user_id].Item1, users[user_id].Item2, birthday, users[user_id].Item4);
                        users[user_id] = updated_user;

                        break;
                    case 'd':
                        Console.WriteLine("Prijasnji popis putovanja korisnika:");
                        foreach (var trip_id in users[user_id].Item4)
                        {
                            Console.WriteLine($"{trip_id}-{trips[trip_id].Item1}");
                        }
                        Choose_trips(chosen_trips, trips);
                        updated_user = new Tuple<string, string, DateTime, List<int>>(users[user_id].Item1, users[user_id].Item2, users[user_id].Item3, chosen_trips);
                        users[user_id] = updated_user;

                        break;
                    case 'e':
                        Console.Write($"Staro ime: {users[user_id].Item1}\nNovo ime: ");
                        name = Console.ReadLine();

                        Console.Clear();
                        Console.Write($"Staro prezime: {users[user_id].Item2}\nNovo prezime: ");
                        last_name = Console.ReadLine();
                        updated_user = new Tuple<string, string, DateTime, List<int>>(users[user_id].Item1, last_name, users[user_id].Item3, users[user_id].Item4);

                        Console.Clear();
                        Console.Write($"Stari datum rođenja: {users[user_id].Item3}\nNovi datum rođenja: ");

                        while (birthday == DateTime.MinValue)
                        {
                            birthday = DateTime.Parse(Console.ReadLine());
                            if (birthday.Year < 1925 || birthday.Year > 2025)
                            {
                                Console.Write("Unesite godinu veću od 1925 i manju od 2025:");
                                birthday = DateTime.MinValue;
                            }
                        }

                        Console.Clear();
                        Console.WriteLine("Prijasnji popis putovanja:");
                        foreach (var trip_id in users[user_id].Item4)
                        {
                            Console.WriteLine($"{trip_id}-{trips[trip_id].Item1}");
                        }
                        Choose_trips(chosen_trips, trips);
                        updated_user = new Tuple<string, string, DateTime, List<int>>(name, last_name, birthday, chosen_trips);
                        users[user_id] = updated_user;

                        break;
                    case '0':
                        return 0;
                        break;
                    default:
                        Console.WriteLine("Pogrešan unos, pokušajte ponovo");
                        break;

                }
            }
            Console.WriteLine("Promjene uspješno spremljene");
            Thread.Sleep(2000);
            return 0;

        }
        else
        {
            Console.WriteLine("Korisnik s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
        return 0;
    }
    static int Users_list(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = '0';
        Console.WriteLine("Ispis:\r\n\ra)Svih korisnika abecedno po prezimenu\r\n\rb)Svih onih koji imaju više od 20 godina\r\nc)Svih onih koji imaju barem 2 putovanja\r\n\r");
        while (input == '0')
        {
            input = char.Parse(Console.ReadLine());
            char.ToLower(input);
            if (input != 'a' && input != 'b' && input != 'c')
            {
                Console.WriteLine("Krivi unos, unesite opciju a, b ili c kako biste nastavili");
                input = '0';

            }


        }
        if (input == 'a')
            Users_list_a(users);
        else if (input == 'b')
            Users_list_b(users);
        else if (input == 'c')
            Users_list_c(users, trips);
        return 0;
    }
    static int Users_list_a(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        Console.WriteLine("Ispis korisnika abecedno po prezimenu:\n");
        Console.WriteLine("(ID-Ime-Prezime-Datum rodjenja)");
        var temp = users[1];
        int i;
        int j;
        for (i = 1; i <= users.Count; i++)
        {
            for (j = i + 1; j <= users.Count; j++)
            {
                if (string.Compare(users[i].Item2, users[j].Item2) > 0)
                {
                    temp = users[i];
                    users[i] = users[j];
                    users[j] = temp;
                }
            }
        }

        foreach (var user in users)
        {
            Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{user.Value.Item3.ToString("yyyy-MM-dd")}");
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
        return 0;

    }
    static int Users_list_b(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        Console.WriteLine("Ispis korisnika starijih od 20:\n");
        Console.WriteLine("(ID-Ime-Prezime-Datum rodjenja)");
        DateTime now = DateTime.Now;
        foreach (var user in users)
        {
            if (now.Year - user.Value.Item3.Year > 20)
            {
                Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{user.Value.Item3.ToString("yyyy-MM-dd")}");
            }
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
        return 0;
    }
    static int Users_list_c(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Ispis korisnika s minimalno 2 putovanja:\n");
        Console.WriteLine("(ID-Ime-Prezime-Datum rodjenja)");
        foreach (var user in users)
        {
            if (user.Value.Item4.Count >= 2)
            {
                Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{user.Value.Item3.ToString("yyyy-MM-dd")}");
            }
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
        return 0;
    }
    static void Trips_choice(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        var input = -1;
        while (input != 0)
        {
            Console.Clear();

            Console.WriteLine("1 - Unos novog putovanja\r\n2 - Brisanje putovanja\r\n3 - Uređivanje postojećeg putovanja\r\n4 - Pregled svih putovanja\r\n5 - Izvještaji i analize\r\n0 - Povratak na glavni izbornik\r\n");

            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Trip_input(users, trips);
                    break;
                case 2:
                    Trip_remove(users, trips);
                    break;
                case 3:
                    Trip_edit(users, trips);
                    break;
                case 4:
                    Trips_list(users, trips);
                    break;
                case 5:
                    Trips_reports(users, trips);
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
    static int Trip_input(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        int trip_id = trips.Count + 1;
        string? start = null;
        string? end = null;
        DateTime trip_date = DateTime.MinValue;
        double km = 0;
        double gas = 0;
        double price = 0;
        double full_price =0;
        try
        {
            Console.Write("Unesi grad polaska: ");
            start = Console.ReadLine();

            Console.Write("Unesi destinaciju: ");
            end = Console.ReadLine();
            string destination = start + " - " + end;
            Console.Write("Unesi datum polaska (yyyy-mm-dd): ");
            while (trip_date == DateTime.MinValue)
            {
                trip_date = DateTime.Parse(Console.ReadLine());
                if (trip_date.Year < 1925 || trip_date.Year > 2025)
                {
                    Console.WriteLine("Unesite godinu veću od 1925 i manju od 2025");
                    trip_date = DateTime.MinValue;
                }
            }
            Console.Write("Unesi prijeđenu kilometražu: ");
            km = double.Parse(Console.ReadLine());
            Console.Write("Unesi količinu potrošenog goriva(u L) : ");
            gas = double.Parse(Console.ReadLine());
            Console.Write("Unesi cijenu goriva(po L): ");
            price = double.Parse(Console.ReadLine());
            Whole_price(gas,price);
            trips.Add(trip_id, new Tuple<string, DateTime, double,double,double,double>(destination,trip_date,km,gas,price,full_price));
            Console.WriteLine("Putovanje uspješno dodano.");
            Thread.Sleep(2000);
            Console.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Greška: " + ex.Message);
        }



        return 0;
    }
    static int Trip_remove(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = '0';
        Console.WriteLine(" Brisanje putovanja\r\na)po id-u\r\nb)svih putovanja skupljih od unesenog iznosa\r\nc)svih putovanja jeftinijih od unesenog iznosa\r\n0)Za povratak na izbornik putovanja");
        while (input == '0')
        {
            input = char.Parse(Console.ReadLine());
            char.ToLower(input);
           switch(input)
            {
                case 'a':
                     Trip_remove_a(trips);
                    break;
                case 'b':
                    Trip_remove_b(trips);
                    break;
                case 'c':
                    Trip_remove_c(trips);
                    break;
                case '0':
                    Console.WriteLine("Povratak u izbornik putovanja");
                    Thread.Sleep(2000);
                    return 0;
                    break;
                default:
                    Console.WriteLine("Pogrešan unos, unesite a,b,c ili 0");
                    break;
            }

        }
       
            return 0;
    }
    static int Trip_remove_a(Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        int trip_id = 0;
        Console.WriteLine("Upišite ID putovanja koje želite maknuti:");
        trip_id = int.Parse(Console.ReadLine());


        if (trips.ContainsKey(trip_id))
        {
            trips.Remove(trip_id);
            Console.WriteLine("Putovanje uspješno izbrisano");
        }
        else
        {
            Console.WriteLine("Putovanje s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
        return 0;
    }
    static int Trip_remove_b(Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        Console.Write("Unesite iznos:");
        double price=double.Parse(Console.ReadLine());
        List<int> list = new List<int>();
        foreach(var trip in trips)
        {
            if(trip.Value.Item6>price)
            {
                list.Add(trip.Key);
            }
        }
        for(int i=0;i<list.Count;i++)
        {
            trips.Remove(list[i]);
        }
        Console.WriteLine("Putovanja uspješno izbrisana");
        Thread.Sleep(2000);

        return 0;
    }
    static int Trip_remove_c(Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        Console.Write("Unesite iznos:");
        double price = double.Parse(Console.ReadLine());
        List<int> list = new List<int>();
        foreach (var trip in trips)
        {
            if (trip.Value.Item6 > price)
            {
                list.Add(trip.Key);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            trips.Remove(list[i]);
        }
        Console.WriteLine("Putovanja uspješno izbrisana");
        Thread.Sleep(2000);

        return 0;
    }
    static int Trip_edit(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        return 0;
    }
    static int Trips_list(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        return 0;
    }
    static int Trips_reports(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        return 0;
    }
}