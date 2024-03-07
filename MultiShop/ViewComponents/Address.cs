using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;

namespace MultiShop.ViewComponents
{
    public class Address:ViewComponent
    {
        ShopConnection context= new ShopConnection();
        public string Invoke()
        {
            string address = context.Settings.FirstOrDefault(s => s.SettingID == 1).Address;
            return $"{address}";
        }
    }
}
