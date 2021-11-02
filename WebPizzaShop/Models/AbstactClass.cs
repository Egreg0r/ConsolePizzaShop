using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPizzaShop.Models
{

    #region abstact class

    public abstract class Product : BaseId
    {
        [Required(ErrorMessage = "Введите название продукта")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ведите цену")]
        [Display(Name = "Цена")]
        public int Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        [Display(Name = "Дата создания")]
        [Required(ErrorMessage = "Дата начала продаж не указана")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Дата закрытия")]
        public DateTime CloseDate { get; set; }

    }

    /// <summary>
    ///  Базовый класс генерации ключей в базе.
    /// </summary>

    [Index("Guid", IsUnique = true)]
    public abstract class BaseId
    {
        public BaseId()
        {
            Guid = Guid.NewGuid();
        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [Key]
        public int Id { get; set; }
    }
    #endregion







}
