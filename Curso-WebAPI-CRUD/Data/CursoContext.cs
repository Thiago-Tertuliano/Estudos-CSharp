using Curso.Models;
using Microsoft.EntityFrameworkCore;

namespace Curso.Data;

public class CursoContext(DbContextOptions<CursoContext> options) : DbContext(options)
{
    public DbSet<CursoModel> Pessoas { get; set; }
}