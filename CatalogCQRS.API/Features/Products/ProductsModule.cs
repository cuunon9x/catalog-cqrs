using Carter;
using CatalogCQRS.Application.Features.Products.Commands;
using CatalogCQRS.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogCQRS.API.Features.Products;

public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        group.MapPost("/", async (CreateProductCommand command, ISender mediator) =>
        {
            var productId = await mediator.Send(command);
            return Results.Created($"/api/products/{productId}", productId);
        })
        .WithName("CreateProduct")
        .ProducesValidationProblem()
        .Produces<Guid>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            try
            {
                return Results.Ok(await mediator.Send(new GetProductByIdQuery(id)));
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/", async (ISender mediator) =>
            Results.Ok(await mediator.Send(new GetAllProductsQuery())))
        .WithName("GetAllProducts")
        .Produces<IEnumerable<GetProductByIdResponse>>();

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest();
            try
            {
                await mediator.Send(command);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateProduct")
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            try
            {
                await mediator.Send(new DeleteProductCommand(id));
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/category/{category}", async (string category, ISender mediator) =>
            Results.Ok(await mediator.Send(new GetProductsByCategoryQuery(category))))
        .WithName("GetProductsByCategory")
        .Produces<IEnumerable<GetProductByIdResponse>>();

        group.MapGet("/search", async (string searchTerm, ISender mediator) =>
            Results.Ok(await mediator.Send(new SearchProductsQuery(searchTerm))))
        .WithName("SearchProducts")
        .Produces<IEnumerable<GetProductByIdResponse>>();
    }
}
