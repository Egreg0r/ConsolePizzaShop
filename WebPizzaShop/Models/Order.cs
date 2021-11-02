using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

// ****************************
// Позиция в чеке
//****************************

namespace WebPizzaShop.Models
{
    public class Order : BaseId
    {
        [Required]
        public int CheckId { get; set; }
        public Check Check { get; set; }
        [Required]
        public int PizzaId { get; set; }
        public virtual Pizza Pizza { get; set; }
    }

}
