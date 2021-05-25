using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UniCoffeeShop.Enum;
using UniCoffeeShop.Models;

namespace UniCoffeeShop.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "JamesAdmin",
                Email = "jamesranderson36@gmail.com",
                FirstName = "James",
                LastName = "Randerson",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin36!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
            }
        }

        public static void Seed(ProductDBAccessLayer accessLayer)
        {
            var products = new List<Product>
            {
                new Product{Id="chocolate-bar",Name="Chocolate Bar",Description="A Tasty Chocolate bar",Price=0.50m,Picture = null},
                new Product{Id="coca-cola",Name="Cola",Description="A Fizzy Cola",Price=1.00m,Picture = null},
                new Product{Id="crisps",Name="Crisps",Description="A bag of Ready Salted Crisps",Price=0.50m,Picture = null},
                new Product{Id="lemonade",Name="Lemonade",Description="A fizzy glass of lemonade",Price=1.00m,Picture = null},
                new Product{Id="tea",Name="Tea",Description="A Cup of Tea",Price=1.50m,Picture = null},
                new Product{Id="white-coffee",Name="White Coffee",Description="A normal coffee",Price=1.99m,Picture = null},

            };

            products.ForEach(p => accessLayer.AddProduct(p));

        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}
