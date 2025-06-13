using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UsersService.Models;

namespace UsersService.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }

    public class UserRepository(IConfiguration configuration) : IUserRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var db = Connection;
            return await db.QueryAsync<User>("SELECT * FROM Users");
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var db = Connection;
            return await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id } );
        }

        public async Task AddAsync(User user)
        {
            using var db = Connection;
            string sql = "INSERT INTO Users (Id, FullName, Email, PasswordHash, Role, CreatedAt) " +
                         "VALUES (@Id, @FullName, @Email, @PasswordHash, @Role, @CreatedAt)";
            await db.ExecuteAsync(sql, user);
        }

        public async Task UpdateAsync(User user)
        {
            using var db = Connection;
            string sql = "UPDATE Users SET FullName = @FullName, Email = @Email, PasswordHash = @PasswordHash, Role = @Role WHERE Id = @Id";
            await db.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var db = Connection;
            await db.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
        }
    }
}
