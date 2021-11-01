using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsolePizzaShop
{
    public class BaseContent : DbContext
    {
        public string DbPath { get; private set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }


        //public BaseContent(DbContextOptions<BaseContent> options): base(options)
        public BaseContent()
        {
            DbPath = $"./pizzashop.db";
            // создаем базу данных
            Database.EnsureCreated();

        }

        // The following configures EF to create a Sqlite database file in the
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        /*
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {

        }
        */
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

}