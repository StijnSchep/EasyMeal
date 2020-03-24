using Xunit;
using EM.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EM.Domain.Order_Entities;
using EM.DomainServices;
using System;

namespace EM.Tests
{
    public class OrderDayCountTests
    {
        
        [Fact]
        public void Order_Meal_Count_Empty_Returns_False()
        {
            // Arrange
            Order testOrder = new Order();

            // Act
            bool HasEnoughOrders = testOrder.HasEnoughOrdered();

            // Assert
            Assert.False(HasEnoughOrders);
        }

        [Fact]
        public void Order_Meal_Count_Weekend_Only_Returns_False()
        {
            // Arrange
            DateTime activeMonday;
            if ((int)DateTime.Today.DayOfWeek == 0)
            {
                activeMonday = DateTime.Today.AddDays(-6);
            }
            else
            {
                activeMonday = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek);
            }

            DateTime activeSat = activeMonday.AddDays(5);
            DateTime activeSun = activeMonday.AddDays(6);

            Order testOrder = new Order()
            {
                StartDate = activeMonday
            };

            testOrder.OrderedMeals.AddRange( new List<OrderedMeal> {
                    new OrderedMeal
                    {
                        MealDate = activeSat
                    },
                    new OrderedMeal
                    {
                        MealDate = activeSun
                    }
                }
             );

            // Act
            bool HasEnoughOrders = testOrder.HasEnoughOrdered();

            // Assert
            Assert.False(HasEnoughOrders);
        }

        [Fact]
        public void Order_Meal_Count_Unbalanced_Returns_False()
        {
            // Arrange
            DateTime activeMonday;
            if ((int)DateTime.Today.DayOfWeek == 0)
            {
                activeMonday = DateTime.Today.AddDays(-6);
            }
            else
            {
                activeMonday = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek);
            }

            DateTime activeTue = activeMonday.AddDays(1);
            DateTime activeSat = activeMonday.AddDays(5);
            DateTime activeSun = activeMonday.AddDays(6);

            Order testOrder = new Order()
            {
                StartDate = activeMonday
            };

            testOrder.OrderedMeals.AddRange(new List<OrderedMeal> {
                    new OrderedMeal
                    {
                        MealDate = activeMonday
                    },
                    new OrderedMeal
                    {
                        MealDate = activeTue
                    },
                    new OrderedMeal
                    {
                        MealDate = activeSat
                    },
                    new OrderedMeal
                    {
                        MealDate = activeSun
                    }
                }
             );

            // Act
            bool HasEnoughOrders = testOrder.HasEnoughOrdered();

            // Assert
            Assert.False(HasEnoughOrders);
        }

        [Fact]
        public void Order_Meal_Count_Balanced_Returns_True()
        {
            // Arrange
            DateTime activeMonday;
            if ((int)DateTime.Today.DayOfWeek == 0)
            {
                activeMonday = DateTime.Today.AddDays(-6);
            }
            else
            {
                activeMonday = DateTime.Today.AddDays(1 - (int)DateTime.Today.DayOfWeek);
            }

            Order testOrder = new Order()
            {
                StartDate = activeMonday
            };

            testOrder.OrderedMeals.AddRange(new List<OrderedMeal> {
                    new OrderedMeal
                    {
                        MealDate = activeMonday
                    },
                    new OrderedMeal
                    {
                        MealDate = activeMonday.AddDays(1)
                    },
                    new OrderedMeal
                    {
                        MealDate = activeMonday.AddDays(2)
                    },
                    new OrderedMeal
                    {
                        MealDate = activeMonday.AddDays(3)
                    }
                }
             );

            // Act
            bool HasEnoughOrders = testOrder.HasEnoughOrdered();

            // Assert
            Assert.True(HasEnoughOrders);
        }


    }
}
