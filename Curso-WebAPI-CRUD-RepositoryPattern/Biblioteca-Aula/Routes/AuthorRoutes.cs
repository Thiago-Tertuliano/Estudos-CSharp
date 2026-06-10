using Author.DTOs;
using Biblioteca_Aula.Services;

namespace Author.Routes;

public static class AuthorRoute
{
    public static void AuthorRoutes(this WebApplication app)
    {
        var routes = app.MapGroup("author");
        routes.WithTags("Authors");

        routes.MapGet("", async (IAuthorService service) =>
        {
            var authors = await service.GetAll();
            return Results.Ok(authors);
        });

        routes.MapGet("{id:guid}", async (Guid id, IAuthorService service) =>
        {
            var author = await service.GetById(id);
            return author is null ? Results.NotFound() : Results.Ok(author);
        });

        routes.MapPost("", async (AuthorRequest request, IAuthorService service) =>
        {
            try
            {
                var author = await service.Create(request);
                return Results.Created($"/author/{author.Id}", author);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapPut("{id:guid}", async (Guid id, AuthorRequest request, IAuthorService service) =>
        {
            try
            {
                var author = await service.Update(id, request);
                return author is null ? Results.NotFound() : Results.Ok(author);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        routes.MapDelete("{id:guid}", async (Guid id, IAuthorService service) =>
        {
            try
            {
                var deleted = await service.Delete(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
        });

        routes.MapGet("{id:guid}/books", async (Guid id, IAuthorService authorService, IBookService bookService) =>
        {
            var author = await authorService.GetById(id);
            if (author is null) return Results.NotFound();

            var books = await bookService.GetByAuthor(id);
            return Results.Ok(books);
        });
    }
}
