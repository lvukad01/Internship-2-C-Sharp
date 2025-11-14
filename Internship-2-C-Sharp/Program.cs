using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
class Program
{
    static void Main(string[] args)
    {

        var trips = new Dictionary<int, Tuple<string, DateTime, double, double, double, double>>()
        {
            { 1,new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Split", new DateTime(2024, 7, 15), 410.0, 75, 1.5, Whole_price(75, 1.5) ) },
            { 2, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Dubrovnik", new DateTime(2024, 8, 10), 600.0, 36, 2, Whole_price(36, 2) ) },
            { 3, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Rijeka", new DateTime(2024, 9, 5), 160.0, 30, 1.75, Whole_price(7, 30.0) ) },
            { 4,new Tuple<string, DateTime, double, double, double, double>( "Split - Livno", new DateTime(2025, 11, 5), 100, 25, 1, Whole_price(25,1) )  },
            { 5,new Tuple<string, DateTime, double, double, double, double>( "Split - Bologna", new DateTime(2023, 11,12 ), 782,150 , 2, Whole_price(150,2) )  }
        };
        var users = new Dictionary<int, Tuple<string, string, DateTime, List<int>>>()
        {
            { 1, new Tuple<string, string, DateTime,List<int>>("Nika","Vukadin", new DateTime(1990, 1, 1),new List<int>{ 1, 5 }) },
            { 2, new Tuple<string, string, DateTime,List<int>>("Lana","Vukadin", new DateTime(1992, 2, 2), new List<int>{ 4 }) },
            { 3, new Tuple<string, string, DateTime,List<int>>("Marko","Petric", new DateTime(1995, 3, 3), new List<int>{ 2,4 }) }
        };
        var input = -1;

        while (input != 0)
        {
            Console.Clear();
            Console.WriteLine("\n\t\t\t\t\tAPLIKACIJA ZA EVIDENCIJU GORIVA\n");

            Console.WriteLine("1 - Korisnici\n2 - Putovanja\n0 - Izlaz iz aplikacije");
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Neispravan unos, unesite broj:");
            }
            switch (input)
            {
                case 1:
                    Choice_users(users, trips);
                    break;
                case 2:
                    Trips_choice(users, trips);
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
        Thread.Sleep(2000);
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
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Neispravan unos, unesite broj:");
            }
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
        int id = users.Keys.Max() + 1;
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
                    continue;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Greška: " + ex.Message);
            return;
        }

        Choose_trips(chosen_trips, trips);
        users.Add(id, new Tuple<string, string, DateTime, List<int>>(name, last_name, birthday, chosen_trips));
        Console.WriteLine("Korisnik uspješno dodan.");
        Thread.Sleep(2000);
        Console.Clear();

    }
    static void Choose_trips(List<int> chosen_trips, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        var input = -1;
        Console.WriteLine("Unesi ID putovanja s liste za dodavanje (0 za kraj): ");
        int i = 0;
        foreach (var trip in trips)
        {
            Console.WriteLine($"{trip.Key} - {trip.Value.Item1} ({trip.Value.Item2.ToString("yyyy-mm-dd")})");
        }
        while (input != 0)
        {
while(!int.TryParse(Console.ReadLine(), out input))
{
    Console.WriteLine("Neispravan unos, unesite ID putovanja:");
    input = -1;
}            if (input != 0 && trips.ContainsKey(input)&&!chosen_trips.Contains(input))
                chosen_trips.Add(input);
            else if (input != 0 && trips.ContainsKey(input) == false)
                Console.WriteLine("Pogrešan unos, pokušajte ponovo");
            else if (input == 0 && chosen_trips.Count == 0)
            {
                Console.WriteLine("Morate unijeti barem  jedno putovanje");
                input = -1;
            }
            else
                input = 0;
            
        }
    }
    static void User_remove(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = '0';
        Console.WriteLine("Brisanje po:\na)ID-u\nb)Imenu i Prezimenu\n");
        while (input == '0')
        {
            input = char.Parse(Console.ReadLine());
            input=char.ToLower(input);
            if (input != 'a' && input != 'b')
            {
                Console.WriteLine("Krivi unos, unesite opciju a ili b kako biste nastavili");
                input = '0';

            }


        }
        if (input == 'a')
            User_remove_id(users);
        else
            User_remove_name(users);

    }
    static void User_remove_id(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        int user_id = 0;
        Console.WriteLine("Upišite ID korisnika kojeg želite maknuti:");
        while (!int.TryParse(Console.ReadLine(), out user_id))
        {
            Console.WriteLine("Neispravan unos, pokušajte ponovo:");
        }

        if (users.ContainsKey(user_id))
        {
            Console.Write($"Jeste li sigurni da želite izbrisati korisnika {users[user_id].Item1} {users[user_id].Item2}? (y/n)");
            string confirm = Console.ReadLine().ToLower();
            if (confirm == "y")
            {
                users.Remove(user_id);
                Console.WriteLine("Korisnik uspješno izbrisan.");
            }
            else
            {
                Console.WriteLine("Brisanje otkazano.");
            }
        }
        else
        {
            Console.WriteLine("Korisnik s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
    }
    static void User_remove_name(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {

        Console.Write("Unesite ime i prezime korisnika kojeg zelite izbrisati(Ime Prezime),za odustajanje unesite 0: ");
        string user_name = Console.ReadLine();
        if (user_name == "0")
        {
            Console.WriteLine("Povratak u izbornik korisnika\n");
            Thread.Sleep(2000);
            return;
        }
        string names = null;
        int key_delete = 0;
        foreach (var user in users)
        {
            names = user.Value.Item1 + " " + user.Value.Item2;
            if (string.Equals(names, user_name, StringComparison.OrdinalIgnoreCase))
            {
                key_delete = user.Key;
                Console.WriteLine($"Jeste li sigurni da želite izbrisati korisnika {users[key_delete].Item1} {users[key_delete].Item2}? (y/n)");
                string confirm = Console.ReadLine().ToLower();
                if (confirm == "y")
                {
                    users.Remove(key_delete);
                    Console.WriteLine("Korisnik izbrisan, povratak u izbornik korisnik");
                }
                else
                {
                    Console.WriteLine("Brisanje otkazano.");
                }
                return;
            }

        }
        if (key_delete == 0)
        {
            Console.WriteLine("Osoba nije pronađena");
            Thread.Sleep(2000);
            return;
        }
        Thread.Sleep(2000);
        return ;
    }
    static void User_edit(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Unesite ID korisnika kojeg želite urediti:");
        int user_id = 0;
        while (!int.TryParse(Console.ReadLine(), out user_id))
        {
            Console.WriteLine("Neispravan unos, pokušajte ponovo:");
        }
        char input = '0';
        string name = null, last_name = null;
        DateTime birthday = DateTime.MinValue;
        List<int> chosen_trips = new List<int>();

        if (users.ContainsKey(user_id))
        {
            Console.WriteLine($"Jeste li sigurni da želite urediti korisnika {users[user_id].Item1} {users[user_id].Item2}? (y/n)");
            char confirm = char.Parse(Console.ReadLine());
            confirm = char.ToLower(confirm);
            if (confirm != 'y')
            {
                Console.WriteLine("Uređivanje otkazano.");
                Thread.Sleep(2000);
                return;
            }

            Console.WriteLine("Odaberite šta želite promijeniti:\r\n\ra)Ime\r\n\rb)Prezime\r\n\rc)Datum rođenja\r\n\rd)Listu putovanja\r\n\re)Sve od navedeno\r\n\r0)Odustani");
            while (input == '0')
            {
                input = char.Parse(Console.ReadLine());
                input=char.ToLower(input);
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
                        break;
                    default:
                        Console.WriteLine("Pogrešan unos, pokušajte ponovo");
                        break;

                }
            }
            Console.WriteLine("Promjene uspješno spremljene");

        }
        else
        {
            Console.WriteLine("Korisnik s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
    }
    static void Users_list(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = '0';
        Console.WriteLine("Ispis:\r\n\ra)Svih korisnika abecedno po prezimenu\r\n\rb)Svih onih koji imaju više od 20 godina\r\nc)Svih onih koji imaju barem 2 putovanja\r\n\r");
        while (input == '0')
        {
            input = char.Parse(Console.ReadLine());
            input=char.ToLower(input);
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
    }
    static void Users_list_a(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        Console.WriteLine("Ispis korisnika abecedno po prezimenu:\n");
        Console.WriteLine("(ID-Ime-Prezime-Datum rodjenja)");
        var users1 = users.OrderBy(t=>t.Value.Item2);

        foreach (var user in users1)
        {
            Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{user.Value.Item3.ToString("yyyy-MM-dd")}");
        
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();

    }
    static void Users_list_b(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users)
    {
        Console.WriteLine("Ispis korisnika starijih od 20:\n");
        Console.WriteLine("(ID-Ime-Prezime-Datum rodjenja)");
        DateTime now = DateTime.Now;
        foreach (var user in users)
        {
            DateTime birth = user.Value.Item3;
            int age = now.Year - birth.Year;
            if (birth.Date > now.AddYears(-age)) age--;
            if (age >= 20)
            {
                Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{birth.ToString("yyyy-MM-dd")}");
            }
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
    }
    static void Users_list_c(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
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
    }
    static void Trips_choice(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        var input = -1;
        while (input != 0)
        {
            Console.Clear();

            Console.WriteLine("1 - Unos novog putovanja\r\n2 - Brisanje putovanja\r\n3 - Uređivanje postojećeg putovanja\r\n4 - Pregled svih putovanja\r\n5 - Izvještaji i analize\r\n0 - Povratak na glavni izbornik\r\n");

            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Neispravan unos, unesite broj:");
            }
            switch (input)
            {
                case 1:
                    Trip_input(users, trips);
                    break;
                case 2:
                    Trip_remove(users, trips);
                    break;
                case 3:
                    Trip_edit(trips);
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
    static void Trip_input(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        int trip_id = trips.Keys.Max() + 1;
        string? start = null;
        string? end = null;
        DateTime trip_date = DateTime.MinValue;
        double km = 0;
        double gas = 0;
        double price = 0;
        double full_price = 0;
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
                while (!DateTime.TryParse(Console.ReadLine(), out trip_date))
                {
                    Console.WriteLine("Neispravan datum, unesite u formatu yyyy-MM-dd:");
                }
                if (trip_date.Year < 1925 || trip_date.Year > 2025)
                {
                    Console.WriteLine("Unesite godinu veću od 1925 i manju od 2025");
                    trip_date = DateTime.MinValue;
                }
            }
            Console.Write("Unesi prijeđenu kilometražu: ");
            while (!double.TryParse(Console.ReadLine(), out km))
            {
                Console.WriteLine("Neispravan unos, unesite broj kilometara:");
            }
            Console.Write("Unesi količinu potrošenog goriva(u L) : ");
            while (!double.TryParse(Console.ReadLine(), out gas))
            {
                Console.WriteLine("Neispravan unos, unesite količinu goriva:");
            }
            Console.Write("Unesi cijenu goriva(po L): ");
            while (!double.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Neispravan unos, unesite cijenu goriva:");
            }
            full_price =Whole_price(gas, price);
            trips.Add(trip_id, new Tuple<string, DateTime, double, double, double, double>(destination, trip_date, km, gas, price, full_price));

            choose_user(users, trip_id);
            Console.WriteLine("Putovanje uspješno dodano.");
            Thread.Sleep(2000);
            Console.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Greška: " + ex.Message);
        }

    }
    static void choose_user(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, int trip_id)
    {
        Console.WriteLine("Odaberite korisnika kojem želite dodati putovanje:");
        int input = 0;
       
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Key}- {user.Value.Item1} {user.Value.Item2}");
        }
        while(input==0)
        {
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Neispravan unos, pokušajte ponovo:");
                input = 0;
            }
            if (users.ContainsKey(input))
            {
                users[input].Item4.Add(trip_id);
            }
            else
            {
                Console.WriteLine("Korisnik tog ID-a ne postoji, pokušajte ponovo");
                input = 0;
            }
        }
    }
    static void Trip_remove(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        char input = '0';
        Console.WriteLine(" Brisanje putovanja\r\na)po id-u\r\nb)svih putovanja skupljih od unesenog iznosa\r\nc)svih putovanja jeftinijih od unesenog iznosa\r\n0)Za povratak na izbornik putovanja");
        while (input == '0')
        {
            input = char.Parse(Console.ReadLine());
            input = char.ToLower(input);
            switch (input)
            {
                case 'a':
                    Trip_remove_a(users,trips);
                    break;
                case 'b':
                    Trip_remove_b(users,trips);
                    break;
                case 'c':
                    Trip_remove_c(users, trips);
                    break;
                case '0':
                    Console.WriteLine("Povratak u izbornik putovanja");
                    Thread.Sleep(2000);
                    break;
                default:
                    Console.WriteLine("Pogrešan unos, unesite a,b,c ili 0");
                    break;
            }

        }

    }
    static void Trip_remove_a(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        int trip_id = 0;
        Console.WriteLine("Upišite ID putovanja koje želite maknuti:");
        while (!int.TryParse(Console.ReadLine(), out trip_id))
        {
            Console.WriteLine("Neispravan ID, pokušajte ponovo:");
        }

        if (trips.ContainsKey(trip_id))
        {
            Console.WriteLine($"Jeste li sigurni da želite izbrisati putovanje (#{trip_id} {trips[trip_id].Item1}? (y/n)");
            char confirm = char.Parse(Console.ReadLine());
            confirm=char.ToLower(confirm);
            if (confirm != 'y')
            {
                Console.WriteLine("Brisanje putovanja otkazano.");
                Thread.Sleep(2000);
                return;
            }

            trips.Remove(trip_id);
            foreach (var key in users.Keys.ToList())
            {
                var user = users[key];
                if (user.Item4.Contains(trip_id))
                {
                    var new_trips= user.Item4.Where(t => t != trip_id).ToList();
                    users[key] = new Tuple<string, string, DateTime, List<int>>(user.Item1, user.Item2, user.Item3, new_trips);
                }
            }
            Console.WriteLine("Putovanje uspješno izbrisano");
        }
        else
        {
            Console.WriteLine("Putovanje s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
    }
    static void Trip_remove_b(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        Console.Write("Unesite iznos:");
        double price = 0;
        while (!double.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Neispravan unos, pokušajte ponovo:");
        }
        Console.WriteLine($"Jeste li sigurni da želite izbrisati putovanja skuplja od {price}? (y/n)");
        char confirm = char.Parse(Console.ReadLine());
        confirm = char.ToLower(confirm);
        if (confirm != 'y')
        {
            Console.WriteLine("Brisanje putovanja otkazano.");
            Thread.Sleep(2000);
            return;
        }
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
            foreach (var key in users.Keys.ToList())
            {
                var user = users[key];
                if (user.Item4.Contains(i))
                {
                    var new_trips = user.Item4.Where(t => t != i).ToList();
                    users[key] = new Tuple<string, string, DateTime, List<int>>(user.Item1, user.Item2, user.Item3, new_trips);
                }
            }

        }
        Console.WriteLine($"{list.Count} putovanja izbrisano");
        Thread.Sleep(2000);

    }
    static void Trip_remove_c(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.Clear();
        Console.Write("Unesite iznos:");
        double price = 0;
        while (!double.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Neispravan unos, pokušajte ponovo:");
        }
        Console.WriteLine($"Jeste li sigurni da želite izbrisati putovanja jeftinija od {price}? (y/n)");
        char confirm = char.Parse(Console.ReadLine());
        confirm = char.ToLower(confirm);
        if (confirm != 'y')
        {
            Console.WriteLine("Brisanje putovanja otkazano.");
            Thread.Sleep(2000);
            return;
        }
        List<int> list = new List<int>();
        foreach (var trip in trips)
        {
            if (trip.Value.Item6 < price)
            {
                list.Add(trip.Key);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            trips.Remove(list[i]);
            foreach (var key in users.Keys.ToList())
            {
                var user = users[key];
                if (user.Item4.Contains(i))
                {
                    var new_trips = user.Item4.Where(t => t != i).ToList();
                    users[key] = new Tuple<string, string, DateTime, List<int>>(user.Item1, user.Item2, user.Item3, new_trips);
                }
            }
        }
        Console.WriteLine($"{list.Count} putovanja izbrisano");
        Thread.Sleep(2000);
    }
    static void Trip_edit(Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Unesite ID putovanja kojeg želite urediti:");
        int trip_id = 0;
        while (!int.TryParse(Console.ReadLine(), out trip_id))
        {
            Console.WriteLine("Neispravan ID putovanja, pokušajte ponovo:");
        }
        char input = '0';
        DateTime trip_date = DateTime.MinValue;
        double km = 0, gas = 0, price = 0, full_price = 0;
        List<int> chosen_trips = new List<int>();

        if (trips.ContainsKey(trip_id))
        {
            Console.WriteLine($"Jeste li sigurni da želite urediti putovanje #{trip_id} {trips[trip_id].Item1}? (y/n)");
            char confirm = char.Parse(Console.ReadLine());
            confirm = char.ToLower(confirm);
            if (confirm != 'y')
            {
                Console.WriteLine("Brisanje putovanja otkazano.");
                Thread.Sleep(2000);
                return;
            }

            Console.WriteLine("Odaberite šta želite promijeniti:\r\n\ra)Datum\r\n\rb)Kilometražu\r\n\rc)Gorivo\r\n\rd)Cijenu goriva\r\n\re)Sve od navedeno\r\n\r0)Odustani");
            while (input == '0')
            {
                input = char.Parse(Console.ReadLine());
                input = char.ToLower(input);
                switch (input)
                {

                    case 'a':
                        Console.Write($"Stari datum putovanja: {trips[trip_id].Item2}\nNovi datum putovanja: ");

                        while (!DateTime.TryParse(Console.ReadLine(), out trip_date))
                        {
                            Console.WriteLine("Neispravan datum, unesite u formatu yyyy-MM-dd:");
                        }
                        var updated_trip = new Tuple<string, DateTime, double, double, double, double>(trips[trip_id].Item1, trip_date, trips[trip_id].Item3, trips[trip_id].Item4, trips[trip_id].Item5, trips[trip_id].Item6);
                        trips[trip_id] = updated_trip;

                        break;
                    case 'b':
                        Console.Write($"Stara kilometraža: {trips[trip_id].Item3}\nNova kilometraža: ");
                        while (!double.TryParse(Console.ReadLine(), out km))
                        {
                            Console.WriteLine("Neispravan unos, unesite broj kilometara:");
                        }
                        updated_trip = new Tuple<string, DateTime, double, double, double, double>(trips[trip_id].Item1, trips[trip_id].Item2,km, trips[trip_id].Item4, trips[trip_id].Item5, trips[trip_id].Item6);
                        trips[trip_id] = updated_trip;

                        break;
                    case 'c':
                        Console.Write($"Stara količina potrošenog goriva: {trips[trip_id].Item4} L\nNovi količina: ");
                        while (!double.TryParse(Console.ReadLine(), out gas))
                        {
                            Console.WriteLine("Neispravan unos, unesite količinu goriva:");
                        }
                        full_price = Whole_price(gas, trips[trip_id].Item5);
                        updated_trip = new Tuple<string, DateTime, double, double, double, double>(trips[trip_id].Item1, trips[trip_id].Item2, trips[trip_id].Item3, gas, trips[trip_id].Item5, full_price);
                        trips[trip_id] = updated_trip;

                        break;
                    case 'd':
                        Console.Write($"Stara cijena goriva: {trips[trip_id].Item4} L\nNova cijena: ");
                        while (!double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Neispravan unos, unesite cijenu goriva:");
                        }
                        full_price = Whole_price(trips[trip_id].Item4, price);
                        updated_trip = new Tuple<string, DateTime, double, double, double, double>(trips[trip_id].Item1, trips[trip_id].Item2, trips[trip_id].Item3, trips[trip_id].Item4, price, full_price);
                        trips[trip_id] = updated_trip;

                        break;
                    case 'e':
                        Console.Write($"Stari datum putovanja: {trips[trip_id].Item2}\nStara kilometraža: {trips[trip_id].Item3}\nStara količina potrošenog goriva: {trips[trip_id].Item4} L\nStara cijena goriva: {trips[trip_id].Item5} \n\n");

                        Console.Write("Novi datum putovanja: ");
                        while (!DateTime.TryParse(Console.ReadLine(), out trip_date))
                        {
                            Console.WriteLine("Neispravan datum, unesite u formatu yyyy-MM-dd:");
                        }
                        Console.Write("Nova kilometraža: ");
                        while (!double.TryParse(Console.ReadLine(), out km))
                        {
                            Console.WriteLine("Neispravan unos, unesite broj kilometara:");
                        }
                        Console.Write($"Nova količina potrošenog goriva: ");
                        while (!double.TryParse(Console.ReadLine(), out gas))
                        {
                            Console.WriteLine("Neispravan unos, unesite količinu goriva:");
                        }
                        Console.Write($"Nova cijena: ");
                        while (!double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Neispravan unos, unesite cijenu goriva:");
                        }
                        full_price = Whole_price(gas, price);
                        updated_trip = new Tuple<string, DateTime, double, double, double, double>(trips[trip_id].Item1,trip_date, km, gas, price, full_price);
                        trips[trip_id] = updated_trip;

                        break;
                    case '0':
                        break;
                    default:
                        Console.WriteLine("Pogrešan unos, pokušajte ponovo");
                        break;

                }
            }
            Console.WriteLine("Promjene uspješno spremljene");

        }
        else
        {
            Console.WriteLine("Putovanje s tim ID-om ne postoji");
        }
        Thread.Sleep(2000);
    }
    static void Trips_list(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Pregled svih putovanja\r\na)Sva putovanja redom kako su spremljena\n\r\nb)Sva putovanja sortirana po trošku uzlazno\r\nc)Sva  putovanja sortirana po trošku silazno\r\nd)Sva  putovanja sortirana po kilometraži uzlazno\r\ne)Sva  putovanja sortirana po kilometraži silazno\r\nf)Sva  putovanja sortirana po datumu uzlazno\r\ng)Sva  putovanja sortirana po datumu silazno\r\nf)Sva putovanja\n");
        var trips1=trips.ToList();
        char input = char.Parse(Console.ReadLine());
        input=char.ToLower(input);
        switch (input)
        {
            case 'a':
                trips1 = trips.ToList();
                break;
            case 'b':
               trips1 = trips.OrderBy(t=>t.Value.Item6).ToList();
                
                break;
            case 'c':
                trips1 = trips.OrderByDescending(t => t.Value.Item6).ToList();

                break;
            case 'd':
                trips1 = trips.OrderBy(t => t.Value.Item3).ToList();
                break;
            case 'e':
                trips1 = trips.OrderByDescending(t => t.Value.Item3).ToList();
                break;
            case 'f':
                trips1 = trips.OrderBy(t => t.Value.Item2).ToList();
                break;
            case 'g':
                trips1 = trips.OrderByDescending(t => t.Value.Item2).ToList();
                break;
            default:
                Console.WriteLine("Pogrešan unos\n");
                return;

                
        }
        foreach (var trip in trips1)
        {
            Console.WriteLine($"\n\nPutovanje #{trip.Key}\nPolazište - Odredište: {trip.Value.Item1}\nDatum: {trip.Value.Item2}\nKilometri: {trip.Value.Item3}\nGorivo: {trip.Value.Item4} L\nCijena po litri: {trip.Value.Item5} EUR\n Ukupno: {trip.Value.Item6} EUR");
        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
    }

    


    
    static void Trips_reports(Dictionary<int, Tuple<string, string, DateTime, List<int>>> users, Dictionary<int, Tuple<string, DateTime, double, double, double, double>> trips)
    {
        Console.WriteLine("Popis korisnika:\n");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Key}-{user.Value.Item1}-{user.Value.Item2}-{user.Value.Item3.ToString("yyyy-MM-dd")}");

        }
        Console.WriteLine("Unesite ID korisnika za kojeg želite izvještaj:");
        int id = 0;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Neispravan unos korisnika, pokušajte ponovo:");
        }
        if (users.ContainsKey(id))
        {
            double total_gas = 0;
            double total_price = 0;
            double total_km = 0;
            foreach (int trip_id in users[id].Item4)
            {
                total_km += trips[trip_id].Item3;
                total_gas += trips[trip_id].Item4;
                total_price += trips[trip_id].Item6;
            }
            Console.WriteLine($"Izvjestaj za {users[id].Item1} {users[id].Item2}\n");
            Console.WriteLine("a)Ukupna potrošnja goriva (zbroj svih litara)\r\nb)Ukupni troškovi goriva \r\nc)Prosječna potrošnja goriva u L/100km \r\nd)Putovanje s najvećom potrošnjom goriva\r\ne)Pregled putovanja po određenom datumu\r\n");
            char input=char.Parse(Console.ReadLine());
           
            switch (input)
            {
                case 'a':
                    Console.WriteLine($"Ukupna potrošnja goriva: {total_gas} L");
                    break;
                case 'b':
                    Console.WriteLine($"Ukupni troškovi goriva: {total_price} L");
                    break;
                case 'c':
                    Console.WriteLine($"Prosječna potrošnja goriva u L/100km: {(total_gas/total_km)*100}%");
                    break;
                case 'd':
                    int max_id = -1;
                    double max_gas = -1;

                    foreach (int trip_id in users[id].Item4)
                    {
                        if (trips[trip_id].Item4 > max_gas)
                        {
                            max_gas = trips[trip_id].Item4;
                            max_id= trip_id;
                        }
                    }
                    Console.WriteLine($"Putovanje s najvećom potrošnjom goriva:");
                    Console.WriteLine($"#{max_id} - {trips[max_id].Item1} ({trips[max_id].Item4} L)");

                    break;
                case 'e':
                    Console.WriteLine("Unesite datum (yyyy-mm-dd): ");
                    DateTime date;
                    while (!DateTime.TryParse(Console.ReadLine(), out date))
                    {
                        Console.WriteLine("Pogrešan unos, pokušajte ponovo (yyyy-mm-dd): ");
                    }

                    Console.WriteLine($"\nPutovanja na datum {date.ToShortDateString()}:");

                    bool found = false;

                    foreach (var trip_id in users[id].Item4)
                    {
                        if (trips[trip_id].Item2.Date == date.Date)
                        {
                            found = true;
                            Console.WriteLine($"\n\nPutovanje #{trip_id}\nPolazište - Odredište: {trips[trip_id].Item1}\nDatum: {trips[trip_id].Item2}\nKilometri: {trips[trip_id].Item3}\nGorivo: {trips[trip_id].Item4} L\nCijena po litri: {trips[trip_id].Item5} EUR\n Ukupno: {trips[trip_id].Item6} EUR");
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine("Nema putovanja na taj datum.");
                    }
                    break;
                case 'f':
                    Console.WriteLine("Popis putovanja:");
                    foreach (int trip_id in users[id].Item4)
                    {                        
                            Console.WriteLine($"\n\nPutovanje #{trip_id}\nPolazište - Odredište: {trips[trip_id].Item1}\nDatum: {trips[trip_id].Item2}\nKilometri: {trips[trip_id].Item3}\nGorivo: {trips[trip_id].Item4} L\nCijena po litri: {trips[trip_id].Item5} EUR\n Ukupno: {trips[trip_id].Item6} EUR");

                    }
                    break;
                default:
                    Console.WriteLine("Pogrešan unos");
                    break;
            }


        }
        Console.WriteLine("\n\nPritisnite bilo koju tipku za povratak na glavni izbornik");
        Console.ReadKey();
    }
}
