using Author.DTOs;
using Biblioteca_Aula.Repositories;

namespace Biblioteca_Aula.Services;

public class AuthorService : IAuthorService
{
    private readonly IRepository<Author.Models.Author> _authorRepo;
    private readonly IRepository<Book.Models.Book> _bookRepo;

    public AuthorService(
        IRepository<Author.Models.Author> authorRepo,
        IRepository<Book.Models.Book> bookRepo)
    {
        _authorRepo = authorRepo;
        _bookRepo = bookRepo;
    }

    public async Task<List<AuthorResponse>> GetAll()
    {
        var authors = await _authorRepo.GetAll();
        return authors
            .Select(a => new AuthorResponse(a.Id, a.Name, a.Age, a.Gender))
            .ToList();
    }

    public async Task<AuthorResponse?> GetById(Guid id)
    {
        var author = await _authorRepo.GetById(id);
        if (author is null) return null;

        return new AuthorResponse(author.Id, author.Name, author.Age, author.Gender);
    }

    public async Task<AuthorResponse> Create(AuthorRequest request)
    {
        Validate(request);

        var author = new Author.Models.Author(request.Name, request.Age, request.Gender);
        await _authorRepo.Add(author);

        return new AuthorResponse(author.Id, author.Name, author.Age, author.Gender);
    }

    public async Task<AuthorResponse?> Update(Guid id, AuthorRequest request)
    {
        Validate(request);

        var author = await _authorRepo.GetById(id);
        if (author is null) return null;

        author.Update(request.Name, request.Age, request.Gender);
        await _authorRepo.Update(author);

        return new AuthorResponse(author.Id, author.Name, author.Age, author.Gender);
    }

    public async Task<bool> Delete(Guid id)
    {
        var author = await _authorRepo.GetById(id);
        if (author is null) return false;

        var books = await _bookRepo.Find(b => b.AuthorId == id);
        if (books.Count > 0)
            throw new InvalidOperationException("Cannot delete author with registered books.");

        return await _authorRepo.Delete(id);
    }

    private static void Validate(AuthorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required.");

        if (request.Age < 0)
            throw new ArgumentException("Age cannot be negative.");

        if (string.IsNullOrWhiteSpace(request.Gender))
            throw new ArgumentException("Gender is required.");
    }
}
