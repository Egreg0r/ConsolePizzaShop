using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPizzaShop.Data;

namespace WebPizzaShop.Models
{
    public partial class Pizza: Product
    {
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
                    Guid = Guid.NewGuid(),
                    Price = price,
                    Name = name,
                    Active = active
                });
                db.SaveChanges();
            }
        }


    }
}
