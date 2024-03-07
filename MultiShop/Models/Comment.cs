using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.Models
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        [StringLength(150)]
        public string? Review { get; set; }
    }
}
