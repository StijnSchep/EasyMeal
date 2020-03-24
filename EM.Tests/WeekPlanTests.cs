using Xunit;
using EM.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EM.Domain.Chef_Entities;
using EM.Domain.Utility;

namespace EM.Tests
{
    public class WeekPlanTests
    {
        private static readonly Dish validStarter = new Dish()
        {
            Name = "Valid Starter Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 4,
            Description = "Valid Starter Description"
        };
        private static readonly Dish validMain = new Dish()
        {
            Name = "Valid main Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 2,
            Description = "Valid Main Description"
        };
        private static readonly Dish validDessert = new Dish()
        {
            Name = "Valid Dessert Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 1,
            Description = "Valid Dessert Description"
        };

        private readonly Meal validMeal = new Meal()
        {
            Starter = validStarter,
            Main = validMain,
            Dessert = validDessert
        };

        [Fact]
        public void Validate_WeekPlan_Valid_Test()
        {
            // Arrange
            WeekPlan validPlan = new WeekPlan()
            {
                StartDate = DateTime.Today.AddDays(1)
            };

            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(1) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(2) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(3) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(4) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(5) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(6) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(7) });

            foreach (WeekDay day in validPlan.WeekDays)
            {
                day.AddMeal(validMeal);
            }


            // Act
            string errorMessage = validPlan.Validate();

            // Assert
            Assert.Null(errorMessage);
        }

        [Fact]
        public void Validate_WeekPlan_Missing_Days()
        {
            // Arrange
            WeekPlan invalidPlan = new WeekPlan()
            {
                StartDate = DateTime.Today.AddDays(1)
            };
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(1) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(2) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(3) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(4) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(5) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(6) });
            invalidPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(7) });

            int i = 0;
            foreach (WeekDay day in invalidPlan.WeekDays)
            {
                if (i != 6)
                {
                    day.AddMeal(validMeal);
                }

                i++;
            }

          
            // Act
            string errorMessage = invalidPlan.Validate();

            // Assert
            Assert.Equal(Constants.WeekPlanInvalidSmall, errorMessage);
        }

        [Fact]
        public void Validate_WeekPlan_No_DietaryRepresentation()
        {
            // Arrange
            WeekPlan validPlan = new WeekPlan()
            {
                StartDate = DateTime.Today.AddDays(1)
            };

            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(1) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(2) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(3) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(4) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(5) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(6) });
            validPlan.WeekDays.Add(new WeekDay() { MealDate = DateTime.Today.AddDays(7) });

            // Starter does not represent any dietary restrictions
            // The meal is now unsuitable for any dietary restrictions, test should fail
            validMeal.Starter.DietaryRestrictions = 0;

            foreach (WeekDay day in validPlan.WeekDays)
            {
                day.AddMeal(validMeal);
            }

            // Act
            string errorMessage = validPlan.Validate();

            // Assert
            Assert.Equal(Constants.WeekPlanDietaryRepresentation + ", probleem bij dag 1", errorMessage);
        }

    }
}
