using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsolePizzaShop
{
    public partial class BaseContent : DbContext
    {
        public string DbPath { get; private set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Pizza> Pizzas{ get; set; }


        //public BaseContent(DbContextOptions<BaseContent> options): base(options)
        public BaseContent()
        {
            DbPath = $"./pizzashop.db";
            //Database.EnsureDeleted();
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
        /// Добавляет новую запись пиццы в таблицу Pizza 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public void AddPizza (int price, string name)
        {
            Pizzas.Add(new Pizza
            {
                Guid = Guid.NewGuid(),
                Price = price,
                Name = name,
            });
            SaveChanges();
        }

        /// <summary>
        /// регистрирует клиента в таблицу Client
        /// </summary>
        /// <param name="name"></param>
        /// <param name="adress"></param>
        /// <param name="email"></param>
        /// <param name="active"></param>

        public void AddClient(string name, string email,  bool active = true)
        {
            Client client = new Client
            {
                Guid = Guid.NewGuid(),
                Name = name,
                Email = email,
                RegistrDate = DateTime.Now,
                Active = active
            };
            Clients.Add(client);
            SaveChanges();
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
            Clients.Add(new Client
            {
                Guid = Guid.NewGuid(),
                Name = name,
                Email = email,
                RegistrDate = regdate,
                Active = active
            });
            SaveChanges();
        }

        /// <summary>
        /// Создание чека. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="client"></param>
        /// <param name="pizza"></param>
        /// <param name="paid"></param>
        public void AddCheck(Client client, ICollection<Pizza> pizza , string adress = "Самовывоз", bool paid = false )
        {
            DateTime payDate = DateTime.Parse("01.01.2021");
            if (paid == true) 
                payDate = DateTime.Now;

            Check check = (new Check
            {
                Guid = Guid.NewGuid(),
                //ClientId = (int)client.Id,
                Client = client,
                CreateDate = DateTime.Now,
                Paid = paid,

                CloseDate = payDate,
                Adress = adress

            });
            Checks.Add(check);

            Order order;
            foreach (var p in pizza)
            {
                order = new Order {Guid=Guid.NewGuid(), Pizza = p, Check = check };
                Orders.Add(order);
            }
            SaveChanges();
        }


    }



    /*
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = @"Server=.;Database=Blogging;Trusted_Connection=True;";
        services.AddDbContext<BaseContetn>(o => o.UseSqlServer(connectionString));

        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    }
    */
}
