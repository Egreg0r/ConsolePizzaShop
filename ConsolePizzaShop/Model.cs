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

    // ------------------------
    // Чеки
    // ------------------------

    // Позиция в чеке
    public class Order
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public int CheckId { get; set; }
        [Required]
        public virtual Check Check{ get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int PizzaId { get; set; }

        [Required]
        public virtual Pizza Pizza { get; set; }


    }

    // чек с покупки. 
    public class Check
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        public virtual ICollection<Order> OrderCollection { get; set; }
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

    public class Pizza : Product
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
    }

    public class Menu : Product
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public uint Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public bool Active { get; set; }
    }



    //--------------------------------
    // Класс клиентов пицерии
    //--------------------------------


    public class Client : User
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
    }


    class Model
    {
    }
}
