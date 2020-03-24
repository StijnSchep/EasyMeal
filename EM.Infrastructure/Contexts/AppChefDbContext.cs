using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

using EM.Domain;
using EM.Domain.Chef_Entities;

namespace EM.Infrastructure.Contexts
{
    public class AppChefDbContext : DbContext
    {
        public AppChefDbContext(DbContextOptions<AppChefDbContext> options) 
            : base(options) { }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<WeekPlan> WeekPlans { get; set; }
        public DbSet<WeekDay> WeekDay { get; set; }
        public DbSet<Meal> Meal { get; set; }
    }
}
