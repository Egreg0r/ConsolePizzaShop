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
    public class Check : IdForBase
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
        public DateTime? CloseDate { get; set; }


        /// <summary>
        /// Создание чека. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <param name="pizza"></param>
        /// <param name="paid"></param>
        public static void addCheck(BaseContent db, int clientID, List<int> pizza, string adress = "Самовывоз", bool paid = false)
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
                        Pizza = db.Pizzas.Find(p),
                        Check = check
                    });
                }
                db.Orders.AddRange(orders);
                //db.SaveChanges();
            }
        }


        /// <summary>
        /// Суммарная цена покупок в чеке. 
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public int SumCheck(int checkId)
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
    }
}
