using Data;
using Entities.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Tellbal
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var isProd = environment == "Production";

            var logger = NLogBuilder.ConfigureNLog(isProd ? "nlog.production.config" : "nlog.config").GetCurrentClassLogger();





            var host = CreateHostBuilder(args)
                //.UseDefaultServiceProvider(x => x.ValidateScopes = false)
                .Build();

            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var categoryService = services.GetService<ICategoryService>();

            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<Role>>();

            try
            {
                // await context.Database.EnsureDeletedAsync();

                //await context.Database.EnsureCreatedAsync();

                //await context.Database.MigrateAsync();

                await categoryService.FirstRunAfterBoot();

                await Seeder.SeedUsers(userManager, roleManager);

                await Seeder.SeedProvince(context);

                await Seeder.SeedCities(context);

                logger.Debug("init main");
                await host.RunAsync();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseNLog();
                webBuilder.ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(LogLevel.Trace);
                 })
                .UseNLog();
            });
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //    logging.SetMinimumLevel(LogLevel.Trace);
        //});
        //.UseNLog();  // NLog: Setup NLog for Dependency injection
    }
}
