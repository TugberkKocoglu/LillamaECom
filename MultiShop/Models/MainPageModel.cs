namespace MultiShop.Models
{
    public class MainPageModel
    {
        public List<Product> SliderProducts { get; set; } //slider ürünler  statusid=1

        public Product Productofday { get; set; } //tek ürün olduğu için list tipinde değil statusid=6

        public List<Product> NewProducts { get; set; } // Yeni ürünler orderByDesending Adddate

        public List<Product> SpecialProducts { get; set; } //Özel Ürünler statusid=2

        public List<Product> DiscountedProducts { get; set; } // İndirimli ürünler orderByDesending discount

        public List<Product> HighlightedProducts { get; set; } //öne çıkan ürünler 

        public List<Product> TopsellerProducts { get; set; } //çok satanlar

        public List<Product> StarProducts { get; set; }//yıldızlı ürünler statysid=3

        public List<Product> FeaturedProducts { get; set; } //Fırsat ürünler statusid=4

        public List<Product> NotableProducts { get; set; }//Dikkat Çeken Ürünler statusid=5


        //Detay sayfasında Gözükecek olanlar 
        public Product? ProductDetails { get; set; }

        public string? CategoryName { get; set; }

        public string? BrandName { get; set; }

        public List<Product>? RelatedProducts { get; set; }
    }
}
