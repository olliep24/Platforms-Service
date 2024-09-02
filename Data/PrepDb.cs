using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding Data...");

                context.Platforms.AddRange(
                    new Platform()
                    {
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform()
                    {
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform()
                    {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computer Foundation",
                        Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Data already present");
            }
        }
    }
}