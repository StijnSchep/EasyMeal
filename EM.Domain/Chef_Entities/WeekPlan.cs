using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EM.Domain.Utility;

namespace EM.Domain.Chef_Entities
{
    public class WeekPlan : Indexable
    {
        public WeekPlan()
        {
            WeekDays = new HashSet<WeekDay>();
        }

        public DateTime StartDate { get; set; }

        public virtual ICollection<WeekDay> WeekDays { get; set; }

        public string ChefEmail { get; set; }

        public string Validate()
        {
            string dayCount = ValidateWeekDaysCount();

            if (dayCount == null)
            {
                return ValidateDietaryRepresentation();
            }
            else
            {
                return dayCount;
            }

        }

        private string ValidateWeekDaysCount()
        {
            foreach (WeekDay day in WeekDays)
            {
                if (day.Meals.Count == 0)
                {
                    return Constants.WeekPlanInvalidSmall;
                }
            }

            return null;
        }

        private string ValidateDietaryRepresentation()
        {
            int i = 1;
            foreach (WeekDay day in WeekDays)
            {
                int representation = 0;

                foreach (Meal m in day.Meals)
                {
                    representation |= m.GetDietaryRepresentation();
                }

                // After the foreach loop every dietary restriction should have been represented
                // The binary representation should be 11111... depending on the amount of dietary restrictions
                if (representation != Math.Pow(2, Constants.DietaryRestrictions.Length) - 1)
                {
                    // Not every dietary restriction is represented
                    return Constants.WeekPlanDietaryRepresentation + ", probleem bij dag " + i;
                }

                i++;
            }

            return null;
        }

    }


}
