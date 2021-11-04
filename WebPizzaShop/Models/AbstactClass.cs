using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPizzaShop.Models
{

    #region abstact class

    public abstract class Product : IdForBase
    {
        [Required(ErrorMessage = "Введите название продукта")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ведите цену")]
        [Range(1,1000000, ErrorMessage ="Недопустимая цена")]
        [Display(Name = "Цена")]
        public int Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        [Display(Name = "Дата создания")]
        [Required(ErrorMessage = "Дата начала продаж не указана")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Дата закрытия")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CloseDate { get; set; }

    }

    /// <summary>
    ///  Базовый класс генерации ключей в базе.
    /// </summary>

    [Index("Guid", IsUnique = true)]
    public abstract class IdForBase
    {
        public IdForBase()
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
