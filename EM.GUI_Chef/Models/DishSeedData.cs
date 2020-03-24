using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using EM.Domain;
using EM.Domain.Chef_Entities;
using EM.Infrastructure.Contexts;

namespace EM.GUI_Chef.Models
{
    public class DishSeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppChefDbContext context = app.ApplicationServices.GetRequiredService<AppChefDbContext>();

            context.Database.Migrate();
            if (!context.Dishes.Any())
            {
                context.Dishes.AddRange(
                    new Dish()
                    {
                        Name = "Pasta Pesto",
                        Price = 10,
                        DietaryRestrictions = 7,
                        DishCategory = 7,
                        Description = "Pasta Pesto",
                        ChefEmail = "chef@gmail.com"
                    },
                    new Dish()
                    {
                        Name = "Pasta Carbonara",
                        Price = 10,
                        DietaryRestrictions = 7,
                        DishCategory = 7,
                        Description = "Pasta Carbonara",
                        ChefEmail = "chef@gmail.com"
                    },
                    new Dish()
                    {
                        Name = "Noedelsoep",
                        Price = 10,
                        DietaryRestrictions = 7,
                        DishCategory = 7,
                        Description = "Noedelsoep",
                        ChefEmail = "chef@gmail.com"
                    },
                    new Dish()
                    {
                        Name = "Couscous",
                        Price = 10,
                        DietaryRestrictions = 7,
                        DishCategory = 7,
                        Description = "Couscous",
                        ChefEmail = "chef@gmail.com"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
