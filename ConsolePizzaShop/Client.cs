using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsolePizzaShop
{
    public partial class Client
    {
        /// <summary>
        /// регистрирует клиента в таблицу Client
        /// </summary>
        /// <param name="name"></param>
        /// <param name="adress"></param>
        /// <param name="email"></param>
        /// <param name="active"></param>

        public void AddClient(string name, string email, bool active = true)
        {
            using (var db = new BaseContent())
            {
                Client client = new Client
                {
                    Guid = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    RegistrDate = DateTime.Now,
                    Active = active
                };
                db.Clients.Add(client);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// регистрация клиента с указаной датой регистрации в таблицу Client
        /// </summary>
        /// <param name="name"></param>
        /// <param name="adress"></param>
        /// <param name="regdate"></param>
        /// <param name="email"></param>
        /// <param name="active"></param>
        public void AddClient(string name, string email, DateTime regdate, bool active = true)
        {
            using (var db = new BaseContent())
            {
                db.Clients.Add(new Client
                {
                    Guid = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    RegistrDate = regdate,
                    Active = active
                });
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Возвращает список неоплаченных счетов
        /// </summary>
        /// <param name=" clienId">Client.id</param>

        public ICollection<Check> GetClientNoPaidCheck(int id)
        {
            using (var db = new BaseContent())
            {
                var check = db.Checks
                    .Include(c => c.Orders) 
                        .ThenInclude(o => o.Pizza)
                    .Include (c => c.Client)
                    .Where(cl => cl.Client.Id == id)
                    .AsNoTracking();
                int sum = 0;
                foreach (var p in check)
                {
                    int sumord = 0;
                    foreach (var k in p.Orders)
                    {
                        if (p.Id == k.CheckId)
                            sumord = sumord + k.Pizza.Price;
                    }
                    sum = sum + sumord;
                    Console.WriteLine("Клиент {0} заказал {1} пицу(ы). Дата заказа {2}. Сумма: {3} ", p.Client.Name, p.Orders.Count(), p.CreateDate, sumord);
                }
                Console.WriteLine("Клиент {0}. Общая задолженность на сумму {1}", check.First().Client.Name, sum);
                return check.ToList();
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
