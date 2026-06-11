namespace Book.DTOs;

public record BookResponse(Guid Id, string Title, int Year, string Genre, decimal Price, Guid AuthorId);
