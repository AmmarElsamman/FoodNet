using System;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Data;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Restaurant>().HasData(
            new Restaurant { Id = 1, Name = "Pizza Place", Address = "123 Main St" },
            new Restaurant { Id = 2, Name = "Burger Joint", Address = "456 Elm St" }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Pepperoni Pizza", Price = 12.99m, RestaurantId = 1 },
            new MenuItem { Id = 2, Name = "Cheese Pizza", Price = 10.99m, RestaurantId = 1 },
            new MenuItem { Id = 3, Name = "Veggie Burger", Price = 9.99m, RestaurantId = 2 },
            new MenuItem { Id = 4, Name = "Bacon Burger", Price = 11.99m, RestaurantId = 2 }
        );
    }
}
