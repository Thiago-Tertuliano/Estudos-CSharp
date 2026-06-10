using Biblioteca_Aula.Repositories;
using Book.DTOs;

namespace Biblioteca_Aula.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book.Models.Book> _bookRepo;
    private readonly IRepository<Author.Models.Author> _authorRepo;

    public BookService(
        IRepository<Book.Models.Book> bookRepo,
        IRepository<Author.Models.Author> authorRepo)
    {
        _bookRepo = bookRepo;
        _authorRepo = authorRepo;
    }

    public async Task<List<BookResponse>> GetAll()
    {
        var books = await _bookRepo.GetAll();
        return books
            .Select(b => new BookResponse(b.Id, b.Title, b.Year, b.Genre, b.Price, b.AuthorId))
            .ToList();
    }

    public async Task<List<BookResponse>> GetByAuthor(Guid authorId)
    {
        var books = await _bookRepo.Find(b => b.AuthorId == authorId);
        return books
            .Select(b => new BookResponse(b.Id, b.Title, b.Year, b.Genre, b.Price, b.AuthorId))
            .ToList();
    }

    public async Task<BookResponse?> GetById(Guid id)
    {
        var book = await _bookRepo.GetById(id);
        if (book is null) return null;

        return new BookResponse(book.Id, book.Title, book.Year, book.Genre, book.Price, book.AuthorId);
    }

    public async Task<BookResponse> Create(BookRequest request)
    {
        Validate(request);

        var author = await _authorRepo.GetById(request.AuthorId);
        if (author is null)
            throw new ArgumentException("Author not found.");

        var book = new Book.Models.Book(request.Title, request.AuthorId, request.Year, request.Genre, request.Price);
        await _bookRepo.Add(book);

        return new BookResponse(book.Id, book.Title, book.Year, book.Genre, book.Price, book.AuthorId);
    }

    public async Task<BookResponse?> Update(Guid id, BookRequest request)
    {
        Validate(request);

        var book = await _bookRepo.GetById(id);
        if (book is null) return null;

        var author = await _authorRepo.GetById(request.AuthorId);
        if (author is null)
            throw new ArgumentException("Author not found.");

        book.Update(request.Title, request.Year, request.Genre, request.Price, request.AuthorId);
        await _bookRepo.Update(book);

        return new BookResponse(book.Id, book.Title, book.Year, book.Genre, book.Price, book.AuthorId);
    }

    public async Task<bool> Delete(Guid id)
    {
        var book = await _bookRepo.GetById(id);
        if (book is null) return false;

        return await _bookRepo.Delete(id);
    }

    private static void Validate(BookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required.");

        if (request.Year < 1400 || request.Year > DateTime.Now.Year + 1)
            throw new ArgumentException($"Year must be between 1400 and {DateTime.Now.Year + 1}.");

        if (request.Price < 0)
            throw new ArgumentException("Price cannot be negative.");
    }
}
