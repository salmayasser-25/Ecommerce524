using Ecommerce524.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Ecommerce524.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           _emailSender = emailSender;

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

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);



                }
                return View(registerVM);
            }
          var token =  await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            //  await _emailSender.SendEmailAsync(applicationUser,"confirm your account!","<a href=''><h1>click here to confirm your account.</h1>");
           

            var confirmationLink = Url.Action("Confirm", "Account", new { area = "Identity", token, applicationUser.Id },Request.Scheme);
            //await _emailSender.SendEmailAsync(applicationUser.Email, "confirm your account!", "<h1>click <a href='{confirmationLink}'> here </a> to confirm your account.</h1>");
            await _emailSender.SendEmailAsync(applicationUser.Email,"confirm your account!",$"<h1>click <a href='{confirmationLink}'> here </a> to confirm your account.</h1>"
);
            TempData["success-notification"] = "Add Account Successfylly";
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Confirm(string id,string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return NotFound();
            }
           // var result =await _userManager.ConfirmEmailAsync(user, token);
           var result = await _userManager.ConfirmEmailAsync( user, token);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);

                }
                TempData["error-notification"] = "Error Confirming Your Account, please try again";
                

                //return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            else
            {
                TempData["success-notification"] = "Your Account Confirmed Successfully, you can login now!";

            }
            return RedirectToAction("Login", "Account", new { area = "Identity" });

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUserName) ?? await _userManager.FindByNameAsync(loginVM.EmailOrUserName);
            

            if (user is null)
            {
                ModelState.AddModelError("EmailOrUserName", "Invalid Email or UserName");
                ModelState.AddModelError("Password", "Invalid Passowrd");
                return View(loginVM);

            }
            //var result = await _userManager.ChangePasswordAsync(user, loginVM.Password);
            //  var result = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("EmailOrUserName", "Confirm your email first!");
                return View(loginVM);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Too many attempts, try again later.");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("EmailOrUserName", "Invalid Email or UserName");
                ModelState.AddModelError("Password", "Invalid Password");
                return View(loginVM);
            }
            TempData["success-notification"] = $"Welcome Back {user.UserName}";
            return RedirectToAction("Index", "Home", new { area = "Custemor" });
        }
    }
}
