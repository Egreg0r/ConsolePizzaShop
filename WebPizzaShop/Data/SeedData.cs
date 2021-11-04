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
            if (db.Pizzas.Any())
                    return; // DB has been seeded

            Console.WriteLine("Insert Seed in empty bd");


            Pizza pizza = new Pizza();
            pizza.AddPizza(11124, "Тестовая пицца за 100,24");
            pizza.AddPizza(25023, "Тестовая пицца за 250,23");
            pizza.AddPizza(9, "Тестовая пицца за 0,09");

            Client client = new Client();
            client.AddClient("Тест норм клиент", "roga@mail.ru");
            client.AddClient("Тест должник", "everydebtor@mail.ru", DateTime.Parse("01.01.2021"));

            List<int> pizzas;

            db = new BaseContent();
            pizzas = new List<int> {1,2};
            pizzas.Add(2);
            Check.addCheck(db, 1, pizzas);

            db = new BaseContent();
            pizzas = new List<int> {2,3,1 };
            Check.addCheck(db, 2, pizzas);

            db = new BaseContent();
            pizzas = new List<int> {1,1,2,2 };
            Check.addCheck(db, 1, pizzas);


        }


    }
}
