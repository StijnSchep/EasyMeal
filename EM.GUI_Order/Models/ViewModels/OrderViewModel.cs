using EM.Domain.Order_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EM.GUI_Order.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel(Order order)
        {
            Order = order;
            EndDate = order.StartDate.AddDays(6);
            NextMonday = order.StartDate.AddDays(7);
            PreviousMonday = order.StartDate.AddDays(-7);

            System.Diagnostics.Debug.WriteLine("Next Monday: " + NextMonday.ToString());
            System.Diagnostics.Debug.WriteLine("Previous Monday: " + PreviousMonday.ToString());

            OrderPhaseActive = (int)DateTime.Today.DayOfWeek > 0 && 
                (int)DateTime.Today.DayOfWeek < 5;

            days = new List<DateTime>();
            for(int i = 0; i < 7; i++)
            {
                days.Add(order.StartDate.AddDays(i));
            }

            TuesdayLimit = order.StartDate.AddDays(3);
        }

        // The order for which to display info
        public Order Order { get; set; }

        public List<DateTime> days { get; set; }

        // Navigation info
        public DateTime EndDate { get; }
        public DateTime NextMonday { get; }
        public DateTime PreviousMonday { get; }

        public DateTime TuesdayLimit { get; set; }

        // Check if the user is allowed to create / edit next week's order
        public bool OrderPhaseActive { get; set; }

        public bool HasMealForDate(DateTime mealDate)
        {
            OrderedMeal oMeal = Order.OrderedMeals
                .FirstOrDefault(om => om.MealDate == mealDate);

            bool test = oMeal != null;
            System.Diagnostics.Debug.WriteLine("Meal found for date? " + test);
            return test;
        }

        public DateTime GetNewOrderStartDate()
        {
            int todayInt = (int)DateTime.Today.DayOfWeek;
            if(todayInt == 0)
            {
                return DateTime.Today.AddDays(1);
            } else
            {
                return DateTime.Today.AddDays(8 - todayInt);
            }
        }
    }
}
