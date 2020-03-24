using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Infrastructure.Contexts;
using EM.Domain.Order_Entities;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EM.GUI_Order.Models
{
    public static class CustomerSeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppOrderDbContext context = app.ApplicationServices.GetRequiredService<AppOrderDbContext>();

            if(!context.Customers.Any())
            {
                context.Customers.Add(new Customer() {
                    EMail = "customer@gmail.com",
                    BirthDate = new DateTime(2000, 03, 24),
                    Name = "Customer Jack",
                    Address= "Lovensdijkstraat 61",
                    PostalCode="4000VM"
                });
                context.SaveChanges();
            }

        }
    }
}
