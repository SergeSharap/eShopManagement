using Microsoft.AspNetCore.Mvc;
using OrdersService.Models;
using OrdersService.Repositories;

namespace OrdersService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(IOrderRepository orderRepository) : ControllerBase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreatedAt = DateTime.UtcNow;
            foreach (var item in order.Items)
            {
                item.Id = Guid.NewGuid();
                item.OrderId = order.Id;
            }

            await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Order order)
        {
            var existingUser = await _orderRepository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            order.Id = id;
            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUser = await _orderRepository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
