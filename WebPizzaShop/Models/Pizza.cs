using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

//--------------------------------
// Ассортимент пицерии
//--------------------------------


namespace WebPizzaShop.Models
{
    public class Pizza : Product
    {
        [Required]
        [Display(Name = "Активна")]
        public bool Active { get; set; }
        


        /// <summary>
        /// Добавляет новую запись пиццы в таблицу Pizza 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public void AddPizza(int price, string name, bool active = true )
        {
            using (var db = new BaseContent())
            {
                db.Pizzas.Add(new Pizza
                {
                    Price = price,
                    Name = name,
                    Active = active,
                    CreateDate = DateTime.Now
                }) ;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение Icollection пицц в чеке
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public static ICollection<Pizza> ListPizzasInCheck(int checkId, BaseContent baseContent)
        {
            ICollection<Pizza> pizzas = new List<Pizza> { };
            using (var db = baseContent)
            {
                var order = db.Orders
                           .Where(ord => ord.CheckId == checkId)
                           .Select(ord => ord.PizzaId)
                           .ToList();
                foreach (var o in order)
                {
                    var piz = db.Pizzas.Where(p => p.Id == o).FirstOrDefault();
                    pizzas.Add(piz);
                }
            }
            return pizzas;
        }



    }
}
