using System;
using Ordering.API.Entities;

namespace Ordering.API.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task DeleteOrderAsync(int id);
    Task AddOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);

}
