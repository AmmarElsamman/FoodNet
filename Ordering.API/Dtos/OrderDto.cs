using System;
using Ordering.API.Entities;

namespace Ordering.API.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }

    public List<OrderItemDetails> Items { get; set; } = [];
}
