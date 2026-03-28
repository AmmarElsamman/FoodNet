using System.ComponentModel.DataAnnotations;

namespace Identity.API.Dtos;

public record RegisterDto
(
    [Required] string FirstName,
    [Required] string LastName,
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password
);
