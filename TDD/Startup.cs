using Microsoft.EntityFrameworkCore;
using TDD.Data;

namespace TDD
{
    public static class Startup
    {
        public static IServiceCollection UseStarup(this IServiceCollection services)
        {

            services.AddDbContextPool<BuDbContext>(options =>
            {

              options.UseSqlite("Filename=BuDatabase.db");
            }
            );

            return services;
        }

  
    }
}