using Microsoft.AspNetCore.Mvc;

namespace YemekTarifiWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
