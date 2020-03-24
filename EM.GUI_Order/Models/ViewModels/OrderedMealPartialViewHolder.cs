using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Order_Entities;

namespace EM.GUI_Order.Models.ViewModels
{
    public class OrderedMealPartialViewHolder
    {
        public OrderedMeal OrderedMeal { get; set; }
        public DateTime MealDate { get; set; }
    }
}
