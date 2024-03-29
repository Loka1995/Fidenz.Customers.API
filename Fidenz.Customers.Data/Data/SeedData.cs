﻿using Fidenz.Customers.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fidenz.Customers.Data.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                var jsonString = File.ReadAllText("Data/UserData.json");
                if (jsonString != null)
                {
                    var user = System.Text.Json.JsonSerializer.Deserialize<List<User>>(jsonString);

                    if (user != null)
                    {
                        context.Users.AddRange(user);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
