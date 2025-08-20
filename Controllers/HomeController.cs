using Microsoft.AspNetCore.Mvc;

namespace YemekTarifiWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();  // Anasayfa
        }

        public IActionResult About()
        {
            return View();  // Hakkımızda
        }

        public IActionResult Contact()
        {
            return View();  // İletişim
        }
    }
}
