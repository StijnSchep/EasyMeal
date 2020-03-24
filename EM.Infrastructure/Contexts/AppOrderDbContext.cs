using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

using EM.Domain;
using EM.Domain.Order_Entities;

namespace EM.Infrastructure.Contexts
{
    public class AppOrderDbContext : DbContext
    {
        public AppOrderDbContext(DbContextOptions<AppOrderDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
}