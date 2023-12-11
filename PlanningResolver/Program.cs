using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaninngResolver.Domain.Application;
using PlaninngResolver.Domain.Application.Rules;
using PlaninngResolver.Domain.Infrastructure.Persistence;
using PlaninngResolver.Domain.Infrastructure.Persistence.Repositories;
using PlaninngResolver.Domain.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
         var serviceProvider = new ServiceCollection()
            .AddSingleton(BuildConfiguration())
            .AddDbContext<PlanningDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AccessDb")))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IResolverService), typeof(ResolverService))
            // Add other services
            .BuildServiceProvider();

        var dbContext = serviceProvider.GetService<IResolverService>();

        var years = dbContext.GeneratingPlanning(1, 1, 1);
        // Now you can use connectionString to create your DbContext
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}