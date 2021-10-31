using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace ConsolePizzaShop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var db = new BaseContent())
            {
                // This sample requires the database to be created before running.
                Console.WriteLine($"Database path: {db.DbPath}");
                // Create
                SeedData.Initialize();
            }

            var order = new NewOrder();
            order.CreateOrder();

        }

    }
}
