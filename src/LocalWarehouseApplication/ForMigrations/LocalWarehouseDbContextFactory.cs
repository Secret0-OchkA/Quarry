using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForMigrations
{
    public class LocalWarehouseDbContextFactory : IDesignTimeDbContextFactory<LocalWarehouseDbContext>
    {
        public LocalWarehouseDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<LocalWarehouseDbContext> optBuilder = new DbContextOptionsBuilder<LocalWarehouseDbContext>();
            optBuilder.UseSqlServer(args.ElementAt(0));

            return new LocalWarehouseDbContext(optBuilder.Options);
        }
    }
}
