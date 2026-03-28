using System.ComponentModel.DataAnnotations;

namespace Identity.API.Dtos;

public record LoginDto
(
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password
);
