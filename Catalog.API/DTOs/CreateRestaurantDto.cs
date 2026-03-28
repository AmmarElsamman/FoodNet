using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs;

public record CreateRestaurantDto([Required] string Name, [Required] string Address);
