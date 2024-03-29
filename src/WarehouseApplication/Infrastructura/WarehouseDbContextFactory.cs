﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructura
{
    public class WarehouseDbContextFactory : IDesignTimeDbContextFactory<WarehouseDbContext>
    {
        public WarehouseDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<WarehouseDbContext> optBuilder = new DbContextOptionsBuilder<WarehouseDbContext>();
            optBuilder.UseNpgsql(args.FirstOrDefault() ?? "");

            return new WarehouseDbContext(optBuilder.Options);
        }
    }
}
