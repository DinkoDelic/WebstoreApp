using System.Linq;
using System.Threading.Tasks;
using Core.Entitites.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobberino",
                        Street = "1 Green Street",
                        City = "New York",
                        State = "NY",
                        Zipcode = "90210"
                    }
                };

                // password has to be complex otherwise we get an error
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}