using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MultiShop.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int CategoryID { get; set; }

        public int ParentID { get; set; }

        [StringLength(50, ErrorMessage = "En fazla 50 Karakter")]
        [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]




        private string? _CategoryName { get; set; }
        //kapsülleme
        [DisplayName("Kategori Adı")]
        public string? CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value?.ToUpper(); }
        }

        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }
    }
}
