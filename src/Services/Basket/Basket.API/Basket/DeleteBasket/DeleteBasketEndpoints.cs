namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketReques(string UserName) : ICommand<DeleteBasketResponse>;
public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var command  = new DeleteBasketCommand(userName);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete a basket for a specific user")
        .WithDescription("Deletes the basket associated with the provided user name. " +
        "Returns a success response if the basket was deleted successfully, " +
        "or an error response if the basket was not found or could not be deleted.");
    }
}
