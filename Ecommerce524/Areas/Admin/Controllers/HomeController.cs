
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce524.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
      
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
