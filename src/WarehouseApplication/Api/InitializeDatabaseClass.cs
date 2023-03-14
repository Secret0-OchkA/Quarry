using infrastructura;
using Microsoft.EntityFrameworkCore;

namespace Order.Api
{
    public static class InitializeDatabaseClass
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<WarehouseDbContext>().Database.Migrate();
            }
        }
    }
}
