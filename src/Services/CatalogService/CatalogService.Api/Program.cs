using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore;


namespace CatalogService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.MigrateDbContext<CatalogContext>((context, services) =>
                {
                    var env=services.GetService<IWebHostEnvironment>();
                    var logger=services.GetService<ILogger<CatalogContextSeed>>();
                    new CatalogContextSeed()
                    .SeedAsync(context, env, logger)
                    .Wait();
                });

            hostBuilder.Run();
        }

        static IWebHost CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .UseWebRoot("Pics")
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .Build();
        }
    }
}
