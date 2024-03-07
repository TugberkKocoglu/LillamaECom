using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MultiShop.Models
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }

        [StringLength(100)]
        [Required]
        [DisplayName("Marka Adı")]
        public string? BrandName { get; set; }

        [DisplayName("Resim")]
        public string? PhotoPath { get; set; }


        public bool Active { get; set; }
    }
}
