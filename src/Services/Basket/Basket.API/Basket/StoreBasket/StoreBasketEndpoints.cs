namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);
public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
       app.MapPost("/basket/{username}", async (string username, StoreBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("StoreBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store the shopping cart for a specific user")
        .WithDescription("This endpoint stores the shopping cart for a specific user based on their username.");
    }
}
