using System;
using System.Collections.Generic;
using System.Text;

using EM.Domain.Chef_Entities;

namespace EM.Domain.API_Entities
{
    public class APIMeal
    {
        public APIMeal(Meal Meal)
        {
            Name = Meal.Name;
            StarterId = Meal.StarterId;
            MainId = Meal.MainId;
            DessertId = Meal.DessertId;
            Id = Meal.Id;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int StarterId { get; set; }
        public int MainId { get; set; }
        public int DessertId { get; set; }
    }
}
