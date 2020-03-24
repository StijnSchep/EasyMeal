using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EM.Domain.Order_Entities;

namespace EM.Domain.Order_Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public Order()
        {
            OrderedMeals = new List<OrderedMeal>();
        }

        public DateTime StartDate { get; set; }

        public List<OrderedMeal> OrderedMeals { get; set; }

        [ForeignKey("Customer")]
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
