using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using EM.DomainServices;
using EM.GUI_Chef.Models.ViewModels;
using EM.Domain;
using System.IO;

using EM.Domain.Chef_Entities;

namespace EM.GUI_Chef.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IChefRepository repository;

        public DashboardController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr, IChefRepository repo)
        {
            repository = repo;
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dishes()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            return View(repository
                .Dishes(user.Email)
                .Select(d => new DishViewModel{ Dish = d})
                .ToList()
                );
        }

        public IActionResult CreateDish()
        {
            return View("EditDish", new DishViewModel
            {
                Dish = new Dish()
            });
        }

        public IActionResult EditDish(int dishId)
        {
            DishViewModel viewModel = new DishViewModel
            {
                Dish = repository.GetDishById(dishId)
            };
            viewModel.checkBooleans();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish(DishViewModel dishViewModel)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            System.Diagnostics.Debug.WriteLine("Dish image: " + dishViewModel.Image);

            if (ModelState.IsValid)
            {
                if(dishViewModel.Image != null)
                {
                    MemoryStream ms = new MemoryStream();
                    dishViewModel.Image.CopyTo(ms);
                    byte[] imageArray = ms.ToArray();

                    dishViewModel.Dish.Image = imageArray;
                }

                dishViewModel.Dish.ChefEmail = user.Email;
                dishViewModel.Dish.DietaryRestrictions = dishViewModel.DietaryInt;
                dishViewModel.Dish.DishCategory = dishViewModel.CategorieInt;

                repository.SaveDish(dishViewModel.Dish);
                TempData["message"] = $"{dishViewModel.Dish.Name} is opgeslagen";
                return RedirectToAction("Dishes");
            } else
            {
                return View(dishViewModel);
            }
        }

        public IActionResult DeleteDish(int dishId)
        {
            Dish deletedDish = repository.deleteDish(dishId);
            if (deletedDish != null) {
                TempData["message"] = $"{deletedDish.Name} is verwijderd";
            }
            return RedirectToAction("Dishes");
        }


    }
}
