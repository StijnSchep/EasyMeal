using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EM.Domain;
using EM.DomainServices;
using EM.Domain.Order_Entities;
using EM.Domain.API_Entities;

using EM.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EM.Domain.Chef_Entities;
using System.Net.Http;

namespace EM.Infrastructure.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private AppOrderDbContext context;
        private readonly string BaseURL = "https://localhost:44397/api/";

        public EFOrderRepository(AppOrderDbContext ctx)
        {
            context = ctx;
        }

        public async Task<Customer> GetCustomer(UserManager<IdentityUser> userManager, 
            HttpContext httpContext)
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            return context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail.Equals(user.Email));
        }

        public async Task<Meal> GetMealById(int MealId)
        {
            Meal Meal = null;
            HttpResponseMessage response = await new HttpClient()
                .GetAsync(BaseURL + "Meal/" + MealId);

            if (response.IsSuccessStatusCode)
            {
                ReceivedAPIMeal APIMeal = await response.Content.ReadAsAsync<ReceivedAPIMeal>();
                Meal = new Meal()
                {
                    Name = APIMeal.Name,
                    StarterId = APIMeal.StarterId,
                    Starter = await GetDishById(APIMeal.StarterId),
                    MainId = APIMeal.MainId,
                    Main = await GetDishById(APIMeal.MainId),
                    DessertId = APIMeal.DessertId,
                    Dessert = await GetDishById(APIMeal.DessertId),
                    Id = APIMeal.Id
                };

            }

            return Meal;
        }

        public async Task<Meal> GetMealByAPIMeal(ReceivedAPIMeal APIMeal)
        {
            return new Meal()
            {
                Name = APIMeal.Name,
                StarterId = APIMeal.StarterId,
                Starter = await GetDishById(APIMeal.StarterId),
                MainId = APIMeal.MainId,
                Main = await GetDishById(APIMeal.MainId),
                DessertId = APIMeal.DessertId,
                Dessert = await GetDishById(APIMeal.DessertId),
                Id = APIMeal.Id
            };
        }

        public Order GetOrderByDate(DateTime StartDate, string Email)
        {
            Order order = context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail.Equals(Email))
                .Orders.FirstOrDefault(o => o.StartDate.Date == StartDate.Date);

            if(order == null)
            {
                System.Diagnostics.Debug.WriteLine("No order found, returning new order with startdate: " + StartDate.ToString());
                order = new Order
                {
                    StartDate = StartDate
                };
            }

            return order;
        }

        public OrderedMeal GetOrderedMealForDate(DateTime startDate, DateTime mealDate, string Email)
        {
            Customer customer = context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail.Equals(Email));

            if (customer == null) {
                System.Diagnostics.Debug.WriteLine("No customer found");
                return null; }

            Order RelevantOrder = customer.Orders
                .FirstOrDefault(o => o.StartDate.Date == startDate.Date);

            // If there was no order for the week, there won't be an ordered meal
            if(RelevantOrder == null) {
                System.Diagnostics.Debug.WriteLine("No relevant order found");
                return null; }

            System.Diagnostics.Debug.WriteLine("Found a relevant order, returning ordered meal with date");
            return RelevantOrder.OrderedMeals.FirstOrDefault(om => om.MealDate.Date == mealDate.Date);
        }

        public async Task<Dish> GetDishById(int DishId)
        {
            Dish Dish = null;

            HttpResponseMessage response = await new HttpClient()
                .GetAsync(BaseURL + "Dish/" + DishId);

            if (response.IsSuccessStatusCode)
            {
                Dish = await response.Content.ReadAsAsync<Dish>();
            }

            return Dish;
        }

        public async Task<List<Meal>> GetMealsForDate(DateTime date)
        {
            List<Meal> Meals = new List<Meal>();

            HttpResponseMessage response = await new HttpClient()
                .GetAsync(BaseURL + "List/" + date.ToString("yyyy-MM-dd"));

            if(response.IsSuccessStatusCode)
            {
                List<ReceivedAPIMeal> APIMeals = await response.Content.ReadAsAsync<List<ReceivedAPIMeal>>();
                foreach(ReceivedAPIMeal APIMeal in APIMeals)
                {
                    Meals.Add(await GetMealByAPIMeal(APIMeal));
                }
            }

            return Meals;
        }

        public void SaveOrder(Order order, string Email)
        {
            Order existing = context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail == Email)
                .Orders.FirstOrDefault(o => o.StartDate.Date == order.StartDate.Date);

            context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail == Email)
                .Orders.Remove(existing);

            context.SaveChanges();
            order.Id = 0;
            foreach(OrderedMeal oMeal in order.OrderedMeals)
            {
                oMeal.Id = 0;
            }

            context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail == Email)
                .Orders.Add(order);

            context.SaveChanges();
        }

        public void SaveCustomer(Customer cust)
        {
            if(cust.CustomerNumber == 0)
            {
                context.Customers.Add(cust);
            }

            context.SaveChanges();
        }

        public async Task<List<OrderedMeal>> GetOrderedMealsForMonth(DateTime dateTime, string Email)
        {
            List<OrderedMeal> orderedMeals = new List<OrderedMeal>();

            Customer cust = context.Customers
                .Include("Orders.OrderedMeals")
                .FirstOrDefault(c => c.EMail == Email);

            List<Order> ordersForMonth = cust.Orders
                .Where(o =>
                o.StartDate.Month == dateTime.Month &&
                o.StartDate.Year == dateTime.Year).ToList();

            foreach(Order order in ordersForMonth)
            {
                foreach(OrderedMeal oMeal in order.OrderedMeals)
                {
                    oMeal.Meal = await GetMealById(oMeal.MealId);
                    orderedMeals.Add(oMeal);
                }
            }

           return orderedMeals;
        }
    }
}
