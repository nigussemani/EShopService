namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest
(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageUrl,
    decimal Price
);

public record UpdateProductResponse(bool IsSuccess, string Message);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command, CancellationToken.None);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        }).WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update a product")
        .WithDescription("Updates an existing product in the catalog with the provided details.");
    }
}
