using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Entities;
using Catalog.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Catalog.API.Repositories;

public class CatalogRepository(CatalogContext context) : ICatalogRepository
{
    private readonly ResiliencePipeline _pipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 5,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Exponential
        })
        .Build();

    public async Task<RestaurantDto> AddRestaurantAsync(CreateRestaurantDto dto)
    {
        var restaurant = new Restaurant
        {
            Name = dto.Name,
            Address = dto.Address
        };

        context.Restaurants.Add(restaurant);
        await context.SaveChangesAsync();

        var result = new RestaurantDto(
                restaurant.Id,
                restaurant.Name,
                restaurant.Address,
                []
            );
        return result;
    }

    public async Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync()
    {
        return await _pipeline.ExecuteAsync(async ct =>
        {
            var restaurants = await context.Restaurants
                .Include(r => r.Items)
                .ToListAsync(ct);

            return restaurants.Select(r => new RestaurantDto(
                r.Id,
                r.Name,
                r.Address,
                r.Items.Select(i => new MenuItemDto(i.Id, i.Name, i.Price)).ToList()
            ));
        });
    }

    public async Task<RestaurantDto?> GetRestaurantsByIdAsync(int id)
    {

        return await _pipeline.ExecuteAsync(async ct =>
        {
            var restaurant = await context.Restaurants
                 .Include(r => r.Items)
                 .FirstOrDefaultAsync(r => r.Id == id, ct);

            if (restaurant == null) return null;
            return new RestaurantDto(
                restaurant.Id,
                restaurant.Name,
                restaurant.Address,
                restaurant.Items.Select(i => new MenuItemDto(i.Id, i.Name, i.Price)).ToList()
            );

        });
    }
    public async Task<bool> RestaurantExistsAsync(string name, string address)
    {
        return await context.Restaurants.AnyAsync(r => r.Name == name && r.Address == address);
    }
}
