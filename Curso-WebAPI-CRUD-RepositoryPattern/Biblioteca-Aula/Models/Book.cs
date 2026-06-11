namespace Book.Models;

public class Book
{
    public Book(string title, Guid authorId, int year, string genre, decimal price)
    {
        Id = Guid.NewGuid();
        Title = title;
        AuthorId = authorId;
        Year = year;
        Genre = genre;
        Price = price;
    }

    public Guid Id { get; init; }
    public string Title { get; private set; }
    public int Year { get; private set; }
    public string Genre { get; private set; }
    public decimal Price { get; private set; }
    public Guid AuthorId { get; private set; }

    public void Update(string title, int year, string genre, decimal price, Guid authorId)
    {
        Title = title;
        Year = year;
        Genre = genre;
        Price = price;
        AuthorId = authorId;
    }
}
