using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required]
        public string? NameSurname { get; set; }

        [Required]
        [EmailAddress] //Email kontrolü yapan yer
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        [Required]
        [DataType(DataType.Password)]//yıldızlı gözükmesi için
        public string? Password { get; set; } //d375af34cc08aba9a1cc9b6596a70c36  123.nın karşılığı

        public string? Telephone { get; set; }

        public string? InvoicesAddres { get; set; } //fatura Adresi

        public bool IsAdmin { get; set; } //Admin=True ise personel giriş yapıyor,admin paneline girebilir

        public bool Active { get; set; }
    }
}
