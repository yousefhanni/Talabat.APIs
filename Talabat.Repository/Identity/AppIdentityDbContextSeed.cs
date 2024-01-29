using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        //Data seeding : is the process of populating a database with initial data
        //Make Seeding => (create User) and method that create User inside service (UserManager) 
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            // if Table Users Not Contain any element
            if (_userManager.Users.Count()==0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Yousef Hani",
                    Email = "ytaha6368@gmail.com",
                    UserName = "yousef.hani",
                    PhoneNumber = "01222535063"
                };

                await _userManager.CreateAsync(user,"Pa$$w0rd");
            }

            //To Update email of user 
            var existingUser = await _userManager.FindByEmailAsync("ytaha6368@gmail.com");

            if (existingUser != null)
            {      
                existingUser.Email = "yousef.hani@gmail.com";
                await _userManager.UpdateAsync(existingUser);
            }
        }
  
    }
}
