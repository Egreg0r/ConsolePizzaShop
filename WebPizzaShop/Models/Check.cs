using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebPizzaShop.Models
{
    // ------------------------
    // Чеки
    // ------------------------

    // чек с покупки. 
    public class Check : BaseId
    {
        [Required]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [Required, Display(Name = "Адресс заказа")]
        public string Adress { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        [Required, Display(Name = "Дата заказа")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        [Required, Display(Name = "Оплачен")]
        public bool Paid { get; set; }
        [Display(Name = "Дата оплаты")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CloseDate { get; set; }


        /// <summary>
        /// Генерация нового заказа
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pizzasId"></param>
        /// <param name="isPaid"></param>
        public void CreateCheck(BaseContent db, int clientID, ICollection<Pizza> pizzas, bool isPaid = false)
        {
            using (db)
            {
                Client client = db.Clients.Find(clientID);
                Console.WriteLine("Client " + client.Name);
                //Проверка на не пустой список заказа
                if (!pizzas.Any())
                {
                    Console.WriteLine("Вы ничего не заказали");
                    return;
                }
                Console.WriteLine("Клиент {0} заказывает {1}", client.Name, string.Join(", ", pizzas.Select(p => p.Name).ToArray()));

                //Производим заказ если клиент может это сделать. 
                if (client.CanPaid(clientID))
                {
                    addCheck(db, clientID, pizzas, paid: isPaid);
                    Console.WriteLine("Заказ оформлен");
                }
                else Console.WriteLine("К сожалению вы должник и не можете заказывать");
            }
        }

        /// <summary>
        /// Создание чека. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <param name="pizza"></param>
        /// <param name="paid"></param>
        private void addCheck(BaseContent db, int clientID, ICollection<Pizza> pizza, string adress = "Самовывоз", bool paid = false)
        {
            using (db)
            {
                Check check;
                if (paid == true)
                {
                    var payDate = DateTime.Now;

                    check = (new Check
                    {
                        Client = db.Clients.Find(clientID),
                        CreateDate = DateTime.Now,
                        Paid = paid,
                        CloseDate = payDate,
                        Adress = adress,
                    });
                }
                else
                {
                    check = (new Check
                    {
                        Client = db.Clients.Find(clientID),
                        CreateDate = DateTime.Now,
                        Paid = paid,
                        Adress = adress,
                    });

                }
                db.Checks.Add(check);

                List<Order> orders = new List<Order>();
                foreach (var p in pizza)
                {
                    orders.Add(new Order
                    {
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
        public void SetChecksIsPaid(int id)
        {
            using (var db = new BaseContent())
            {
                Check check = db.Checks.FirstOrDefault(c => c.Id == id);
                if (check != null)
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
        public void AllSetChecksPaid(int clientID, BaseContent baseContent)
        {

            var client = GetClientNoPaidCheck(clientID);
            foreach (var p in client)
            {
                SetChecksIsPaid(p.Id);
            }
        }

        /// <summary>
        /// Суммарная цена покупок в чеке. 
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public int SumCheck(int checkId, BaseContent baseContent)
        {
            using (var db = new BaseContent())
            {
                var check = db.Checks
                    .Include(ch => ch.Orders)
                    .ThenInclude(ord => ord.Pizza)
                    .AsNoTracking()
                    .Where(ch => ch.Id == checkId)
                    .First();
                int prices = check.Orders.Sum(ord => ord.Pizza.Price);
                return prices;
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
                    .Include(c => c.Client)
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
                }
                return check.ToList();
            }


        }

    }
}
