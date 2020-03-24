using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EM.Domain;
using EM.DomainServices;
using EM.Domain.Chef_Entities;

using EM.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EM.Infrastructure.Repositories
{
    public class EFChefRepository : IChefRepository
    {
        private AppChefDbContext context;

        public EFChefRepository(AppChefDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Dish> Dishes(string ChefEmail)
        {
            return context.Dishes.Where(d => d.ChefEmail.Equals(ChefEmail));
        }

        public IQueryable<WeekPlan> WeekPlans(string ChefEmail)
        {
            return context.WeekPlans
                .Include("WeekDays.Meals")
                .Where(wp => wp.ChefEmail.Equals(ChefEmail));
        }

        public void SaveDish(Dish dish)
        {
            if(dish.Id == 0)
            {
                context.Dishes.Add(dish);
            } else
            {
                Dish dbEntry = context.Dishes
                    .FirstOrDefault(d => d.Id == dish.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = dish.Name;
                    dbEntry.Price = dish.Price;
                    dbEntry.DietaryRestrictions = dish.DietaryRestrictions;
                    dbEntry.DishCategory = dish.DishCategory;
                    dbEntry.Description = dish.Description;
                    dbEntry.Image = dish.Image;
                    dbEntry.ChefEmail = dish.ChefEmail;
                }
            }

            context.SaveChanges();
        }

        public void SaveWeekPlan(WeekPlan weekPlan)
        {
            if (weekPlan.Id == 0)
            {
                context.WeekPlans.Add(weekPlan);
            }
            else
            {
                WeekPlan dbEntry = context.WeekPlans
                    .FirstOrDefault(d => d.Id == weekPlan.Id);

                context.WeekPlans.Remove(dbEntry);
                weekPlan.Id = 0;
                context.WeekPlans.Add(weekPlan);
            }

            context.SaveChanges();
        }

        public Dish GetDishById(int dishId)
        {
            return context.Dishes.FirstOrDefault(d => d.Id == dishId);
        }

        public Dish deleteDish(int dishId)
        {
            Dish dbEntry = context.Dishes
                .FirstOrDefault(d => d.Id == dishId);
            if (dbEntry != null) {
                context.Dishes.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }

        public List<Meal> GetMealsByDate(DateTime date)
        {
            List<Meal> meals = new List<Meal>();
            List<WeekDay> days = context.WeekDay
                .Include(wd => wd.Meals)
                .Where(wd => wd.MealDate.Date == date.Date).ToList();
            System.Diagnostics.Debug.WriteLine("days found: " + days.Count());
            foreach (WeekDay day in days)
            {
                System.Diagnostics.Debug.WriteLine("This day has: " + day.Meals.Count() + " meal(s)");
                meals.AddRange(day.Meals);
            }

            System.Diagnostics.Debug.WriteLine("Total meals to return: " + meals.Count());
            return meals;
        }

        public Meal GetMealById(int MealId)
        {
            return context.Meal.FirstOrDefault(m => m.Id == MealId);
        }
    }
}
