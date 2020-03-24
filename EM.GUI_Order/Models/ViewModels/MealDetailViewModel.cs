using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain.Chef_Entities;
using EM.Domain.Order_Entities;

namespace EM.GUI_Order.Models.ViewModels
{
    public class MealDetailViewModel
    {
        public MealDetailViewModel(OrderedMeal orderedMeal)
        {
            bool test = orderedMeal == null;
            System.Diagnostics.Debug.WriteLine("Received ordered meal is null? " + test);

            if (test)
            {
                HasOrderedMeal = false;
                return;
            }

            IncludeStarter = orderedMeal.IncludeStarter;
            IncludeDessert = orderedMeal.IncludeDessert;
            Size = orderedMeal.Size;

            // Meal object is given through controller
        }

        public bool HasOrderedMeal { get; set; } = true;
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }

        // The meal with dishes
        // May be null
        public Meal OrderedMeal { get; set; }

        // Options for the ordered meal
        public bool IncludeStarter { get; set; }
        public bool IncludeDessert { get; set; }
        public string Size { get; set; }
    }
}
