using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MultiShop.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int ProductID { get; set; }

        [DisplayName("Ad")]
        [StringLength(100)]
        [Required(ErrorMessage = "Ürün Adı Zorunlu Alan")]
        public string? ProductName { get; set; }

        [DisplayName("Fiyat")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Kategori")]
        public int CategoryID { get; set; }

        [DisplayName("Marka")]
        public int SupplierID { get; set; }

        [DisplayName("Stok")]
        public int Stock { get; set; }

        [DisplayName("İnd")]
        public int Discount { get; set; }

        [DisplayName("Statü")]
        public int StatusID { get; set; }

        public DateTime AddDate { get; set; }

        [DisplayName("Anahtar")]
        public string? Keywords { get; set; } //Ürünle ilgili anahtar kelimeler



        //encapsulation(kapsülleme)

        private int _Kdv { get; set; }

        public int Kdv
        {
            get { return _Kdv; }
            set
            {
                _Kdv = Math.Abs(value);
            }
        }


        public int HighLighted { get; set; }//Öne Çıkanlar

        public int TopSeller { get; set; }//Cok Satanlar

        [DisplayName("Bakan")]
        public int Related { get; set; } //Buna Bakanlar

        [DisplayName("Not")]
        public string? Notes { get; set; }

        [DisplayName("Resim")]
        public string? PhotoPath { get; set; }

        [DisplayName("A/P")]
        public bool Active { get; set; }
    }
}
