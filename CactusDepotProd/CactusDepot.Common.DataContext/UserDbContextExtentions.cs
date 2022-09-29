using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CactusDepot.Shared.DataContext
{
    /// <summary>
    /// Add Db contect into the service collection
    /// </summary>
    public static class UserDbContextExtensions
    {
        public static IServiceCollection AddUserMySqlContext(this IServiceCollection services, string connStr)
        {
            try
            {
                services.AddDbContextPool<UserDbContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));
            }
            catch
            {
                //log message here not available
            }

            return services;
        }

    }
}
