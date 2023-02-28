using infrastructura;
using Microsoft.EntityFrameworkCore;

namespace Order.Api
{
    public class InitializeDatabaseClass
    {
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<WarehouseDbContext>().Database.Migrate();
            }
        }
    }
}
