using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try{
            // Seeding brands
            if (!context.ProductBrands.Any())
            {
                // reads json object in SeedData
                var brandsData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                // Converts json to list
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                // Populates entity with items
                foreach (var item in brands)
                {
                    context.ProductBrands.Add(item);
                }
                // Saves the changes in db
                await context.SaveChangesAsync();

            }

            // Seeding types
            if (!context.ProductTypes.Any())
            {
                // reads json object in SeedData
                var typesData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                // Converts json to list
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                // Populates entity with items
                foreach (var item in types)
                {
                    context.ProductTypes.Add(item);
                }
                // Saves the changes in db
                await context.SaveChangesAsync();

            }
            // Seeding products
            if (!context.Products.Any())
            {
                // reads json object in SeedData
                var productsData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                // Converts json to list
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                // Populates entity with items
                foreach (var item in products)
                {
                    context.Products.Add(item);
                }
                // Saves the changes in db
                await context.SaveChangesAsync();

            }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}