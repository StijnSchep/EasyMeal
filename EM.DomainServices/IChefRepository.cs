using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EM.Domain;

using EM.Domain.Chef_Entities;

namespace EM.DomainServices
{
    public interface IChefRepository
    {
        Dish GetDishById(int dishId);
        IQueryable<Dish> Dishes(string ChefEmail);
        void SaveDish(Dish dish);

        IQueryable<WeekPlan> WeekPlans(string ChefEmail);
        void SaveWeekPlan(WeekPlan weekPlan);

        Dish deleteDish(int dishId);

        List<Meal> GetMealsByDate(DateTime date);
        Meal GetMealById(int MealId);
    }
}
