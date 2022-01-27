using Microsoft.AspNetCore.Mvc;

namespace Asp_Net.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
