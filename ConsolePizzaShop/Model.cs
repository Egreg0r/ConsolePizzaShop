using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;


namespace ConsolePizzaShop
{

    #region abstact class

    [Index("Guid", IsUnique = true)]
    public abstract class Product
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
    /*
    public abstract class User
    {
        public Guid Guid { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    */

    #endregion


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
        public int Id { get; set; }
        [Required]
        public int CheckId { get; set; }
        public Check Check { get; set; }
        [Required]
        public int PizzaId { get; set; }
        public virtual Pizza Pizza { get; set; }
    }

    // чек с покупки. 
    [Index("Guid", IsUnique=true)]
    public partial class Check
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
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

    public partial class Pizza : Product
    {
        [Required]
        public bool Active { get; set; }

    }


    //--------------------------------
    // Класс клиентов пицерии
    //--------------------------------

    [Index("Guid", IsUnique = true)]
    public partial class Client
    {
        [Required]
        public Guid Guid { get; set; }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime RegistrDate { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
    }


}
