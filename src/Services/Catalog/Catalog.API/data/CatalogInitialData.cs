using Marten.Schema;

namespace Catalog.API.data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync())
        {
                     return;
        }

        //Marten UPSERT operation to insert initial data
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Wireless Mouse",
                Category = new List<string>{ "Electronics", "Accessories" },
                Description = "Ergonomic wireless mouse with adjustable DPI and long battery life.",
                ImageUrl = "https://example.com/images/wireless-mouse.jpg",
                Price = 29.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mechanical Keyboard",
                Category = new List<string>{ "Electronics", "Accessories" },
                Description = "High-quality mechanical keyboard with customizable RGB backlight.",
                ImageUrl = "https://example.com/images/mechanical-keyboard.jpg",
                Price = 89.50m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Noise-Cancelling Headphones",
                Category = new List<string>{ "Electronics", "Audio" },
                Description = "Over-ear headphones with active noise cancellation and rich sound.",
                ImageUrl = "https://example.com/images/noise-cancelling-headphones.jpg",
                Price = 199.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Smartphone Stand",
                Category = new List<string>{ "Accessories", "Mobile" },
                Description = "Adjustable aluminum smartphone stand for desk use.",
                ImageUrl = "https://example.com/images/smartphone-stand.jpg",
                Price = 15.00m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "4K Monitor",
                Category = new List<string>{ "Electronics", "Displays" },
                Description = "27-inch 4K UHD monitor with HDR and thin bezels.",
                ImageUrl = "https://example.com/images/4k-monitor.jpg",
                Price = 349.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "External SSD 1TB",
                Category = new List<string>{ "Storage", "Electronics" },
                Description = "Portable high-speed external SSD with USB-C connection.",
                ImageUrl = "https://example.com/images/external-ssd-1tb.jpg",
                Price = 129.00m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Fitness Tracker",
                Category = new List<string>{ "Wearables", "Fitness" },
                Description = "Water-resistant fitness tracker with heart-rate monitoring and sleep tracking.",
                ImageUrl = "https://example.com/images/fitness-tracker.jpg",
                Price = 59.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Espresso Machine",
                Category = new List<string>{ "Home", "Kitchen" },
                Description = "Compact pump espresso machine with steam wand for milk frothing.",
                ImageUrl = "https://example.com/images/espresso-machine.jpg",
                Price = 249.00m
            }
        };
    }
}
