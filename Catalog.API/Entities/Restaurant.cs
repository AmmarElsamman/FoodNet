using System;

namespace Catalog.API.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public List<MenuItem> Items { get; set; } = [];
}
