using Microsoft.AspNetCore.Mvc;

namespace ShopWeb.Controllers
{
    public class CouponController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
