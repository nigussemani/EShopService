namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsDeleted, string message);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
    }
}

public class DeleteProductCommandHandler(IDocumentSession session)
 : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.ProductId) ?? throw new ProductNotFoundException(request.ProductId);
        session.Delete<Product>(product);
        await session.SaveChangesAsync();
        return new DeleteProductResult(true, $"Product with ID {request.ProductId} has been deleted successfully.");
    }
}
