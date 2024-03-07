using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Components.Forms;

namespace MultiShop.Controllers
{
    public class AdminController : Controller
    {
        UserRepository userRepository= new UserRepository();
        CategoryRepository categoryRepository= new CategoryRepository();
        ShopConnection context= new ShopConnection();
        SupplierRepository supplierRepository= new SupplierRepository();
        StatusRepository statusRepository= new StatusRepository();
        ProductRepository productRepository= new ProductRepository();

	
		public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
		public IActionResult Login(User user)
		{
            User usr = userRepository.LoginControl(user);
            
			if (usr != null)
			{
			    return RedirectToAction(nameof(Index));
			}
            else
            {
                ViewBag.error = "HATALI Giriş";
                return RedirectToAction(nameof(Login));
            }
			
		}
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await categoryRepository.CategorySelect();
            return View(categories);
        }

        [HttpGet]
		public  IActionResult CategoryCreate()
		{
			CategoryFill();
			return View();
		}


		public void CategoryFill()
		{
			List<Category> categories = categoryRepository.CategorySelectMain();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });

		}

        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            bool answer = CategoryRepository.CategoryInsert(category);
            if (answer)
            {
                TempData["Message"] =  category.CategoryName+"Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(CategoryCreate));
            
        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            CategoryFill();
            if (id==null||context.Categories==null)
            {
                return NotFound();
            }
            else
            {
                var category = categoryRepository.CategoryDetails(id);
                return View(category);
            }
        }

        [HttpPost]
        public  IActionResult CategoryEdit(Category category)
        {
            bool answer = CategoryRepository.CategoryUpdate(category);
            if (answer)
            {
                TempData["Message"] = "Güncellendi";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "HATA Güncellenemedi";
                return RedirectToAction(nameof(CategoryEdit));
            }
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int? id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c=>c.CategoryID==id);
            ViewBag.categoryName = category?.CategoryName;
            return View(category);
        }

        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id==null||context.Categories==null)
            {
                return NotFound();
            }
            
            var category = await context.Categories.FirstOrDefaultAsync(c=>c.CategoryID==id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("CategoryDelete")]
        public   IActionResult CategoryDeleteConfirmed(int id)
        {
            bool answer =  CategoryRepository.CategoryDelete(id);
            if (answer)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "HATA silme işlemi gerçekleştirilemedi";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }

        public async Task<IActionResult> SupplierIndex()
        {
            List<Supplier> suppliers = await supplierRepository.SupplierSelect();
            return View(suppliers);
        }

        public IActionResult SupplierCreate()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            bool answer = SupplierRepository.SupplierInsert(supplier);
            if (answer)
            {
                TempData["Message"] = supplier.BrandName + " Markası Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA " + supplier.BrandName + " Eklenemedi"; 
            }
            return RedirectToAction(nameof(SupplierCreate));
            
        }

        [HttpGet]
        public IActionResult SupplierEdit(int id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }
            else
            {
               var supplier= supplierRepository.SupplierDetails(id);
                return View(supplier);
            }
        }
		[HttpPost]
		public IActionResult SupplierEdit(Supplier supplier)
		{
			if (supplier.PhotoPath == null)
			{
				string? PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath;
				supplier.PhotoPath = PhotoPath;
			}

			bool answer = SupplierRepository.SupplierUpdate(supplier);
			if (answer)
			{
				TempData["Message"] = "Güncellendi";
				return RedirectToAction("SupplierIndex");
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(SupplierEdit));
			}
		}

        public async Task<IActionResult> SupplierDetails(int id)
        {
            Supplier? supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);
            ViewBag.brandName = supplier?.BrandName;
            return View(supplier);
        }

        public async Task<IActionResult> SupplierDelete(int? id) 
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }


        [HttpPost, ActionName("SupplierDelete")]
        public async Task<IActionResult> SupplierDeleteConfirmed(int id)
        {
            bool answer = SupplierRepository.SupplierDelete(id);
            if (answer)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(SupplierDelete));
            }
        }


        public async Task<IActionResult> StatusIndex()
        {
            List<Status> statuses = await statusRepository.StatusSelect();
            return View(statuses);
        }

        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            bool answer = StatusRepository.StatusInsert(status);
            if (answer)
            {
                TempData["Message"] =status.StatusName +" Statü Başarıyla Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA " + status.StatusName + " Eklenemdi";
            }
            return RedirectToAction(nameof(StatusCreate));
        }

        public async Task<IActionResult> StatusEdit(int? id)
        {
            if (id == null || context.Statuses==null)
            {
                return NotFound();
            }
            var statuses = await statusRepository.StatusDetails(id);
            return View(statuses);
        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {
            bool answer = StatusRepository.StatusUpdate(status);
            if (answer)
            {
                TempData["Message"] = " Başarıyla Güncellendi.";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA";
                return RedirectToAction(nameof(StatusEdit));
            }
        }

        public async Task<IActionResult> StatusDelete(int id)
        {
            if (id==null|| context.Statuses==null)
            {
                return NotFound();
            }
            var status = await context.Statuses.FirstOrDefaultAsync(s => s.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }  
            return View(status);
        }

        [HttpPost, ActionName("StatusDelete")]
        public async Task<IActionResult> StatusDeleteConfirmed(int id)
        {
            bool answer = StatusRepository.StatusDelete(id);
            if (answer)
            {
                TempData["Message"] = "Silindi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "HATA Silinemedi";
                return RedirectToAction(nameof(StatusDelete));
            }
        }


        public async Task<IActionResult> ProductIndex()
        {
            List<Product> products = await productRepository.ProductSelect(); 
            return View(products);
        }

        public async  Task<IActionResult> ProductCreate()
        {
			List<Category> categories = await categoryRepository.CategorySelect();
			ViewData["categoryList"] = categories.Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryID.ToString() });

			List<Supplier> suppliers = await supplierRepository.SupplierSelect();
			ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });

			List<Status> statuses = await statusRepository.StatusSelect();
			ViewData["statusList"] = statuses.Select(st => new SelectListItem { Text = st.StatusName, Value = st.StatusID.ToString() });

			return View();
		}
        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            bool answer = ProductRepository.ProductInsert(product);
            if (answer)
            {
                TempData["Message"] = product.ProductName + " Başarıyla Eklendi";
            }
            else
            {
                TempData["Message"] = "HATA";
            }
            return RedirectToAction(nameof(ProductCreate));
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            CategoryFill();
            SupplierFill();
            StatusFill();
            if (id == null|| context.Products==null)
            {
                return NotFound();
            }
            var product = await productRepository.ProductDetails(id);
            return View(product);
        }

		[HttpPost]
		public IActionResult ProductEdit(Product product)
		{
			Product? prd = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID);
			product.AddDate = prd.AddDate;
			product.HighLighted = prd.HighLighted;
			product.TopSeller = prd.TopSeller;

			if (product.PhotoPath == null)
			{
				string? PhotoPath = context.Products.FirstOrDefault(s => s.ProductID == product.ProductID).PhotoPath;
				product.PhotoPath = PhotoPath;
			}
			bool answer = ProductRepository.ProductUpdate(product);
			if (answer)
			{
				TempData["Message"] = "Güncellendi";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(ProductEdit));
			}
		}

		public async void SupplierFill()
        {
           List<Supplier> suppliers = await supplierRepository.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }
        public async void StatusFill()
        {
            List<Status> statuses = await statusRepository.StatusSelect();
            ViewData["statusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(s => s.ProductID == id);
            ViewBag.productname = product?.ProductName;

            return View(product);
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            if (id==null||context.Products==null)
            {
                return NotFound();
            }
            var product=await context.Products.FirstOrDefaultAsync(p=> p.ProductID == id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }

		[HttpPost, ActionName("ProductDelete")]
		public async Task<IActionResult> ProductDeleteConfirmed(int id)
		{
			bool answer = ProductRepository.ProductDelete(id);
			if (answer)
			{
				TempData["Message"] = "Silindi";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["Message"] = "HATA";
				return RedirectToAction(nameof(ProductDelete));// "ProductDelete" de yazabilirdin nameof yazmadan
			}
		}



	}
}
