using System;

namespace Ordering.API.Entities;

public class OrderItemDetails
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public string ItemName { get; set; } = default!;
    public decimal Price { get; set; }
    public string ItemId { get; set; } = default!;
}
