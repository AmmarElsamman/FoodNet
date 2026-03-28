using System;
using Identity.API.Entities;

namespace Identity.API.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
