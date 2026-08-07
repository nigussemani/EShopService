using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Shopping cart cannot be null.");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("User name cannot be empty.");
        RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Shopping cart must contain at least one item.");
    }
}

public class StoreBasketCommandHandler 
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command, cancellationToken);

        await repository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await discountProtoService.GetDiscountAsync(
                new GetDiscountRequest() { ProductId = item.ProductId.ToString() }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}
