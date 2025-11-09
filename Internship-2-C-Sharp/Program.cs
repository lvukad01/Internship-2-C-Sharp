using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("\n\t\t\t\t\tAPLIKACIJA ZA EVIDENCIJU GORIVA\n");
        var trips=new Dictionary<double,Tuple< string, DateTime, double, double, double, double>>()
        {
            { 1,new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Split", new DateTime(2024, 7, 15), 410.0, 0.08, 50.0, whole_price(410.0, 0.08, 50.0) ) },
            { 2, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Dubrovnik", new DateTime(2024, 8, 10), 600.0, 0.09, 70.0, whole_price(600.0, 0.09, 70.0) ) },
            { 3, new Tuple<string, DateTime, double, double, double, double>( "Zagreb - Rijeka", new DateTime(2024, 9, 5), 160.0, 0.07, 30.0, whole_price(160.0, 0.07, 30.0) ) }
        };
        var users=new Dictionary<double, Tuple<double,string, string, DateTime,List<double>>>()
        {
            { 1, new Tuple<double,string, string, DateTime,List<double>>(1,"Ivan"," Horvat", new DateTime(1990, 1, 1),new List<double>{ 1, 2 }) },
            { 2, new Tuple<double,string, string, DateTime,List<double>>(2,"Ana"," Novak", new DateTime(1992, 2, 2), new List<double>{ 2 }) },
            { 3, new Tuple<double,string, string, DateTime,List<double>>(3,"Marko"," Petric", new DateTime(1995, 3, 3), new List<double>{ 3 }) }
        };
        var input = -1;
       
        while (input != 0)
        {
            Console.WriteLine("1 - Korisnici\n2 - Putovanja\n0 - Izlaz iz aplikacije");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    choice_users();
                    break;
                case 2:
                    choice_trips();
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
        return (km * gas) + price;
    }
    static void choice_users()
    {

    }
    static void choice_trips()
    {
    }
}