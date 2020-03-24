using Xunit;
using EM.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EM.Domain.Chef_Entities;

namespace EM.Tests
{
    public class MealTests
    {
        private readonly Dish validStarter = new Dish()
        {
            Name = "Valid Starter Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 4,
            Description = "Valid Starter Description"
        };
        private readonly Dish validMain = new Dish()
        {
            Name = "Valid main Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 2,
            Description = "Valid Main Description"
        };
        private readonly Dish validDessert = new Dish()
        {
            Name = "Valid Dessert Dish",
            Price = 10.0M,
            DietaryRestrictions = 7,
            DishCategory = 1,
            Description = "Valid Dessert Description"
        };

        [Fact]
        public void Meal_Returns_Correct_DietaryRepresentation()
        {
            // Arrange
            Meal meal = new Meal()
            {
                Starter = validStarter,
                Main = validMain,
                Dessert = validDessert
            };

            // Act
            int representation = meal.GetDietaryRepresentation();

            // Assert
            Assert.Equal(7, representation);
        }



    }
}
