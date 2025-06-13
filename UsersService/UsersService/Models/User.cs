namespace UsersService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } // "Admin", "User"
        public DateTime CreatedAt { get; set; }
    }
}
