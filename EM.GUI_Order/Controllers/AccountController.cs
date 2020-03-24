using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EM.GUI_Order.Models.ViewModels;
using EM.Domain.Order_Entities;
using EM.DomainServices;

namespace EM.GUI_Order.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IOrderRepository repository;

        public AccountController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr,
            IOrderRepository repo)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            repository = repo;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Orders");
                    }
                }


            }

            ModelState.AddModelError("", "Ongeldige inloggegevens");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public ViewResult Register(string returnUrl)
        {
            return View(new RegisterModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser {
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber};

                Customer customer = new Customer
                {
                    EMail = registerModel.Email,
                    BirthDate = registerModel.BirthDate,
                    Name = registerModel.CustomerName,
                    Address = registerModel.Address,
                    PostalCode = registerModel.PostalCode
                };

                repository.SaveCustomer(customer);

                var result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect(registerModel?.ReturnUrl ?? "/Dashboard");
                }
                    foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registerModel);
        }
    }
}
