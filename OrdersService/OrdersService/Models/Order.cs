namespace OrdersService.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Status { get; set; } // "Pending", "Completed", "Canceled"
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> Items { get; set; } = [];
    }
}
