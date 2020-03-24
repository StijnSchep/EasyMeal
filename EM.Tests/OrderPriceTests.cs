using Xunit;
using EM.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EM.Domain.Order_Entities;
using EM.GUI_Order.Models.ViewModels;
using EM.DomainServices;
using System;

namespace EM.Tests
{
    public class OrderPriceTests
    {
        [Fact]
        public void Order_Price_Empty_Returns_Zero()
        {
            // Arrange
            Order order = new Order();
            DetailedOrderViewModel viewModel = new DetailedOrderViewModel(order);

            // Act
            decimal price = viewModel.CalculateTotalPrice();

            // Assert
            Assert.Equal((decimal)0.0, price);
        }

        [Fact]
        public void Order_Price_Single_Returns_Valid()
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

            Order order = new Order();
            order.OrderedMeals.Add(new OrderedMeal
            {
                MealDate = activeMonday,
                TotalPrice = (decimal)10.50
            });
            DetailedOrderViewModel viewModel = new DetailedOrderViewModel(order);

            // Act
            decimal price = viewModel.CalculateTotalPrice();

            // Assert
            Assert.Equal((decimal)10.50, price);
        }

        [Fact]
        public void Order_Price_Multiple_Returns_Valid()
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

            Order order = new Order();
            order.OrderedMeals.Add(new OrderedMeal
            {
                MealDate = activeMonday,
                TotalPrice = (decimal)10.50
            });
            order.OrderedMeals.Add(new OrderedMeal
            {
                MealDate = activeMonday.AddDays(1),
                TotalPrice = (decimal)20.25
            });
            DetailedOrderViewModel viewModel = new DetailedOrderViewModel(order);

            // Act
            decimal price = viewModel.CalculateTotalPrice();

            // Assert
            Assert.Equal((decimal)30.75, price);
        }



    }
}
