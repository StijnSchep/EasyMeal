using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Order_Entities;

namespace EM.GUI_Order.Models.ViewModels
{
    public class DetailedOrderViewModel
    {
        public DetailedOrderViewModel(Order order)
        {
            Order = order;
            EndDate = order.StartDate.AddDays(6);

            Days = new List<DateTime>();
            for(int i = 0; i < 7; i++)
            {
                Days.Add(order.StartDate.AddDays(i));
            }
        }


        public Order Order { get; set; }
        public DateTime EndDate { get; set; }
        public List<DateTime> Days { get; set; }

        public decimal CalculateTotalPrice()
        {
            decimal result = 0;
            foreach(OrderedMeal meal in Order.OrderedMeals)
            {
                result += meal.TotalPrice;
            }

            return result;
        }
        
        // May return null object
        public OrderedMealPartialViewHolder GetPartialViewModel(DateTime mealDate)
        {
            return new OrderedMealPartialViewHolder
            {
                OrderedMeal = Order.OrderedMeals
                .FirstOrDefault(om => om.MealDate.Date == mealDate),
                MealDate = mealDate
            }; 
        }

        public string GetHeaderMessage()
        {
            return "Bestelling opmaken voor " +
                Order.StartDate.ToString("d MMM", (new CultureInfo("nl-NL")).DateTimeFormat) +
                " - " +
                EndDate.ToString("d MMM", (new CultureInfo("nl-NL")).DateTimeFormat);
        }
    }
}
