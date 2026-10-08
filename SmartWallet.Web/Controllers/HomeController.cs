using Microsoft.AspNetCore.Mvc;
using SmartWallet.Web.Models;
using System.Diagnostics;

namespace SmartWallet.Web.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
