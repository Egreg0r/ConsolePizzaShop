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
        /// <returns></returns>
        public ICollection<Check> GetNoPaidClientChecks(uint id)
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
        public DateTime GetMinDateNoPaidCheck(uint id)
        {
            var date = from c in Checks
                        where c.ClientId == id && c.Paid == false
                        select c.CreateDate;
            if (date.Any())
                return date.Min();
            else return DateTime.Now ;
        }



    }

    public static class NewOrder 
    {
        /// <summary>
        /// Создание чека.
        /// </summary>
        public static void CreateCheck()
        {
            using (var db = new BaseContent())
            //using (var db = baseContent) 
            {
                var client = db.Clients.Find((uint)1);

                Console.WriteLine("Client " + client.Name);

                var piz = from p in db.Pizzas
                          where p.Id == 1
                          select p;
                //Проверка на наличие найденных пицц
                if (piz != null)
                {
                    foreach (var p in piz)
                    {
                        Console.WriteLine("Заказывает {0} {1} {2} ", p.Name, p.Price, p.Id);
                    }

                    if (CanPaid(client.Id) && piz.Any())
                        db.AddCheck(client, piz.ToList(), paid: false);
                    else Console.WriteLine("К сожалению вы должник и не можете заказывать");
                    

                    var check = db.Checks.Include(c => c.Orders);
                    int sum = 0;
                    foreach (var p in check)
                    {
                        foreach (var k in p.Orders)
                        {
                            sum = sum + k.Pizza.Price;
                        }
                        Console.WriteLine("Клиент {0} заказал {1} пицц на сумму {2}", p.Client.Name, p.Orders.Count(), sum);
                    }

                }
                else Console.WriteLine("К сожалению не удалось найти указанные пиццы в меню");
            }
        }

        /// <summary>
        /// Сhecks client can bay
        /// </summary>
        /// <param name="id">Client.id</param>
        /// <returns></returns>
        public static bool CanPaid(uint id)
        {
            bool l;
            using (var db = new BaseContent())
            {
                    var create = db.GetMinDateNoPaidCheck(id);
                    if (create < (DateTime.Now.AddDays(-7)))
                        l = false;
                    else l = true;

            }
            return l;
        }


    }

}
