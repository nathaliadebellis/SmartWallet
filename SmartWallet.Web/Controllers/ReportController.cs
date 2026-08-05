using Microsoft.AspNetCore.Mvc;

namespace SmartWallet.Web.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
