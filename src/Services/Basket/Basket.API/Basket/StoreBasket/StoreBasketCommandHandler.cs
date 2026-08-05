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

public class StoreBasketCommandHandler (IBasketRepository repository): ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await repository.StoreBasketAsync(command.Cart);
        return new StoreBasketResult(command.Cart.UserName);
    }
}
