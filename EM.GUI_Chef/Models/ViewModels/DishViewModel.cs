using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using EM.Domain;
using EM.Domain.Chef_Entities;
using EM.Domain.Utility;

namespace EM.GUI_Chef.Models.ViewModels
{
    public class DishViewModel
    {

        public Dish Dish { get; set; }

        public List<String> Categories
        {
            get
            {
                String binary = Convert.ToString(Dish.DishCategory, 2);

                if (binary.Length != Constants.categories.Length)
                {
                    int difference = Constants.categories.Length - binary.Length;
                    while(difference > 0)
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
                        result.Add(Constants.categories.ElementAt(i));
                    }

                    i++;
                }

                return result;
            }
        }

        public List<String> DietaryRepresentation
        {
            get
            {
                String binary = Convert.ToString(Dish.DietaryRestrictions, 2);

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

        public IFormFile Image { get; set; }

        public int CategorieInt
        {
            get
            {
                string result = "";
                result += isStarter ? "1" : "0";
                result += isMain ? "1" : "0";
                result += isDessert ? "1" : "0";

                return Convert.ToInt32(result, 2);
            }
        }

        public int DietaryInt
        {
            get
            {
                string result = "";
                result += salt ? "1" : "0";
                result += diabetes ? "1" : "0";
                result += gluten ? "1" : "0";

                return Convert.ToInt32(result, 2);
            }
        }

        public void checkBooleans()
        {
            if (Dish.DietaryRestrictions != 7)
            {
                string binary = Convert.ToString(Dish.DietaryRestrictions, 2);
                while (binary.Length < 3)
                {
                    binary = "0" + binary;
                }
                salt = binary.ElementAt(0).Equals('1');
                diabetes = binary.ElementAt(1).Equals('1');
                gluten = binary.ElementAt(2).Equals('1');
            }

            if (Dish.DishCategory != 7)
            {
                string binary = Convert.ToString(Dish.DishCategory, 2);
                while(binary.Length < 3)
                {
                    binary = "0" + binary;
                }

                isStarter = binary.ElementAt(0).Equals('1');
                isMain = binary.ElementAt(1).Equals('1');
                isDessert = binary.ElementAt(2).Equals('1');
            }
        }

        public Boolean isStarter { get; set; } = true;
        public Boolean isMain { get; set; } = true;
        public Boolean isDessert { get; set; } = true;

        public Boolean salt { get; set; } = true;
        public Boolean diabetes { get; set; } = true;
        public Boolean gluten { get; set; } = true;

        public override string ToString()
        {
            return Dish.Name;
        }

    }
}
