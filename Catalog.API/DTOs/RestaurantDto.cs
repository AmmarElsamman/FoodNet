namespace Catalog.API.DTOs;

public record RestaurantDto(int Id, string Name, string Address, List<MenuItemDto> Items);
