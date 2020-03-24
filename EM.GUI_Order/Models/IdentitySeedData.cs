using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EM.GUI_Order.Models
{
    public static class IdentitySeedData
    {
        private const string Email = "customer@gmail.com";
        private const string Password = "Secret123$";
        private const string PhoneNumber = "+31 6 83446623";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                UserManager<IdentityUser> userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

                IdentityUser user = await userManager.FindByIdAsync(Email);
                if (user == null)
                {
                    user = new IdentityUser {
                        UserName = Email,
                        Email = Email,
                        PhoneNumber = PhoneNumber};
                    await userManager.CreateAsync(user, Password);
                }
            } 
        }
    }
}
