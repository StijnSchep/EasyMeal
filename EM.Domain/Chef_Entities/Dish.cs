using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EM.Domain.Utility;

namespace EM.Domain.Chef_Entities
{
    public class Dish : Indexable
    {
        [Required(ErrorMessage = Constants.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = Constants.PriceRequired)]
        public decimal Price { get; set; }

        public int DietaryRestrictions { get; set; } = 7;

        public int DishCategory { get; set; } = 7;

        [Required(ErrorMessage = Constants.DescriptionRequired)]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string ChefEmail { get; set; }
    }
}
