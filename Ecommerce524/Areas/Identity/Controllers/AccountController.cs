using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mapster;
namespace Ecommerce524.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);

            }
            //ApplicationUser applicationUser = new ApplicationUser()
            //{
            //    FName = registerVM.FName,
            //    LName = registerVM.LName,
            //    Email = registerVM.Email,
            //    UserName = registerVM.UserName,
            //    Address = registerVM.Address
            //};
             var applicationUser = registerVM.Adapt<ApplicationUser>();
             applicationUser.Id = Guid.NewGuid().ToString();
             var result = await _userManager.CreateAsync(applicationUser, registerVM.Password);

            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    


                }
                return View(registerVM);
            }
            TempData["success-notification"] = "Add Account Successfylly";
            return RedirectToAction("Login");
        }

    }
}
