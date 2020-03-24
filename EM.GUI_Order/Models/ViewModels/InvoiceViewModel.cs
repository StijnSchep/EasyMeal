using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Order_Entities;

namespace EM.GUI_Order.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public string CurrentMonth { get; set; }
        public DateTime BirthDate { get; set; }
        public List<OrderedMeal> OrderedMeals { get; set; }

        public decimal CalculateSubtotal()
        {
            decimal result = 0;

            foreach(OrderedMeal oMeal in OrderedMeals)
            {
                result += oMeal.TotalPrice;
            }

            return result;
        }

        public bool HasPlusFifteenOrders()
        {
            if(OrderedMeals.Count() < 15)
            {
                return false;
            }

            OrderedMeal oMealForBirthDate = OrderedMeals
                .FirstOrDefault(om => om.MealDate.Date == BirthDate.Date);

            if(oMealForBirthDate == null)
            {
                return true;
            }

            return OrderedMeals.Count() > 15;
        }

        public decimal BirthDateDiscount()
        {
            OrderedMeal oMealForBirthDate = OrderedMeals
                .FirstOrDefault(om => 
                om.MealDate.Month == BirthDate.Month &&
                om.MealDate.Day == BirthDate.Day);

            if(oMealForBirthDate == null)
            {
                return 0;
            }

            return oMealForBirthDate.TotalPrice;
        }

        public decimal CalculateTotalPrice()
        {
            decimal result = CalculateSubtotal() - BirthDateDiscount();

            if(HasPlusFifteenOrders())
            {
                return (decimal)0.9 * result;
            }

            return result;
        }
    }
}
