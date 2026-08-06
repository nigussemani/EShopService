using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbcontext, ILogger<DiscountService> logger) 
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.ProductId));
        if (coupon is null)
            coupon = new Models.Coupon() { Id = 1, ProductId = Guid.Parse(request.ProductId), Description = "No discount", ProductName = "No Discount", Amount = 0 };

        logger.LogInformation("Discount is retrieved for ProdcutId : {productId}, " +
            "Product Name: {productName}, Amount: {amount}", coupon.ProductId, coupon.ProductName, coupon.Amount);

        var couponModel = new CouponModel()
        {
            Id = coupon.Id,
            Amount = coupon.Amount,
            ProductId = coupon.ProductId.ToString(),
            Description = coupon.Description,
            ProductName = coupon.ProductName,
        };
        return couponModel;
    }
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    { 
        var coupon = new Coupon()
        {
            ProductId = Guid.Parse(request.Coupon.ProductId),
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        };
        dbcontext.Coupons.Add(coupon);
        await dbcontext.SaveChangesAsync();
        logger.LogInformation("Discount saved for ProductId:{productId}, " +
            "ProductName : {productName}, Amount :{amount}",
            request.Coupon.ProductId, request.Coupon.ProductName, request.Coupon.Amount);
        return request.Coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon =  await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductId == Guid.Parse(request.Coupon.ProductId));
        if(coupon is null)
        {
            coupon = new Coupon()
            {
                ProductId = Guid.Parse(request.Coupon.ProductId),
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };
            dbcontext.Coupons.Add(coupon);
        }
        else
        {
            coupon.ProductId = Guid.Parse(request.Coupon.ProductId);
            coupon.ProductName = request.Coupon.ProductName;
            coupon.Description = request.Coupon.Description;
            coupon.Amount = request.Coupon.Amount;
            dbcontext.Coupons.Update(coupon);
        }

        await dbcontext.SaveChangesAsync();


        logger.LogInformation("Discount updated for ProductId: {productId}, " +
            "ProductName: {productName}, Amount: {amount}", coupon.ProductId, coupon.ProductName, coupon.Amount);
      
        return new CouponModel()
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Amount = coupon.Amount,
            ProductId = coupon.ProductId.ToString(),
            Description = coupon.Description,
        };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupons = dbcontext.Coupons.Where(x => x.ProductId == Guid.Parse(request.ProductId)).ToArray();
        //foreach(var coupon in coupons)
        dbcontext.Coupons.RemoveRange(coupons);

        await dbcontext.SaveChangesAsync();

        logger.LogInformation("Discount deleted for the ProductId: {productId}", request.ProductId);

        return new DeleteDiscountResponse() { Success = true};
            
     }
}
