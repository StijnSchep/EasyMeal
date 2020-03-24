using EM.AppServices;
using EM.Domain;
using EM.DomainServices;
using EM.GUI_Chef.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Chef_Entities;

namespace EM.GUI_Chef.Controllers
{
    [Authorize]
    public class WeekplanController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IChefRepository repository;

        public WeekplanController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr, IChefRepository repo)
        {
            repository = repo;
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public async Task<IActionResult> WeekPlans()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            return View(repository.WeekPlans(user.Email));
        }

        public IActionResult CreateWeekplan()
        {
            WeekplanViewModel viewModel = GetWeekplanViewModel();
            SaveWeekplanViewModel(viewModel);

            return View("EditWeekplan", viewModel);
        }

        public async Task<IActionResult> EditWeekplan(int weekplanId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            WeekPlan plan = repository.WeekPlans(user.Email)
                .FirstOrDefault(wp => wp.Id == weekplanId);

            System.Diagnostics.Debug.WriteLine("Logging my current weekplan from EditWeekplan");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(plan, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));


            foreach (WeekDay day in plan.WeekDays)
            {
                foreach(Meal meal in day.Meals)
                {
                    if(meal.StarterId != 0) { meal.Starter = repository.GetDishById(meal.StarterId);}
                    if (meal.MainId != 0) { meal.Main = repository.GetDishById(meal.MainId); }
                    if (meal.DessertId != 0) { meal.Dessert = repository.GetDishById(meal.DessertId); }
                }
            }

            WeekplanViewModel viewModel = new WeekplanViewModel
            {
                WeekPlan = plan
            };

            SaveWeekplanViewModel(viewModel);

            return View(viewModel);
        }

        public IActionResult EditWeekday(int dayIndex)
        {
            WeekDay day = GetWeekplanViewModel().WeekPlan.WeekDays.ElementAt(dayIndex);
            WeekdayViewModel viewModel = new WeekdayViewModel
            {
                day = day,
                dayIndex = dayIndex
            };

            System.Diagnostics.Debug.WriteLine("Logging my current weekplan from EditWeekday");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(GetWeekplanViewModel(), new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));

            return View(viewModel);
        }

        private WeekplanViewModel GetWeekplanViewModel()
        {
            WeekplanViewModel viewModel = HttpContext.Session
                .GetJson<WeekplanViewModel>("WeekplanViewModel");

            if(viewModel == null)
            {
                DateTime today = DateTime.Today;
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;

                DateTime nextMonday;
                if (daysUntilMonday == 0) { nextMonday = today.AddDays(7); }
                else { nextMonday = today.AddDays(daysUntilMonday); }

                WeekPlan newPlan = new WeekPlan()
                {
                    StartDate = nextMonday
                };

                WeekplanViewModel newViewModel = new WeekplanViewModel
                {
                    WeekPlan = newPlan
                };

                newViewModel.initiate();

                viewModel = newViewModel;
            }

            return viewModel;
        }
        private void SaveWeekplanViewModel(WeekplanViewModel viewModel)
        {
            HttpContext.Session.SetJson("WeekplanViewModel", viewModel);
        }
        private void DeleteWeekplanViewModel()
        {
            HttpContext.Session.SetJson("WeekplanViewModel", null);
        }

        public IActionResult CreateMeal(int dayIndex)
        {
            System.Diagnostics.Debug.WriteLine("Logging my current weekplan from CreateMeal");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(GetWeekplanViewModel(), new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));

            WeekplanViewModel planModel = GetWeekplanViewModel();
            planModel.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.Add(new Meal());

            int mealIndex = planModel.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.Count() - 1;

            MealViewModel viewModel = new MealViewModel()
            {
                Meal = planModel.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.Last(),
                dayIndex = dayIndex,
                mealIndex = mealIndex
            };
            viewModel.initiate();

            SaveWeekplanViewModel(planModel);
            return View("EditMeal", viewModel);
        }

        public IActionResult EditMeal(int dayIndex, int mealIndex)
        {
            WeekplanViewModel planModel = GetWeekplanViewModel();
            MealViewModel viewModel = new MealViewModel()
            {
                Meal = planModel.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex),
                dayIndex = dayIndex,
                mealIndex = mealIndex
            };
            viewModel.initiate();

            return View(viewModel);
        }

        public IActionResult DeleteMeal(int dayIndex, int mealIndex)
        {
            WeekplanViewModel planModel = GetWeekplanViewModel();
            Meal toBeRemoved = planModel.WeekPlan.WeekDays
                .ElementAt(dayIndex).Meals.ElementAt(mealIndex);

            planModel.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.Remove(toBeRemoved);
            SaveWeekplanViewModel(planModel);

            WeekDay day = planModel.WeekPlan.WeekDays.ElementAt(dayIndex);
            WeekdayViewModel viewModel = new WeekdayViewModel
            {
                day = day,
                dayIndex = dayIndex
            };

            return View("EditWeekday", viewModel);
        }

        public async Task<IActionResult> SelectStarter(int dayIndex, int mealIndex)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            IQueryable<DishViewModel> Dishes = repository
                .Dishes(user.Email).Where(d => d.DishCategory > 3)
                .Select(d => new DishViewModel { Dish = d });

            MealDishViewModel viewModel = new MealDishViewModel()
            {
                Dishes = Dishes,
                dayIndex = dayIndex,
                mealIndex = mealIndex,
                returnAction = "SetStarter",
                dishCategory = "voorgerecht"
            };

            return View("SelectDish", viewModel);
        }
        public async Task<IActionResult> SelectMain(int dayIndex, int mealIndex)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            IQueryable<DishViewModel> Dishes = repository
                .Dishes(user.Email).Where(d => 
                d.DishCategory == 2 || d.DishCategory == 3 || d.DishCategory == 7 || d.DishCategory == 6)
                .Select(d => new DishViewModel { Dish = d });

            MealDishViewModel viewModel = new MealDishViewModel()
            {
                Dishes = Dishes,
                dayIndex = dayIndex,
                mealIndex = mealIndex,
                returnAction = "SetMain",
                dishCategory = "hoofdgerecht"
            };

            return View("SelectDish", viewModel);
        }
        public async Task<IActionResult> SelectDessert (int dayIndex, int mealIndex)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            IQueryable<DishViewModel> Dishes = repository
                .Dishes(user.Email).Where(d =>
                d.DishCategory == 1 || d.DishCategory == 3 || d.DishCategory == 7 || d.DishCategory == 5)
                .Select(d => new DishViewModel { Dish = d });

            MealDishViewModel viewModel = new MealDishViewModel()
            {
                Dishes = Dishes,
                dayIndex = dayIndex,
                mealIndex = mealIndex,
                returnAction = "SetDessert",
                dishCategory = "nagerecht"
            };

            return View("SelectDish", viewModel);
        }

        public IActionResult SetStarter(int dayIndex, int mealIndex, int dishId)
        {
            Dish Dish = repository.GetDishById(dishId);
            WeekplanViewModel plan = GetWeekplanViewModel();
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).Starter = Dish;
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).StarterId = Dish.Id;
            SaveWeekplanViewModel(plan);

            MealViewModel viewModel = new MealViewModel()
            {
                Meal = plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex),
                dayIndex = dayIndex,
                mealIndex = mealIndex
            };
            viewModel.initiate();

            return View("EditMeal", viewModel);
        }
        public IActionResult SetMain(int dayIndex, int mealIndex, int dishId)
        {
            Dish Dish = repository.GetDishById(dishId);
            WeekplanViewModel plan = GetWeekplanViewModel();
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).Main = Dish;
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).MainId = Dish.Id;
            SaveWeekplanViewModel(plan);

            MealViewModel viewModel = new MealViewModel()
            {
                Meal = plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex),
                dayIndex = dayIndex,
                mealIndex = mealIndex
            };
            viewModel.initiate();

            return View("EditMeal", viewModel);
        }
        public IActionResult SetDessert(int dayIndex, int mealIndex, int dishId)
        {
            Dish Dish = repository.GetDishById(dishId);
            WeekplanViewModel plan = GetWeekplanViewModel();
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).Dessert = Dish;
            plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex).DessertId = Dish.Id;
            SaveWeekplanViewModel(plan);

            MealViewModel viewModel = new MealViewModel()
            {
                Meal = plan.WeekPlan.WeekDays.ElementAt(dayIndex).Meals.ElementAt(mealIndex),
                dayIndex = dayIndex,
                mealIndex = mealIndex
            };
            viewModel.initiate();

            return View("EditMeal", viewModel);
        }

        public IActionResult SaveMeal(int dayIndex, int mealIndex, string mealName)
        {
            System.Diagnostics.Debug.WriteLine(mealName);
            WeekplanViewModel planModel = GetWeekplanViewModel();
            planModel.WeekPlan.WeekDays
                .ElementAt(dayIndex).Meals
                .ElementAt(mealIndex).Name = mealName;

            SaveWeekplanViewModel(planModel);
            System.Diagnostics.Debug.WriteLine("Logging my current weekplan from SaveMeal");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(GetWeekplanViewModel(), new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));

            WeekDay day = GetWeekplanViewModel().WeekPlan.WeekDays.ElementAt(dayIndex);
            WeekdayViewModel newViewModel = new WeekdayViewModel
            {
                day = day,
                dayIndex = dayIndex
            };

            return View("EditWeekday", newViewModel);
        }
        public async Task<IActionResult> SaveWeekplan()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            System.Diagnostics.Debug.WriteLine("Logging my current weekplan from SaveWeekplan");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(GetWeekplanViewModel(), new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));

            WeekplanViewModel viewModel = GetWeekplanViewModel();
            WeekPlan weekPlan = viewModel.WeekPlan;
            weekPlan.ChefEmail = user.Email;

            repository.SaveWeekPlan(weekPlan);
            TempData["message"] = $"Weekplan voor {viewModel.dateRange} is opgeslagen";
            DeleteWeekplanViewModel();
            return RedirectToAction("WeekPlans");
        }
    }
}
