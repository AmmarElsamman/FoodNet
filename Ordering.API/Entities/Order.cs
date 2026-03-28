using System;

namespace Ordering.API.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }

    public List<OrderItemDetails> Items { get; set; } = [];
}


public enum OrderStatus
{
    Submitted = 1,
    Confirmed = 2,
    Cancelled = 3
}