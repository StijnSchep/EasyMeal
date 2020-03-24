using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain;
using EM.Domain.Chef_Entities;

namespace EM.GUI_Chef.Models.ViewModels
{
    public class WeekplanViewModel
    {
        public WeekPlan WeekPlan { get; set; }

        public string dateRange
        {
            get
            {
                DateTimeFormatInfo fmt = (new CultureInfo("nl-NL")).DateTimeFormat;
                string start = WeekPlan.StartDate.ToString("d MMM", fmt);
                string end = WeekPlan.StartDate.AddDays(6).ToString("d MMM", fmt);

                return start + " - " + end;
            }
        }

        public void initiate()
        {
            if(WeekPlan.WeekDays.Count() != 7)
            {
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(1) });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(2) });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(3) });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(4) });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(5) });
                WeekPlan.WeekDays.Add(new WeekDay { MealDate = WeekPlan.StartDate.AddDays(6) });
            }
        }

        public bool isComplete
        {
            get
            {
                bool isComplete = true;
                foreach(WeekDay day in WeekPlan.WeekDays)
                {
                    if(!day.Meals.Any()) { isComplete = false;  }
                }

                return isComplete;
            }
        }
    }
}
