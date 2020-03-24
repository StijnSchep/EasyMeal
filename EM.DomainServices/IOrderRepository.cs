using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using EM.Domain;

using EM.Domain.Order_Entities;
using EM.Domain.Chef_Entities;
using EM.Domain.API_Entities;

namespace EM.DomainServices
{
    public interface IOrderRepository
    {
        Task<Customer> GetCustomer(UserManager<IdentityUser> userManager,
            HttpContext httpContext);
        void SaveCustomer(Customer cust);

        Order GetOrderByDate(DateTime StartDate, string Email);

        OrderedMeal GetOrderedMealForDate(DateTime startDate, DateTime Date, string Email);
        Task<List<OrderedMeal>> GetOrderedMealsForMonth(DateTime dateTime, string Email);

        Task<Meal> GetMealById(int MealId);
        Task<Meal> GetMealByAPIMeal(ReceivedAPIMeal APIMeal);
        Task<Dish> GetDishById(int DishId);
        Task<List<Meal>> GetMealsForDate(DateTime date);

        void SaveOrder(Order order, string Email);

    }
}
