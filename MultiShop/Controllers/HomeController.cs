using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Models;
using Newtonsoft.Json;
using PagedList.Core;
using System.Net;

namespace MultiShop.Controllers
{
    public class HomeController : Controller
    {
        MainPageModel mpm = new MainPageModel();
        ProductRepository productRepository = new ProductRepository();
        OrderRepository orderRepository= new OrderRepository();
        ShopConnection context= new ShopConnection();

        int mainpageCount = 0;
        public HomeController()
        {
            this.mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainpageCount;
        }
        public  IActionResult Index()
        {
            mpm.SliderProducts = productRepository.ProductSelect("Slider",mainpageCount,"Ana",0);
            mpm.Productofday = productRepository.ProductDetails("Productofday");//günün ürünü
            mpm.NewProducts = productRepository.ProductSelect("New", mainpageCount, "Ana", 0);//yeni
            mpm.SpecialProducts = productRepository.ProductSelect("Special", mainpageCount, "Ana", 0);//özel 
            mpm.DiscountedProducts = productRepository.ProductSelect("Discounted", mainpageCount, "Ana", 0);//indirimli  
            mpm.HighlightedProducts = productRepository.ProductSelect("Highlighted", mainpageCount, "Ana", 0);//öne çıkan
            mpm.TopsellerProducts = productRepository.ProductSelect("Topseller", mainpageCount, "Ana", 0);//cok satan
            mpm.StarProducts = productRepository.ProductSelect("Star", mainpageCount, "Ana", 0);//Yıldızlı
            mpm.FeaturedProducts = productRepository.ProductSelect("Featured", mainpageCount, "Ana", 0);//Fırsat
            mpm.NotableProducts= productRepository.ProductSelect("Notable", mainpageCount, "Ana", 0);//dikkat çeken

            return View(mpm);
        }

        public IActionResult NewProducts()
        {
            mpm.NewProducts = productRepository.ProductSelect("New", mainpageCount,"New",0);
            return View(mpm);
        }

        public PartialViewResult _PartialNewProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.NewProducts = productRepository.ProductSelect("New", mainpageCount, "New", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = productRepository.ProductSelect("Special", mainpageCount, "Special", 0);
            return View(mpm);
        }

        public PartialViewResult _PartialSpecialProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.SpecialProducts = productRepository.ProductSelect("Special", mainpageCount, "Special", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = productRepository.ProductSelect("Discounted", mainpageCount, "Discounted", 0);
            return View(mpm);
        }

        public PartialViewResult _PartialDiscountedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = productRepository.ProductSelect("Discounted", mainpageCount, "Discounted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = productRepository.ProductSelect("Highlighted", mainpageCount, "Highlighted", 0);
            return View(mpm);
        }

        public PartialViewResult _PartiaHighlightedProducts(string pageno)
        {
            int pagenumber = Convert.ToInt32(pageno);
            mpm.DiscountedProducts = productRepository.ProductSelect("Highlighted", mainpageCount, "Highlighted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult TopsellerProducts(int page=1,int pageSize = 4)
        {
            //PagedList paketi yükledim
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p=>p.TopSeller),page,pageSize);

            return View("TopsellerProducts", model);
        }

        public IActionResult CartProcess(int id)
        {
            ProductRepository.Highlighted_Increase(id);
            orderRepository.ProductID = id;
            orderRepository.Quantity = 1;

            var cookieOptions= new CookieOptions();
            var cookie = Request.Cookies["sepetim"];//tarayıca sepetim isminde çerez mı diye okudum
            if (cookie==null)
            {
                cookieOptions = new CookieOptions();//yoksa yarat dedim
                cookieOptions.Expires = DateTime.Now.AddDays(1);//1 günlük dedim
                cookieOptions.Path= "/";
                orderRepository.MyCart = "";
                orderRepository.AddToMyCart(id.ToString());
                Response.Cookies.Append("sepetim", orderRepository.MyCart, cookieOptions);//ismi sepetim olana mycart propundaki veriyi gönderdim tarayıcıya
                //HttpContext.Session.SetString("Message","Ürün Sepetinize Eklendi"); Sessionla yollama bildirimi
                TempData["Message"] = "Ürün Sepetinize Eklendi";//Bu da kendi mesajım


            }
            else
            {
                orderRepository.MyCart = cookie;//tarayıcıdaki sepet bilgisini propa yolladım
                if (orderRepository.AddToMyCart(id.ToString())==false)
                {
                    //aynı ürün sepette yok eklenecek
                    HttpContext.Response.Cookies.Append("sepetim", orderRepository.MyCart, cookieOptions);
                    cookieOptions.Expires=DateTime.Now.AddDays(1);
                    TempData["Message"] = "Ürün Sepetinize Eklendi";
                }
                else
                {
                    //bu ürün zaten eklenmiş
                    TempData["Message"] = "Bu Ürün Zaten Sepetinizde Var";
                }
            }

            //Hangi sayfadan ürün eklendiyse o sayfada kalsın
            string url = Request.Headers["Referer"].ToString(); //https://localhost:7138/Home/SpecialProducts
            return Redirect(url);
        }

        public async Task<IActionResult> Details(int id)
        {
            //ProductRepository.Highlighted_Increase(id);

           // mpm.ProductDetails = await productRepository.ProductDetails(id);


            //LINQ İLE 
            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();
            
            mpm.CategoryName =  (from p in context.Products
                                join c in context.Categories
                                on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();
            mpm.BrandName =  (from p in context.Products
                             join s in context.Suppliers
                             on p.SupplierID equals s.SupplierID
                             where p.ProductID==id 
                             select s.BrandName).FirstOrDefault();
            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();

            return View(mpm);
        }

        //Sağ üst köşeden sepet sayfasına tıklandığında ve aynı sayfada ürünü sil butonuna tıklandığında
        public IActionResult Cart()
        {
            List<OrderRepository> sepet;
            if (HttpContext.Request.Query["scid"].ToString()!="")
            {
                //sil butonuyla geldim
                string? scid = HttpContext.Request.Query["scid"];
                orderRepository.MyCart = Request.Cookies["sepetim"];//tarayıdan alıp prop a yazdım
                orderRepository.DeleteFromMyCart(scid);
                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", orderRepository.MyCart, cookieOptions);
                cookieOptions.Expires=DateTime.Now.AddDays(1);
                TempData["Message"] = "Ürün Sepetten Sİlindi";
                sepet = orderRepository.SelectMyCart();
                ViewBag.Sepetim=sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                //sepet sayfasına git ile geldim
                var cookie = Request.Cookies["sepetim"];
                if (cookie==null)
                {
                    orderRepository.MyCart = "";
                    sepet=orderRepository.SelectMyCart();
                    ViewBag.Sepetim=sepet;
                    ViewBag.sepet_tablo_detay=sepet;
                }
                else
                {
                    var cookieOptions= new CookieOptions();
                    orderRepository.MyCart = Request.Cookies["sepetim"];
                    sepet = orderRepository.SelectMyCart();
                    ViewBag.Sepetim=sepet;
                    ViewBag.sepet_tablo_detay=sepet;
                }
            }
            return View();
        }

        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") !=null)
            {
                User? user = UserRepository.SelectMemberInfo(HttpContext.Session.GetString("Email"));
                return View(user);
            }
            else
            {
                return RedirectToAction("Login"); 
            }
        }
        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            string txt_individual = Request.Form["txt_individual"];
            string txt_corporate = Request.Form["txt_corporate"];

            if (txt_individual!=null)
            {
                orderRepository.tckimlik_vergi_no = txt_individual;
                orderRepository.EfaturaCerate();
            }
            else
            {
                orderRepository.tckimlik_vergi_no = txt_corporate;
                orderRepository.EfaturaCerate();
            }
            string kredikartno = Request.Form["kredikartno"];//IFormCollection kullanarak yakaldım
            string kredikartay = Request.Form["kredikartay"];

            string kredikartyil = frm["kredikartyil"];//text box alanının değerini Iform kullanmadan yakaladım
            string kredikartcvs = frm["kredikartcvs"];

            return RedirectToAction("Backref"); //onaylanmış gibi rp olsun
            //aşağıya da iyzico gibi sanal post kodlaro gelcek !!!!!!
        }

        public IActionResult Backref()
        {
            ConfirmOrder();
            return RedirectToAction("ConfirmPage");
        }

        public static string OrderGroupGUID = "";

        public IActionResult ConfirmOrder()
        {
            //siparişi tabloya kaydetcem , sepeti cookielerden silicem , e-fatura bilgileri digital planete
            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie!=null)
            {
                orderRepository.MyCart = cookie;
                OrderGroupGUID = orderRepository.OrderCreate(HttpContext.Session.GetString("Email").ToString());

                cookieOptions.Expires=DateTime.Now.AddDays(1);
                Response.Cookies.Delete("sepetim");
                //buraya sms ve email gönderme metotları yazabilirdim yollardı 
            }
            return RedirectToAction("ConfirmPage");
        }

        public IActionResult ConfirmPage()
        {
            ViewBag.OrderGroupGUID = OrderGroupGUID;
            return View();
        }

        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("Email")!=null)
            {
                List<vw_MyOrders> order = orderRepository.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(order);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //Login session ile oturum acacagim
        public IActionResult Login()
        {
            return View();   
        }

        [HttpPost]
        public IActionResult Login (User user)
        {
            string answer = UserRepository.MemberControl(user);
            if (answer=="error")
            {
                HttpContext.Session.SetString("Mesaj", "Email/Şifre yanlış girildi");
                TempData["Message"] = "Email/Şifre yanlış girildi";
                return View();
            }
            else if (answer=="admin")
            {
                HttpContext.Session.SetString("Email", answer);
                HttpContext.Session.SetString("Admin", answer);
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (UserRepository.loginEmailControl(user)==false)
            {
                bool answer = UserRepository.AddUser(user);
                if (answer)
                {
                    TempData["Message"] = "Kaydedildi";
                    return RedirectToAction("Login");
                }
                TempData["Message"] = "Hata.Tekrar deneyiniz";
            }
            else
            {
                TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");
        }

        public IActionResult CategoryPage(int id)
        {
            List<Product> products = productRepository.ProductSelectWithCategoryID(id);
            return View(products);
        }

        public IActionResult SupplierPage(int id)
        {
            List<Product> products = productRepository.ProductSelectWithSupplierID(id);
            return View(products);
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers=context.Suppliers.ToList();
            return View();
        }

        public IActionResult DProducts(int CategoryID, string[] SupplierID,string price,string IsInStock)
        {
            int count = 0;
            string suppliervalue = "";//1-3-4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count==0)
                {
                    suppliervalue = "SupplierID =" + SupplierID[i];
                    count++; //or yok sadece 1 marka seçilmiş
                }
                else//birden fazla marka seçilmiş 
                {
                    suppliervalue += " or SupplierID=" + SupplierID[i];
                }
            }

            price = price.Replace(" ", ""); // fiyattaki boşluğu temizledim
            string[] PriceArray= price.Split('-');
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];

            string sign = ">"; //ado net için aldım
            if (IsInStock == "0")
            {
                sign = ">=";
            }
            string query = "select * from products where CategoryID = " + CategoryID + " and (" + suppliervalue + ") and (UnitPrice > " + startprice + " and UnitPrice < " + endprice + ") and Stock " + sign + " 0 order by ProductName";

            ViewBag.Products = productRepository.SelectProductsByDetails(query);

            return View();
        }
       
        public PartialViewResult gettingProducts(string id)
        {
            id= id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            List<Sp_Arama> ulist = ProductRepository.gettingSearchProduct(id);
            string json = JsonConvert.SerializeObject(ulist);
            var response = JsonConvert.DeserializeObject<List<Search>>(json);
            return PartialView(response);
        }


    }
}
