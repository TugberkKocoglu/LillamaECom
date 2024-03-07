using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;

namespace MultiShop.ViewComponents
{
    public class Menus:ViewComponent
    {
        ShopConnection context = new ShopConnection();

        public IViewComponentResult Invoke()
        {
            List<Category> categories= context.Categories.ToList();
            return View(categories);
        }
    }
}
