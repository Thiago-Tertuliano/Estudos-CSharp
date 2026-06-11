using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Aula.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Author.Models.Author> Authors { get; set; }
    public DbSet<Book.Models.Book> Books { get; set; }
}
