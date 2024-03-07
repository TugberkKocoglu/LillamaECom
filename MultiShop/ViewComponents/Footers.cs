using MultiShop.Models;
using Microsoft.AspNetCore.Mvc;


namespace iakademi38_proje.ViewComponents
{
	public class Footers : ViewComponent
	{
		ShopConnection context = new ShopConnection();

		public IViewComponentResult Invoke()
		{
			List<Supplier> suppliers = context.Suppliers.ToList();
			return View(suppliers);
		}
	}
}
