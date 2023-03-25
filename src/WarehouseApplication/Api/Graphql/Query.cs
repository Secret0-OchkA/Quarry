using Domain;
using infrastructura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Queries
{
    public class Query
    {
        [UseFiltering]
        [UseOffsetPaging(DefaultPageSize = 10, MaxPageSize = 100)]
        public IQueryable<Product> Products([Service] WarehouseDbContext context) => context.Products;
    }
}
