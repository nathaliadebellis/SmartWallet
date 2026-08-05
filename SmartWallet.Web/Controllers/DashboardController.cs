using Microsoft.AspNetCore.Mvc;

namespace SmartWallet.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
