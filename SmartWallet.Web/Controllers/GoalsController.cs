using Microsoft.AspNetCore.Mvc;

namespace SmartWallet.Web.Controllers
{
    public class GoalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
