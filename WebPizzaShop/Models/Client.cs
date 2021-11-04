using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebPizzaShop.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;


namespace WebPizzaShop.Models
{
    //--------------------------------
    // Класс клиентов пицерии
    //--------------------------------

    public class Client : BaseId
    {
        [Required(ErrorMessage = "Введите имя клиента"), Display(Name ="Имя клиента")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required, Display(Name = "Дата регистраци")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime RegistrDate { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
    

        /// <summary>
        /// регистрирует клиента в таблицу Client
        /// </summary>
        /// <param name="name"></param>
        /// <param name="adress"></param>
        /// <param name="email"></param>
        /// <param name="active"></param>

        public void AddClient(string name, string email)
        {
            using (var db = new BaseContent())
            {
                Client client = new Client
                {
                    Name = name,
                    Email = email,
                    RegistrDate = DateTime.Now,
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
        public void AddClient(string name, string email, DateTime regdate)
        {
            using (var db = new BaseContent())
            {
                db.Clients.Add(new Client
                {
                    Name = name,
                    Email = email,
                    RegistrDate = regdate,
                });
                db.SaveChanges();
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



