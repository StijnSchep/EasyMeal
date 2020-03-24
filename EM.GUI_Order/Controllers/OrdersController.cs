using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using EM.DomainServices;
using EM.GUI_Order.Models.ViewModels;
using EM.AppServices;
using EM.Domain;
using System.IO;

using EM.Domain.Chef_Entities;
using EM.Domain.Order_Entities;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EM.GUI_Order.Controllers
{

    [Authorize]
    public class OrdersController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IOrderRepository repository;

        public OrdersController(UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr, IOrderRepository repo)
        {
            repository = repo;
            userManager = userMgr;
            signInManager = signInMgr;
        }

        // View order for current week
        public IActionResult Index()
        {
            int dayValue = (int) DateTime.Today.DayOfWeek;
            int difference = dayValue - (int)DayOfWeek.Monday;

            DateTime currentMonday;
            if(difference >= 0) { currentMonday = DateTime.Today.AddDays(-difference); }
            else { currentMonday = DateTime.Today.AddDays(-6); }

            return RedirectToAction("OrderInfo", new { startDate = currentMonday });
        }

        // View order for specific start date
        public async Task<IActionResult> OrderInfo(DateTime startDate)
        {
            System.Diagnostics.Debug.WriteLine("Received date: " + startDate);

            Customer customer = await repository.GetCustomer(userManager, HttpContext);

            if(customer == null)
            {
                return Redirect("/Account/logout");
            }
            ViewData["customerName"] = customer.Name;

            var user = await userManager.GetUserAsync(HttpContext.User);
            Order OrderForDate = repository.GetOrderByDate(startDate, user.Email);

            OrderViewModel OrderViewModel = new OrderViewModel(OrderForDate);

            return View(OrderViewModel);
        }

        public async Task<IActionResult> DayDetails(DateTime startDate, DateTime mealDate)
        {
            Customer customer = await repository.GetCustomer(userManager, HttpContext);
            ViewData["customerName"] = customer.Name;

            OrderedMeal OrderedMealForDate = repository
                .GetOrderedMealForDate(startDate, mealDate, customer.EMail);

            bool test = OrderedMealForDate == null;
            System.Diagnostics.Debug.WriteLine("Ordered meal is null? " + test);

            MealDetailViewModel mealDetailViewModel =
                new MealDetailViewModel(OrderedMealForDate)
                {
                    Date = mealDate,
                    StartDate = startDate
                };

            if (OrderedMealForDate != null)
            {
                int MealId = OrderedMealForDate.MealId;
                mealDetailViewModel.OrderedMeal = await repository.GetMealById(MealId);
            }
            
            return View(mealDetailViewModel);
        }

        public async Task<IActionResult> EditOrder(DateTime startDate)
        {
            System.Diagnostics.Debug.WriteLine("Received date: " + startDate);

            Customer customer = await repository.GetCustomer(userManager, HttpContext);
            ViewData["customerName"] = customer.Name;

            bool OrderPhaseActive = (int)DateTime.Today.DayOfWeek > 0 &&
                (int)DateTime.Today.DayOfWeek < 5;

            // Check if any order can be editted today
            OrderPhaseActive = true;
            if(!OrderPhaseActive)
            {
                String message = "Bestelling kunnen alleen van maandag t/m donderdag gewijzigd worden";
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    Message = message,
                    ReturnAction = "Index"
                };
                return View("Error", errorViewModel);
            }

            DateTime nextMonday;
            int todayInt = (int) DateTime.Today.DayOfWeek;
            if(todayInt == 0){ nextMonday = DateTime.Today.AddDays(1);
            } else { nextMonday = DateTime.Today.AddDays(8 - todayInt);}

            // Check if the order for this startdate can be editted
            if(nextMonday.Date != startDate.Date)
            {
                string endDateString = startDate.AddDays(6).ToString("d MMM", (new CultureInfo("nl-NL")).DateTimeFormat);
                string startDateString = startDate.ToString("d MMM", (new CultureInfo("nl-NL")).DateTimeFormat);
                string message = "Bestelling voor periode " + startDateString + " - " + endDateString + " kan niet gewijzigd worden";
                ErrorViewModel errorViewModel = new ErrorViewModel
                {
                    Message = message,
                    ReturnAction = "Index"
                };
                
                return View("Error", errorViewModel);
            }

            // Order can be editted / created
            Order Order = await GetOrderFromSession(startDate, customer.EMail);
            DetailedOrderViewModel detailedOrderViewModel =
                new DetailedOrderViewModel(Order);

            return View(detailedOrderViewModel);
        }

        public async Task<IActionResult> SelectMeal(DateTime mealDate)
        {
            Customer customer = await repository.GetCustomer(userManager, HttpContext);
            ViewData["customerName"] = customer.Name;

            List<Meal> Meals = await repository.GetMealsForDate(mealDate);
            List<OrderedMeal> OrderedMeals = Meals.Select(
                m => new OrderedMeal
                {
                    MealId = m.Id,
                    Meal = m,
                    IncludeStarter = true,
                    IncludeDessert = true,
                    MealDate = mealDate,
                    TotalPrice = m.GetTotalPrice()
                }).ToList();

            DateTime activeMonday;
            if((int)mealDate.DayOfWeek == 0)
            {
                activeMonday = mealDate.AddDays(-6);
            } else
            {
                activeMonday = mealDate.AddDays(1 - (int)mealDate.DayOfWeek);
            }

            Order order = await GetOrderFromSession(activeMonday, customer.EMail);
            decimal result = 0;
            foreach (OrderedMeal meal in order.OrderedMeals)
            {
                result += meal.TotalPrice;
            }

            OrderedMealListViewModel orderedMealListViewModel
                = new OrderedMealListViewModel
                {
                    OrderedMeals = OrderedMeals,
                    MealDate = mealDate,
                    TotalOrderPrice = result,
                    StartDate = activeMonday
                };

            return View(orderedMealListViewModel);
        }

        public async Task<IActionResult> SetMeal(OrderedMeal orderedMeal)
        {
            Customer customer = await repository.GetCustomer(userManager, HttpContext);

            Meal Meal = await repository.GetMealById(orderedMeal.MealId);
            orderedMeal.Meal = Meal;
            orderedMeal.TotalPrice = orderedMeal.CalculateTotalPrice();
            System.Diagnostics.Debug.WriteLine("Calculated price: " + orderedMeal.CalculateTotalPrice());
            System.Diagnostics.Debug.WriteLine("Received date: " + orderedMeal.MealDate);
            DateTime activeMonday;
            if ((int)orderedMeal.MealDate.DayOfWeek == 0)
            {
                activeMonday = orderedMeal.MealDate.AddDays(-6);
            }
            else
            {
                activeMonday = orderedMeal.MealDate.AddDays(1 - (int)orderedMeal.MealDate.DayOfWeek);
            }

            System.Diagnostics.Debug.WriteLine("Active monday SetMeal: " + activeMonday);

            Order order = await GetOrderFromSession(activeMonday, customer.EMail);

            OrderedMeal existing = order.OrderedMeals.FirstOrDefault(
                om => om.MealDate.Date == orderedMeal.MealDate.Date);
            order.OrderedMeals.Remove(existing);

            order.OrderedMeals.Add(orderedMeal);
            SaveOrderToSession(order);

            return RedirectToAction("EditOrder", new { startDate = activeMonday});
        }

        public async Task<IActionResult> SaveOrder(DateTime startDate)
        {
            Customer customer = await repository.GetCustomer(userManager, HttpContext);
            Order order = await GetOrderFromSession(startDate, customer.EMail);

            if(!order.HasEnoughOrdered())
            {
                TempData["warning"] = "Je moet minstens vier maaltijden bestellen voor de werkdagen";
                return RedirectToAction("EditOrder", new { startDate = startDate.ToString("yyyy-MM-dd") });
            }

            repository.SaveOrder(order, customer.EMail);
            DeleteCurrentSessionOrder();
            TempData["message"] = $"Bestelling is opgeslagen";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewInvoice()
        {
            Customer customer = await repository.GetCustomer(userManager, HttpContext);
            ViewData["customerName"] = customer.Name;
            // repository GetOrderedMealsForMonth(DateTime Date)
            List<OrderedMeal> orderedMeals =
                await repository.GetOrderedMealsForMonth(DateTime.Today, customer.EMail);

            InvoiceViewModel invoiceViewModel = new InvoiceViewModel
            {
                CurrentMonth = DateTime.Today.ToString("MMMM", (new CultureInfo("nl-NL")).DateTimeFormat),
                BirthDate = customer.BirthDate,
                OrderedMeals = orderedMeals
            };

            return View(invoiceViewModel);
        }

        private async Task<Order> GetOrderFromSession(DateTime startDate, string Email)
        {
            Order Order = HttpContext.Session.GetJson<Order>("Order");

            // User wants to edit a different order
            // Delete current order from session
            if(Order != null && Order.StartDate.Date != startDate.Date)
            {
                DeleteCurrentSessionOrder();
            }

            // No Order found for this startdate
            if (Order == null)
            {
                // Get order from database
                // Returns new order if none found
                Order = repository.GetOrderByDate(startDate, Email);

                // Populate Meal objects in the existing OrderedMeals
                foreach(OrderedMeal orderedMeal in Order.OrderedMeals)
                {
                    orderedMeal.Meal = await repository.GetMealById(orderedMeal.MealId);
                }

                SaveOrderToSession(Order);
            }

            return Order;
        }
        private void SaveOrderToSession(Order Order)
        {
            HttpContext.Session.SetJson("Order", Order);
        }
        private void DeleteCurrentSessionOrder()
        {
            HttpContext.Session.SetJson("Order", null);
        }
    }
}
