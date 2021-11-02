using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebPizzaShop.Data;

namespace WebPizzaShop.Models
{
    public partial class Check
    {
        /// <summary>
        /// Генерация нового заказа
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pizzasId"></param>
        /// <param name="isPaid"></param>
        public void CreateCheck(BaseContent bc, Client client, ICollection<Pizza> pizzas, bool isPaid = false)
        {
            Console.WriteLine("Client " + client.Name);
            //Проверка на не пустой список заказа
            if (!pizzas.Any())
            {
                Console.WriteLine("Вы ничего не заказали");
                return;
            }
            Console.WriteLine("Клиент {0} заказывает {1}", client.Name, string.Join(", ", pizzas.Select(p => p.Name).ToArray()));

            //Производим заказ если клиент может это сделать. 
            if (client.CanPaid(client.Id))
            {
                addCheck(bc, client, pizzas, paid: isPaid);
                Console.WriteLine("Заказ оформлен");
            }
            else Console.WriteLine("К сожалению вы должник и не можете заказывать");
        }
        
        /// <summary>
        /// Создание чека. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <param name="pizza"></param>
        /// <param name="paid"></param>
        private void addCheck(BaseContent db, Client client, ICollection<Pizza> pizza, string adress = "Самовывоз", bool paid = false)
        {
            using (db)
            {

                DateTime payDate = DateTime.Parse("01.01.2021");
                if (paid == true)
                    payDate = DateTime.Now;

                Check check = (new Check
                {
                    Guid = Guid.NewGuid(),
                    ClientId = client.Id,
                    Client = client,
                    CreateDate = DateTime.Now,
                    Paid = paid,
                    CloseDate = payDate,
                    Adress = adress,
                });
                db.Checks.Add(check);

                List<Order> orders = new List<Order>();
                foreach (var p in pizza)
                {
                    orders.Add(new Order
                    {
                        Guid = Guid.NewGuid(),
                        Pizza = p,
                        Check = check
                    });
                }
                db.Orders.AddRange(orders);
                db.SaveChanges();
            }
        }
        
        /// <summary>
        /// Проставление признака оплаты чека. 
        /// </summary>
        /// <param name="id">Check.id</param>
        public void SetChecksIsPaid (int id)
        {
            using (var db = new BaseContent())
            {
                Check check = db.Checks.FirstOrDefault(c => c.Id == id);
                if (check!= null)
                {
                    check.Paid = true;
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Оплата всей задолженности клиента. 
        /// </summary>
        /// <param name="clientID"></param>
        public void AllSetChecksPaid(int clientID)
        {
            var client = new Client().GetClientNoPaidCheck(clientID);
            foreach (var p in client)
            {
                SetChecksIsPaid(p.Id);
            }
        }

    }

}
