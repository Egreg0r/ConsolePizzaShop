using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePizzaShop
{
    public partial class BaseContent
    {
        /// <summary>
        /// Получение набора не оплаченных чеков
        /// </summary>
        /// <param name="id">Client.Id</param>
        /// <returns>ICollection<Check></returns>
        public ICollection<Check> GetNoPaidClientChecks(int id)
        {
            var check = from c in Checks
                        where c.ClientId == id && c.Paid == false
                        select c;
            return check.ToList();
        }

        /// <summary>
        /// Получение даты наиболее раннего не оплаченного чека Клиента
        /// </summary>
        /// <param name="id">Client.Id</param>
        /// <returns></returns>
        public DateTime GetMinDateNoPaidCheck(int id)
        {
            using (var db = new BaseContent())
            {
                var date = from c in Checks
                           where c.ClientId == id && c.Paid == false
                           select c.CreateDate;
                if (date.Any())
                    return date.Min();
                else return DateTime.Now;
            }
        }
    }

    public class CheckNew : Check
    {
        /// <summary>
        /// Генерация нового заказа
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pizzasId"></param>
        /// <param name="isPaid"></param>
        public void CreateCheck(Client client, ICollection<Pizza> pizzas, bool isPaid = false )
        {
                Console.WriteLine("Client " + client.Name);
                //Проверка на не пустой список заказа
                if (!pizzas.Any())
                {
                    Console.WriteLine("Вы ничего не заказали");
                    return;
                }
                Console.WriteLine("Клиент {0} заказывает {1}", client.Name, string.Join(", ",pizzas.Select(p => p.Name).ToArray()));
                
            //Производим заказ если клиент может это сделать. 
                if (client.CanPaid(client.Id))
                {
                    AddCheck(client, pizzas, paid: isPaid);
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
        public void AddCheck(Client client, ICollection<Pizza> pizza, string adress = "Самовывоз", bool paid = false)
        {
            using (var db = new BaseContent())
            {
                DateTime payDate = DateTime.Parse("01.01.2021");
                if (paid == true)
                    payDate = DateTime.Now;

                Check check = (new Check
                {
                    Guid = Guid.NewGuid(),
                    Client = client,
                    CreateDate = DateTime.Now,
                    Paid = paid,
                    CloseDate = payDate,
                    Adress = adress

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


    }

    public partial class Client
    {
        /// <summary>
        /// Возвращает список неоплаченных счетов
        /// </summary>
        public void GetClientNoPaidCheck(int clienId)
        {
            using (var db = new BaseContent())
            {
                var check = db.Checks.Include(c => c.Orders).Where(cl => cl.Client.Id == clienId).AsNoTracking();
                // Если нет чеков то выход
                if (!check.Any())
                {
                    Console.WriteLine("Неоплаченных чеков нет");
                    return;
                }
                int sum = 0;
                foreach (var p in check)
                {
                    int sumord = 0;
                    foreach (var k in p.Orders)
                    {
                        sumord = sumord + k.Pizza.Price;
                    }
                    sum = sum + sumord;
                    Console.WriteLine("Клиент {0} заказал {1} пицу(ы). Дата заказа {2}. Сумма: {3} ", p.Client.Name, p.Orders.Count(), p.CreateDate, sumord);
                }
                Console.WriteLine("Клиент {0}. Общая задолженность {1} пицц на сумму {2}", check.First().Client.Name,  check, sum);
            }

        }

            /// <summary>
            /// Проверка на просроченный чек.
            /// </summary>
            /// <param name="id">Client.id</param>
            /// <returns></returns>
            public bool CanPaid(int id)
            {
                bool l;
                using (var db = new BaseContent())
                {
                    var create = db.GetMinDateNoPaidCheck(id);
                    Console.WriteLine("Первый не оплаченный чек " + create.ToString());
                    if (create < (DateTime.Now.AddDays(-7)))
                        l = false;
                    else l = true;
                }
                return l;
            }

    }

}
