// External
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Xunit;
using Moq;


using EM.Domain.Order_Entities;
using EM.GUI_Order.Controllers;
using EM.GUI_Order.Models.ViewModels;
using EM.DomainServices;

namespace EM.Tests
{
    public class InvoiceTests
    {
        [Fact]
        public async void Invoice_Empty_Returns_Zero()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today.AddMonths(1)
                }));

            // Return empty list
            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.False(HasExtraDiscount);
            Assert.Equal((decimal)0.0, birthdayDiscount);
            Assert.Equal((decimal)0.0, subtotal);
            Assert.Equal((decimal)0.0, total);
        }

        [Fact]
        public async void Invoice_Single_Meal_No_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)10.50,
                        MealDate = DateTime.Today.AddDays(1)
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.False(HasExtraDiscount);
            Assert.Equal((decimal)0.0, birthdayDiscount);
            Assert.Equal((decimal)10.50, subtotal);
            Assert.Equal((decimal)10.50, total);
        }

        [Fact]
        public async void Invoice_Single_Meal_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)10.50,
                        MealDate = DateTime.Today
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.False(HasExtraDiscount);
            Assert.Equal((decimal)10.50, birthdayDiscount);
            Assert.Equal((decimal)10.50, subtotal);
            Assert.Equal((decimal)0.0, total);
        }

        [Fact]
        public async void Invoice_Multiple_Meals_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)10.50,
                        MealDate = DateTime.Today
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(1)
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.False(HasExtraDiscount);
            Assert.Equal((decimal)10.50, birthdayDiscount);
            Assert.Equal((decimal)26.00, subtotal);
            Assert.Equal((decimal)15.50, total);
        }

        [Fact]
        public async void Invoice_Fifteen_Meals_No_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today.AddMonths(1)
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(1)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(2)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(3)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(4)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(5)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(6)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(7)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(8)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(9)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(10)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(11)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(12)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(13)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(14)
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.True(HasExtraDiscount);
            Assert.Equal((decimal)0.0, birthdayDiscount);
            Assert.Equal((decimal)232.50, subtotal);
            Assert.Equal((decimal)209.25, total);
        }

        [Fact]
        public async void Invoice_Fifteen_Meals_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(1)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(2)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(3)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(4)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(5)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(6)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(7)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(8)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(9)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(10)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(11)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(12)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(13)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(14)
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.False(HasExtraDiscount);
            Assert.Equal((decimal)15.50, birthdayDiscount);
            Assert.Equal((decimal)232.50, subtotal);
            Assert.Equal((decimal)217.00, total);
        }

        [Fact]
        public async void Invoice_Sixteen_Meals_BirthDate()
        {
            // Arrange
            var mock = new Mock<IOrderRepository>();
            mock.Setup(m => m.GetCustomer(null, null))
                .Returns(Task.FromResult(new Customer
                {
                    EMail = "mail",
                    BirthDate = DateTime.Today
                }));

            mock.Setup(m => m.GetOrderedMealsForMonth(It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<OrderedMeal>()
                {
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(1)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(2)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(3)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(4)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(5)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(6)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(7)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(8)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(9)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(10)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(11)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(12)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(13)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(14)
                    },
                    new OrderedMeal
                    {
                        TotalPrice = (decimal)15.50,
                        MealDate = DateTime.Today.AddDays(15)
                    }
                }
            ));

            var controller = new OrdersController(null, null, mock.Object);

            // Act
            var actionResult = await controller.ViewInvoice() as ViewResult;
            var model = actionResult.ViewData.Model as InvoiceViewModel;
            decimal subtotal = model.CalculateSubtotal();
            decimal total = model.CalculateTotalPrice();
            decimal birthdayDiscount = model.BirthDateDiscount();
            bool HasExtraDiscount = model.HasPlusFifteenOrders();

            // Assert
            Assert.True(HasExtraDiscount);
            Assert.Equal((decimal)15.50, birthdayDiscount);
            Assert.Equal((decimal)248.00, subtotal);
            Assert.Equal((decimal)209.25, total);
        }



    }
}
