using E_commerce_System.Entities;
using System.Text.Json;

namespace E_commerce_System.Data
{
    public static class StorecontextSeeding
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if(!context.Categories.Any())
            {
                var categorirsData = File.ReadAllText("C:\\Users\\basma\\Desktop\\study\\WepApiTasks\\E-commerce System\\E-commerce System\\Data\\seeding\\categries.json");

                var categories = JsonSerializer.Deserialize<List<Category>>(categorirsData);

                if (categories is null)
                    return;


             await   context.Categories.AddRangeAsync(categories);

                await context.SaveChangesAsync();


            
            }
        }
    }
}
