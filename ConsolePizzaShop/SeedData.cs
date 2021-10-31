using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ***************************
// Семена Базы
// *************************

namespace ConsolePizzaShop
{
    public static class SeedData
    {
        public static void Initialize()
        {
            using (var db = new BaseContent())
               // serviceProvider.GetRequiredService<DbContextOptions<BaseContent>>()))
            {
                if (db.Pizza.Any())
                    return; // DB has been seeded
                Console.WriteLine("Insert Seed in empty bd");
                db.AddPizza(111, "Тестовая пицца за 111");
                db.AddPizza(250, "Тестовая пицца за 250");
                db.AddClient("Тест норм клиент", "roga@mail.ru");
                db.AddClient("Тест должник", "everydebtor@mail.ru", DateTime.Parse("01.01.2021"), true);
                Console.WriteLine("Seed is insert completed");
            }
        }


    }
}
