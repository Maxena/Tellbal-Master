using Data;
using Entities.Identity;
using Entities.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tellbal
{
    public static class Seeder
    {
        public static async Task SeedUsers(
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "Admin",
                    Description = "Admin"
                },
                new Role
                {
                    Name = "SuperAdmin",
                    Description = "SuperAdmin"
                }
            };

            foreach (var role in roles)
            {
                var existing = await roleManager.FindByNameAsync(role.Name);

                if (existing == null)
                {
                    var res = await roleManager.CreateAsync(role);
                }
            }

            var userData = await System.IO.File.ReadAllTextAsync("Seeds/seedUsers.json");

            var users = JsonSerializer.Deserialize<List<User>>(userData);

            if (users == null)
                return;

            foreach (var user in users)
            {
                try
                {
                    var existing = await userManager.FindByNameAsync(user.UserName);

                    if (existing == null)
                    {
                        var res = await userManager.CreateAsync(user, "Admin123@");
                        await userManager.AddToRoleAsync(user, "Admin");
                        await userManager.AddToRoleAsync(user, "SuperAdmin");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

            }
        }

        public static async Task SeedProvince(ApplicationDbContext dbContext)
        {
            var provincesData = await System.IO.File.ReadAllTextAsync("Seeds/Provinces.json");

            var provinces = JsonSerializer.Deserialize<List<Province>>(provincesData);

            if (provinces == null)
                return;

            if (await dbContext.Provinces.AnyAsync())
                return;

            foreach (var province in provinces)
            {
                try
                {
                    dbContext.Provinces.Add(province);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
        }

        public static async Task SeedCities(ApplicationDbContext dbContext)
        {
            var provincesData = await System.IO.File.ReadAllTextAsync("Seeds/Cities.json");

            var cities = JsonSerializer.Deserialize<List<City>>(provincesData);

            if (cities == null)
                return;

            if (await dbContext.Cities.AnyAsync())
                return;

            foreach (var city in cities)
            {
                try
                {
                    dbContext.Cities.Add(city);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

            }
        }

    }
}
