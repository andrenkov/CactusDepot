using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CactusDepot.Shared.DataContext
{
    /// <summary>
    /// Add Db contect into the service collection
    /// </summary>
    public static class SeedsDbContextExtensions
    {
        public static IServiceCollection AddSeedsMySqlContext(this IServiceCollection services, string connStr)
        {
            try
            {
                services.AddDbContextPool<SeedsDbContext>(options => options
                .UseMySql(connStr, ServerVersion.AutoDetect(connStr),
                          b => b.MigrationsAssembly("CactusDepot.Common.DataContext"))
                .UseLoggerFactory(LoggerFactory.Create(b => b
                    .AddConsole()
                    .AddFilter(level => level >= LogLevel.Information)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                );
            }
            catch
            {
                //log message here not available
            }

            return services;
        }

    }
}
