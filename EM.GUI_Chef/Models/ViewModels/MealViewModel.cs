using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain;
using EM.Domain.Chef_Entities;

namespace EM.GUI_Chef.Models.ViewModels
{
    public class MealViewModel
    {
        public Meal Meal { get; set; }
        public int dayIndex { get; set; }
        public int mealIndex { get; set; }

        public DishViewModel Starter { get; set; }
        public DishViewModel Main { get; set; }
        public DishViewModel Dessert { get; set; }

        public void initiate()
        {
            Starter = new DishViewModel() { Dish = Meal.Starter };
            Main = new DishViewModel() { Dish = Meal.Main };
            Dessert = new DishViewModel() { Dish = Meal.Dessert };
        }

        public List<string> DietaryRepresentation
        {
            get
            {
                return Meal.DietaryRepresentation;
            } 
        }

        public decimal TotalPrice
        {
            get
            {
                return Meal.GetTotalPrice();
            }
        }
    }
}
