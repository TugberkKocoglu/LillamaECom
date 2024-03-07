using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MultiShop.Models
{
	public class ProductRepository
	{
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }

        ShopConnection context=new ShopConnection();

		int subpageCount = 0;
		public async Task<List<Product>> ProductSelect()
		{
			List<Product> products =await context.Products.ToListAsync();
			return products;
		}

		public  List<Product> ProductSelect(string mainPageName,int mainpageCount,string subpageName,int pageNumber)
		{
			subpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;//veri tabanından aldırdım ajaxta kaç tane gelceni kullanıcı seçsin
			List<Product> products;
			if (mainPageName == "Slider")
			{
                products = context.Products.Where(p => p.StatusID == 1&& p.Active==true).Take(mainpageCount).ToList();
            }



			else if (mainPageName == "New")
			{
				 
				if (subpageName=="Ana")//anasayfa
				{
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
				else
				{
					//altsayfa
					if (pageNumber==0)
					{
						//altsayfa ilk tıklanis
						products=context.Products.Where(p=>p.Active==true).OrderByDescending(p=>p.AddDate).Take(subpageCount).ToList();

					}
					else
					{
						//ajax=daha fazla ürün getircem
						products = context.Products.Where(p=>p.Active==true).OrderByDescending(p => p.AddDate).Skip(pageNumber * subpageCount).Take(subpageCount).ToList();
					}

				}
			}



            else if (mainPageName == "Special")
            {

				if (subpageName=="Ana")
				{
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Take(mainpageCount).ToList();
                }
				else
				{
					if (pageNumber==0)
					{
						products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).Skip(pageNumber * subpageCount).Take(subpageCount).ToList();
					}
				}
               
            }





            else if (mainPageName == "Discounted")
            {
				if (subpageName=="Ana")
				{
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
                }
				else
				{
					if (pageNumber==0)
					{
						products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.Discount).Take(subpageCount).ToList();
					}
					else
					{
						products=context.Products.Where(p=>p.Active==true).OrderByDescending(p=>p.Discount).Skip(pageNumber*subpageCount).Take(subpageCount).ToList();
					}
				}
                
            }





            else if (mainPageName == "Highlighted")
            {
				if (subpageName=="Ana")
				{
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
                }
				else
				{
					if (pageNumber==0)
					{
						products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Skip(pageNumber * subpageCount).Take(subpageCount).ToList();
					}
				}
            }



            else if (mainPageName == "Topseller")
            {
                products = context.Products.Where(p=>p.Active == true).OrderByDescending(p=>p.TopSeller).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Star")
            {
                products = context.Products.Where(p => p.StatusID == 3 && p.Active == true).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "Featured")
            {
                products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).Take(mainpageCount).OrderBy(p=>p.ProductName).ToList();
            }
            else if (mainPageName == "Notable")
            {
                products = context.Products.Where(p => p.StatusID == 5 && p.Active == true).Take(mainpageCount).OrderByDescending(p => p.UnitPrice).OrderBy(p=>p.ProductName).ToList();
            }
            else
			{
				products=context.Products.ToList();
			}
			return products;
		}

		public static bool ProductInsert(Product product)
		{
			try
			{
				using (ShopConnection context= new ShopConnection())
				{
					product.AddDate=DateTime.Now;
					context.Add(product);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<Product?> ProductDetails(int? id)
		{
			Product product = await context.Products.FindAsync(id);
			return product;
		}

		public Product ProductDetails(string mainPageName)
		{
			Product? product = context.Products.FirstOrDefault(p => p.StatusID == 6);//Özel ürün
			return product;
		}

		public static bool ProductUpdate(Product product)
		{
			try
			{
				using (ShopConnection context=new ShopConnection())
				{
					context.Update(product);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{

				return false;
			}
		}

		public static bool ProductDelete(int id)
		{
			try
			{
				using (ShopConnection context=new ShopConnection())
				{
					Product product = context.Products.FirstOrDefault(p => p.ProductID == id);
					product.Active = false;

					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{

				return false;
			}
		}

		public static void Highlighted_Increase(int id)
		{
			using (ShopConnection context= new ShopConnection())
			{
				Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
				product.HighLighted += 1;
				context.Update(product);
				context.SaveChanges();
			}
		}

		public List<Product> ProductSelectWithCategoryID(int id)
		{
			List<Product> products = context.Products.Where(p=>p.CategoryID==id).OrderBy(p=>p.ProductName).ToList();
			return products;
		}
		
		public List<Product> ProductSelectWithSupplierID(int id)
		{
			List<Product> products = context.Products.Where(p => p.SupplierID == id).OrderBy(p => p.ProductName).ToList();
			return products;
		}

		public List<ProductRepository> SelectProductsByDetails(string query)
		{
			List<ProductRepository> products=new List<ProductRepository> ();
			SqlConnection sqlConnection = Connection.ServerConnect ;
			SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
			sqlConnection.Open ();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader ();
			while (sqlDataReader.Read ())
			{
				ProductRepository product = new ProductRepository ();
				product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
				product.ProductName = sqlDataReader["ProductName"].ToString();
				product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
				product.PhotoPath = sqlDataReader["PhotoPath"].ToString ();
				products.Add (product);
			}
			return products;
		}

		public static List<Sp_Arama> gettingSearchProduct(string id)
		{
			using (ShopConnection context= new ShopConnection ())
			{
				var products = context.sp_Aramas.FromSql($"Sp_Arama {id}").ToList();
				return products;
			}
		}


    }
}
