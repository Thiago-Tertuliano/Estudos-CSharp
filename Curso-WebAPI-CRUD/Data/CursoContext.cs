namespace Curso.Data;
using Curso.Models;
using Microsoft.EntityFrameworkCore;

public class CursoContext : DbContext
{
    public DbSet<CursoModel> Pessoas { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=curso.sqllite");
        base.OnConfiguring(optionsBuilder);
    }
}