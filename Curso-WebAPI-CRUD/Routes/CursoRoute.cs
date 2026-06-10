namespace Curso.Routes;
using Curso.Models;
using Curso.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public static class CursoRoute
{
    public static void CursoRoutes(this WebApplication app)
    {
        var route = app.MapGroup("curso");

        route.MapPost("", async ([FromBody] CursoRequest req, [FromServices] CursoContext context) =>
        {
            var curso = new CursoModel(req.Nome);
            await context.AddAsync(curso);
            await context.SaveChangesAsync();
            return Results.Created($"/curso/{curso.Id}", curso);
        });

        route.MapGet("", async ([FromServices] CursoContext context) =>
        {
            var pessoas = await context.Pessoas.ToListAsync();
            return Results.Ok(pessoas);
        });

        route.MapPut("{id:guid}", async (Guid id, [FromBody] CursoRequest req, [FromServices] CursoContext context) =>
        {
            var curso = await context.Pessoas.FirstOrDefaultAsync(x => x.Id == id);

            if (curso == null)
                return Results.NotFound();

            curso.ChangeName(req.Nome);
            await context.SaveChangesAsync();

            return Results.Ok(curso);
        });

        route.MapDelete("{id:guid}", async (Guid id, [FromServices] CursoContext context) =>
        {
            var curso = await context.Pessoas.FirstOrDefaultAsync(x => x.Id == id);

            if (curso == null)
                return Results.NotFound();

            context.Remove(curso);
            await context.SaveChangesAsync();

            return Results.Ok();
        });
    }
}