using Microsoft.AspNetCore.Mvc;
using MultiShop.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

namespace MultiShop.Controllers
{
    public class WebAPIController : Controller
    {
      
        public IActionResult PharmacyOnDuty()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");
            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);
            return View(pharmacy);
        }

        public IActionResult ArtAndCulture()
        {
            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");
            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);
            return View(activite);
        }

        public IActionResult ExchangeRate()
        {
            return View();
        }

        public IActionResult WeatherForecast()
        {
            string apikey = "52b72dad903d5a0244a91d029fce3686";
            string city = "izmir";

            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&lang=tr&units=metric&appid=" + apikey;

            XDocument weather = XDocument.Load(url);

            ViewBag.temperature = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            return View();
        }
    }
}
