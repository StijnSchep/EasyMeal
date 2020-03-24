using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EM.Domain.Chef_Entities;

namespace EM.Domain.Order_Entities
{
    public class OrderedMeal
    {
        [Key]
        public int Id { get; set; }

        public int MealId { get; set; }
        [NotMapped] public Meal Meal { get; set; }

        public bool IncludeStarter { get; set; }
        public bool IncludeDessert { get; set; }

        public DateTime MealDate { get; set; }
        public string Size { get; set; } = "M";

        public decimal TotalPrice { get; set; }

        public decimal CalculateTotalPrice()
        {
            decimal basePrice = Meal.GetTotalPrice();

            if (!IncludeStarter)
            {
                basePrice -= Meal.Starter.Price;
            }

            if(!IncludeDessert)
            {
                basePrice -= Meal.Dessert.Price;
            }

            if(Size == "S")
            {
                basePrice = Decimal.Multiply((decimal)0.8, basePrice);
            }

            if(Size == "L")
            {
                basePrice = Decimal.Multiply((decimal)1.2, basePrice);
            }


            return basePrice;
        }

        [ForeignKey("Order")]
        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
