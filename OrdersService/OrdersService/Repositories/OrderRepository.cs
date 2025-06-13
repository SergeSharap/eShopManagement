using Dapper;
using Microsoft.Data.SqlClient;
using OrdersService.Models;
using System.Data;

namespace OrdersService.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }

    public class OrderRepository(IConfiguration configuration) : IOrderRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task AddAsync(Order order)
        {
            using var connection = Connection;

            string orderSql = "INSERT INTO Orders (Id, UserId, Status, CreatedAt) VALUES (@Id, @UserId, @Status, @CreatedAt)";
            await connection.ExecuteAsync(orderSql, order);

            if (order.Items.Any())
            {
                string itemsSql = "INSERT INTO OrderItems (Id, OrderId, ProductName, Quantity, Price) VALUES (@Id, @OrderId, @ProductName, @Quantity, @Price)";
                await connection.ExecuteAsync(itemsSql, order.Items);
            }
        }

        public async Task UpdateAsync(Order order)
        {
            using var connection = Connection;

            string orderSql = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            await connection.ExecuteAsync(orderSql, order);

            string deleteItemsSql = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
            await connection.ExecuteAsync(deleteItemsSql, new { OrderId = order.Id });

            if (order.Items.Any())
            {
                string insertItemsSql = "INSERT INTO OrderItems (Id, OrderId, ProductName, Quantity, Price) VALUES (@Id, @OrderId, @ProductName, @Quantity, @Price)";

                foreach (var item in order.Items)
                {
                    item.Id = Guid.NewGuid(); // Генерируем новый ID для товаров
                    item.OrderId = order.Id; // Привязываем к заказу
                }

                await connection.ExecuteAsync(insertItemsSql, order.Items);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = Connection;
            await connection.ExecuteAsync("DELETE FROM Orders WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            using var connection = Connection;

            var sql = @"SELECT * FROM Orders;
                        SELECT * FROM OrderItems;";

            using var multi = await connection.QueryMultipleAsync(sql);

            var orders = (await multi.ReadAsync<Order>()).ToList();
            var items = (await multi.ReadAsync<OrderItem>()).ToList();

            foreach (var order in orders)
            {
                order.Items = items.Where(i => i.OrderId == order.Id).ToList();
            }

            return orders;
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            using var connection = Connection;

            var sql = @"SELECT * FROM Orders WHERE Id = @Id;
                        SELECT * FROM OrderItems WHERE OrderId = @Id;";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

            var order = await multi.ReadFirstOrDefaultAsync<Order>();
            if (order != null)
                order.Items = (await multi.ReadAsync<OrderItem>()).ToList();

            return order;
        }
    }
}
