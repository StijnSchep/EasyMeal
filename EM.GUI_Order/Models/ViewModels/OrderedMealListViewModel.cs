using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Order_Entities;

namespace EM.GUI_Order.Models.ViewModels
{
    public class OrderedMealListViewModel
    {
        public List<OrderedMeal> OrderedMeals { get; set; }
        public DateTime MealDate { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime StartDate { get; set; }

        public string GetHeaderMessage()
        {
            return "Maaltijd kiezen voor " +
               MealDate.ToString("dddd, dd MMMM yyyy", (new CultureInfo("nl-NL")).DateTimeFormat);
        }
    }
}
