using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Dtos;
using Ordering.API.Entities;
using Ordering.API.Interfaces;
using Ordering.API.Mappers;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderRepository orderRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await orderRepository.GetOrdersAsync();
            return Ok(orders.Select(order => order.ToOrderDto()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order.ToOrderDto());
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersOfUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await orderRepository.GetOrdersByUserAsync(userId);
            return Ok(orders.Select(order => order.ToOrderDto()));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await orderRepository.DeleteOrderAsync(id);
            return NoContent();
        }

    }
}
