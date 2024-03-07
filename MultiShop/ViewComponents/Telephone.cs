using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;

namespace MultiShop.ViewComponents
{
    public class Telephone:ViewComponent
    {
        ShopConnection context= new ShopConnection();
        public string Invoke()
        {
            string telephone = context.Settings.FirstOrDefault(s => s.SettingID == 1).Telephone;
            return $"{telephone}";
        }
    }
}
