using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EM.Domain.Order_Entities;

namespace EM.DomainServices
{
    public static class OrderExtension
    {

        public static bool HasEnoughOrdered(this Order order)
        {
            DateTime sat = order.StartDate.AddDays(5);
            DateTime sun = order.StartDate.AddDays(6);

            List<OrderedMeal> oMeals = order.OrderedMeals
                .Where(m => m.MealDate.Date != sat.Date && m.MealDate.Date != sun.Date).ToList();

            if(oMeals == null || oMeals.Count() < 4)
            {
                return false;
            }

            return true;
        }
    }
}
