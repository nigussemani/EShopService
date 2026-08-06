using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
             new Coupon { Id = 1, ProductId = Guid.Parse("2375e70f-27de-42b0-96c4-ee587f6a68d3"), ProductName = "Wireless Mouse", Amount = 5, Description = "Clearance discount" },
             new Coupon { Id = 2, ProductId = Guid.Parse("cd52062a-019c-46d9-99ce-bbac6981761d"), ProductName = "Fitness Tracker", Amount = 10, Description = "Clearance discount" }
            );
    }
}
