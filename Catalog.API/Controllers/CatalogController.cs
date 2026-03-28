using System.Text.Json;
using Catalog.API.Data;
using Catalog.API.DTOs;
using Catalog.API.Entities;
using Catalog.API.Helpers;
using Catalog.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class CatalogController(ICatalogRepository catalogRepository, IDistributedCache cache) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
        {
            var cacheKey = CacheKeys.AllRestaurants;

            var cached = await cache.GetStringAsync(cacheKey);
            if (cached != null)
                return Ok(JsonSerializer.Deserialize<IEnumerable<RestaurantDto>>(cached));

            var result = await catalogRepository.GetRestaurantsAsync();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), options);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurantById(int id)
        {

            var cacheKey = CacheKeys.Restaurant(id);

            var cached = await cache.GetStringAsync(cacheKey);
            if (cached != null)
                return Ok(JsonSerializer.Deserialize<RestaurantDto>(cached));


            var result = await catalogRepository.GetRestaurantsByIdAsync(id);
            if (result == null) return NotFound();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), options);

            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> AddRestaurant(CreateRestaurantDto dto)
        {
            if (await catalogRepository.RestaurantExistsAsync(dto.Name, dto.Address))
                return Conflict("A restaurant with the same name and address already exists.");

            var result = await catalogRepository.AddRestaurantAsync(dto);

            await cache.RemoveAsync(CacheKeys.AllRestaurants);

            return CreatedAtAction(nameof(GetRestaurantById), new { id = result.Id }, result);
        }
    }
}
