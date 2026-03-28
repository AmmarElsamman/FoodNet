using System;

namespace FoodNet.Contracts;

public class BasketCheckoutEvent
{
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? AddressLine { get; set; }

    public string? CardName { get; set; }
    public string? CardNumber { get; set; }
    public string? Expiration { get; set; }
    public string? CVV { get; set; }

    public List<BasketItemDetails> Items { get; set; } = [];

}

public class BasketItemDetails
{
    public int Quantity { get; set; }
    public string ItemName { get; set; } = default!;
    public decimal Price { get; set; }
    public string ItemId { get; set; } = default!;
}
