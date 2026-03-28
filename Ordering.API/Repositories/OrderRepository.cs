using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Data;
using Ordering.API.Entities;
using Ordering.API.Interfaces;

namespace Ordering.API.Repositories;

public class OrderRepository(OrderContext context) : IOrderRepository
{
    public async Task AddOrderAsync(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await context.Orders.FindAsync(id);
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await context.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
    {
        return await context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }
}
