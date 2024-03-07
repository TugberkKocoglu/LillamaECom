using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MultiShop.Models
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("STATÜ ID")]
        public int StatusID { get; set; }

        [StringLength(100)]
        [Required]
        public string? StatusName { get; set; }

        public bool Active { get; set; }
    }
}
