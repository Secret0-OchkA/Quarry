using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructura
{
    public class OrderAppDbContextFactory : IDesignTimeDbContextFactory<OrderAppDbContext>
    {
        public OrderAppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<OrderAppDbContext> optBuilder = new DbContextOptionsBuilder<OrderAppDbContext>();
            optBuilder.UseSqlServer(args.FirstOrDefault() ?? "");

            return new OrderAppDbContext(optBuilder.Options);
        }
    }
}
