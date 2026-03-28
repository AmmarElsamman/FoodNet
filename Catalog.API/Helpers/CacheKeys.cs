using System;

namespace Catalog.API.Helpers;

public static class CacheKeys
{
    public const string AllRestaurants = "catalog:restaurants:all";
    public static string Restaurant(int id) => $"catalog:restaurants:{id}";

}
