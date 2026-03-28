using System;
using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
