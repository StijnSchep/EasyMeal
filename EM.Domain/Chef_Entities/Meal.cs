using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using EM.Domain.Utility;

namespace EM.Domain.Chef_Entities
{
    public class Meal : Indexable
    {

        public string Name { get; set; }

        [NotMapped] public Dish Starter { get; set; }
        [NotMapped] public Dish Main { get; set; }
        [NotMapped] public Dish Dessert { get; set; }

        [ForeignKey("Dishes")] public int StarterId { get; set; }
        [ForeignKey("Dishes")] public int MainId { get; set; }
        [ForeignKey("Dishes")] public int DessertId { get; set; }

        [ForeignKey("WeekDay")]
        public virtual int WeekDayId { get; set; }
        public virtual WeekDay WeekDay { get; set; }

        public int GetDietaryRepresentation()
        {
            int starter, main, dessert;
            if (Starter == null) { starter = 7; }
            else { starter = Starter.DietaryRestrictions; }

            if (Main == null) { main = 7; }
            else { main = Main.DietaryRestrictions; }

            if (Dessert == null) { dessert = 7; }
            else { dessert = Dessert.DietaryRestrictions; }

            return (int)Math.Pow(2, Constants.DietaryRestrictions.Length) - 1
                            & starter
                            & main
                            & dessert;
        }

        public List<string> DietaryRepresentation
        {
            get
            {
                String binary = Convert.ToString(GetDietaryRepresentation(), 2);

                if (binary.Length != Constants.DietaryRestrictions.Length)
                {
                    int difference = binary.Length - Constants.DietaryRestrictions.Length;

                    while (difference > 0)
                    {
                        binary = "0" + binary;
                        difference--;
                    }
                }

                List<String> result = new List<String>();

                int i = 0;
                foreach (char s in binary)
                {
                    if (s.Equals('1'))
                    {
                        result.Add(Constants.DietaryRestrictions.ElementAt(i));
                    }

                    i++;
                }

                return result;
            }
        }

        public decimal GetTotalPrice()
        {
            decimal result = 0;
            if (Starter != null) { result += Starter.Price; }
            if (Main != null) { result += Main.Price; }
            if (Dessert != null) { result += Dessert.Price; }

            return result;
        }
    }
}
