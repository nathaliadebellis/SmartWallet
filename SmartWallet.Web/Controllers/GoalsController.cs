using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartWallet.Web.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
