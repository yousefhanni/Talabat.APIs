 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate_Modul;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        //to Seed data you must to add Database
        public async static Task SeedAnsync(StoreContext _dbContext )
        {
            //This (if) to make Dataseeding 1 time only 
            // Explian if Condition => if it Not Contain any element execute This Code
            if (_dbContext.ProductBrands.Count()==0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json ");

                //Convert from Json file(JavaScript) to List of C# objects(ProductBrand)

                //And Ensure that the JSON data and the ProductBrand class are compatible in terms of structure for successful deserialization
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count() > 0)
                {

                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);

                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }

            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json ");

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count() > 0)
                {

                    foreach (var category in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);

                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (_dbContext.Products.Count() == 0)
            {    
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json ");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count() > 0)
                {

                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);

                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Check if there are no delivery methods in the database
            if (_dbContext.DeliveryMethod.Count() == 0)
            {
                // Read delivery methods data from a JSON file
                var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json ");

                // Deserialize the JSON data into a list of DeliveryMethod objects
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                // Check if there are any delivery methods to add
                if (deliveryMethods?.Count() > 0)
                {
                    // Iterate through the delivery methods and add them to the database
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
