using System;
using Catalog.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Interfaces;

public interface ICatalogRepository
{
    Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync();
    Task<RestaurantDto?> GetRestaurantsByIdAsync(int id);
    Task<RestaurantDto> AddRestaurantAsync(CreateRestaurantDto dto);
    Task<bool> RestaurantExistsAsync(string name, string address);

}
