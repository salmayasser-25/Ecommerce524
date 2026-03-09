
using Microsoft.AspNetCore.Mvc;
using Ecommerce524.DataAccess;
namespace Ecommerce524.Areas.Admin.Controllers
{

    [Area(SD.ADMIN_AREA)]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            var products = _context.Products.ToList();
            return View();
        }
    }
}

