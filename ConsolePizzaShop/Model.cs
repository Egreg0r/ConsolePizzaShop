using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;


namespace ConsolePizzaShop
{
    /*
    #region abstact class
    
    public abstract class Product
    {
        public Guid Guid { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public abstract class User
    {
        public Guid Guid { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    #endregion
    */

    // ------------------------
    // Чеки
    // ------------------------

    // Позиция в чеке
    [Index("Guid", IsUnique = true)]
    public class Order
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public int CheckId { get; set; }
        public Check Check { get; set; }
        [Required]
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }
    }

    // чек с покупки. 
    [Index("Guid", IsUnique=true)]
    public class Check
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<Order> Orders { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public bool Paid { get; set; }
        public DateTime CloseDate { get; set; }
        [Required]
        public string Adress { get; set; }

    }

    //--------------------------------
    // Ассортимент пицерии
    //--------------------------------

    [Index("Guid", IsUnique = true)]
    public class Pizza
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    //ассортимент 
    public class Menu : Pizza
    {
        [Required]
        public bool Active { get; set; }
    }



    //--------------------------------
    // Класс клиентов пицерии
    //--------------------------------

    [Index("Guid", IsUnique = true)]
    public class Client
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime RegistrDate { get; set; }
        public ICollection<Check> Checks { get; set; }
    }


    class Model
    {
    }
}
