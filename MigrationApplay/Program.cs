
using ForMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();

try
{
    Console.WriteLine("apply migration");

    using (WarehouseDbContext context = new WarehouseDbContextFactory()
        .CreateDbContext(new string[] { configuration.GetConnectionString("LocalWarehouseDbContext")! }))
        await context.Database.MigrateAsync().ConfigureAwait(false);

    Console.WriteLine("end migration");
}
catch (Exception ex)
{ Console.WriteLine(ex.ToString()); }

System.Environment.Exit(0);