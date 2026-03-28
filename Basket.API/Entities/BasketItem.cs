using System;

namespace Basket.API.Entities;

public class BasketItem
{
    public int Quantity { get; set; }
    public string ItemName { get; set; } = default!;
    public decimal Price { get; set; }
    public required string ItemId { get; set; }
}
