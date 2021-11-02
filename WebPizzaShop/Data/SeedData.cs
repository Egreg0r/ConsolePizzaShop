using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebPizzaShop.Models;

// ***************************
// Семена Базы
// *************************

namespace WebPizzaShop.Data
{
    public static class SeedData
    {
        public static void Initialize()
        {
            var db = new BaseContent();
            //db.Database.EnsureDeleted();
            // создаем базу данных
            //db.Database.EnsureCreated();

            if (db.Pizzas.Any())
                    return; // DB has been seeded

            Console.WriteLine("Insert Seed in empty bd");


            Pizza pizza = new Pizza();
            pizza.AddPizza(111, "Тестовая пицца за 111");
            pizza.AddPizza(250, "Тестовая пицца за 250");
            Console.WriteLine("Add pizza compleeted");

            Client client = new Client();
            client.AddClient("Тест норм клиент", "roga@mail.ru");
            client.AddClient("Тест должник", "everydebtor@mail.ru", DateTime.Parse("01.01.2021"), true);
            Console.WriteLine("Add Client compleeted");

            var check = new Check();
            List<Pizza> pizzas;

            db = new BaseContent();
            pizzas = new List<Pizza>();
            pizzas.Add(db.Pizzas.Find(1));
            pizzas.Add(db.Pizzas.Find(2));
            client = db.Clients.Find(1);
            check.CreateCheck(db, client, pizzas);

            db = new BaseContent();
            pizzas = new List<Pizza>();
            pizzas.Add(db.Pizzas.Find(1));
            pizzas.Add(db.Pizzas.Find(1));
            client = db.Clients.Find(2);
            check.CreateCheck(db, client, pizzas);
            Console.WriteLine("Seed is insert completed");

            db = new BaseContent();
            pizzas = new List<Pizza>();
            pizzas.Add(db.Pizzas.Find(2));
            pizzas.Add(db.Pizzas.Find(2));
            client = db.Clients.Find(1);
            check.CreateCheck(db, client, pizzas);


        }


    }
}
