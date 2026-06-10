using Biblioteca_Aula.Services;
using Book.DTOs;

namespace Book.Routes;

public static class BookRoute
{
    public static void BookRoutes(this WebApplication app)
    {
        var routes = app.MapGroup("book");
        routes.WithTags("Books");

        routes.MapGet("", async (IBookService service) =>
        {
            var books = await service.GetAll();
            return Results.Ok(books);
        });

        routes.MapGet("{id:guid}", async (Guid id, IBookService service) =>
        {
            var book = await service.GetById(id);
            return book is null ? Results.NotFound() : Results.Ok(book);
        });

        routes.MapPost("", async (BookRequest request, IBookService service) =>
        {
            try
            {
                var book = await service.Create(request);
                return Results.Created($"/book/{book.Id}", book);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapPut("{id:guid}", async (Guid id, BookRequest request, IBookService service) =>
        {
            try
            {
                var book = await service.Update(id, request);
                return book is null ? Results.NotFound() : Results.Ok(book);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapDelete("{id:guid}", async (Guid id, IBookService service) =>
        {
            var deleted = await service.Delete(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
