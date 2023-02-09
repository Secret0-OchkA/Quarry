using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructura
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IDbConnection dbConnection;

        public ProductRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<int> Cereate(Product entity)
        {
            string query = $"INSERT INTO [Products]" +
                $"(" +
                $"[{nameof(Product.Id)}]," +
                $"[{nameof(Product.Name)}]," +
                $"[{nameof(Product.Description)}]," +
                $"[{nameof(Product.Cost)}]," +
                $"[{nameof(Product.Count)}]," +
                $"[{nameof(Product.Unit)}]" +
                $")" +
                $"VALUES" +
                $"(" +
                $"@{nameof(Product.Id)}," +
                $"@{nameof(Product.Name)}," +
                $"@{nameof(Product.Description)}," +
                $"@{nameof(Product.Cost)}," +
                $"@{nameof(Product.Count)}," +
                $"@{nameof(Product.Unit)}" +
                $")";

            return await dbConnection.ExecuteAsync(query,entity);
        }

        public async Task<int> Delete(Guid Id)
        {
            string query = $"DELETE FROM [Products]" +
                $"WHERE [{nameof(Product.Id)}] = @{nameof(Product.Id)}";
            return await dbConnection.ExecuteAsync(query, new { Id });
        }
        public async Task<IEnumerable<Product>> GetAll(int page = 0, int pageSize = 10)
        {
            return await dbConnection.QueryAsync<Product>($"SELECT * FROM [Products] ORDER BY id OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY;");
        }

        public async Task<Product> GetById(Guid id)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<Product>($"SELECT * FROM [Products] WHERE [{nameof(Product.Id)}] = @{nameof(Product.Id)}",
                new {id});
        }

        public async Task<int> Update(Product entity)
        {
            string query = $"UPDATE [Products] SET" +
                $"[{nameof(Product.Name)}] = @{nameof(Product.Name)}," +
                $"[{nameof(Product.Description)}] = @{nameof(Product.Description)}," +
                $"[{nameof(Product.Cost)}] = @{nameof(Product.Cost)}," +
                $"[{nameof(Product.Count)}] = @{nameof(Product.Count)}," +
                $"[{nameof(Product.Unit)}] = @{nameof(Product.Unit)} " +
                $"WHERE [{nameof(Product.Id)}] = @{nameof(Product.Id)}";

            return await dbConnection.ExecuteAsync(query,entity);
        }
    }
}
           