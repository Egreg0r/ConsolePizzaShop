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


    }
}
