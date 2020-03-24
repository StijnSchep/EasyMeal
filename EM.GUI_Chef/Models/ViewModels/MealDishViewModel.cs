using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain;

namespace EM.GUI_Chef.Models.ViewModels
{
    public class MealDishViewModel
    {
        public IQueryable<DishViewModel> Dishes { get; set; }
        public int dayIndex { get; set; }
        public int mealIndex { get; set; }
        public string returnAction { get; set; }
        public string dishCategory { get; set; }
    }
}
