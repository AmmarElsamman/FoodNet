using System;

namespace Basket.API.Entities;

public class ShoppingCart
{
    public string? UserName { get; set; }
    public List<BasketItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);


    public ShoppingCart() { }
    public ShoppingCart(string userName) => UserName = userName;

}
