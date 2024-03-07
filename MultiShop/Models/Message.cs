using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.Models
{
    public class Message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        public string? Content { get; set; }
    }
}
