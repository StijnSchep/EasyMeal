using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections;
using System.Collections.Generic;

using EM.Domain.Utility;

namespace EM.Domain.Chef_Entities
{
    public class WeekDay : Indexable
    {
        public WeekDay()
        {
            Meals = new HashSet<Meal>();
        }

        public DateTime MealDate { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }

        public void AddMeal(Meal meal)
        {
            Meals.Add(meal);
        }

        public void DeleteMeal(Meal meal)
        {
            Meals.Remove(meal);
        }

        [ForeignKey("WeekPlan")]
        public virtual int WeekPlanId { get; set; }
        public virtual WeekPlan WeekPlan { get; set; }

        public bool valid()
        {
            int representation = 0;

            foreach (Meal m in Meals)
            {
                representation |= m.GetDietaryRepresentation();
            }

            // After the foreach loop every dietary restriction should have been represented
            // The binary representation should be 11111... depending on the amount of dietary restrictions
            if (representation != Math.Pow(2, Constants.DietaryRestrictions.Length) - 1)
            {
                return false;
            }

            return true;
        }
    }
}
