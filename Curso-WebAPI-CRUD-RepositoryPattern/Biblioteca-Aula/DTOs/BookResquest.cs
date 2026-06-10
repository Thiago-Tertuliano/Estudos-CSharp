namespace Book.DTOs;

public record BookRequest(string Title, Guid AuthorId, int Year, string Genre, decimal Price);
