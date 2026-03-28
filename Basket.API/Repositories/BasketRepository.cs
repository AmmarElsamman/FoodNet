using System;
using System.Text.Json;
using Basket.API.Entities;
using Basket.API.Interfaces;
using StackExchange.Redis;

namespace Basket.API.Repositories;

public class BasketRepository(IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _redis = redis.GetDatabase();

    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
        var data = await _redis.StringGetAsync(userName.ToUpper());
        if (data.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<ShoppingCart>(data.ToString());
    }

    public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket)
    {
        string jsonBasket = JsonSerializer.Serialize(basket);

        await _redis.StringSetAsync(basket.UserName.ToUpper(), jsonBasket);

        return await GetBasketAsync(basket.UserName);
    }

    public Task DeleteBasketAsync(string userName)
    {
        return _redis.KeyDeleteAsync(userName.ToUpper());
    }

}
