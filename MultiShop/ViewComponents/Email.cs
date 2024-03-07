using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;

namespace MultiShop.ViewComponents
{
    public class Email: ViewComponent
    {
        ShopConnection context= new ShopConnection();
        public string Invoke()
        {
            string email = context.Settings.FirstOrDefault(s => s.SettingID == 1).Email;
            return $"{email}";
        }
    }
}
