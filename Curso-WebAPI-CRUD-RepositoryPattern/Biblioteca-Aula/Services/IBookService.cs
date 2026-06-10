using Book.DTOs;

namespace Biblioteca_Aula.Services;

public interface IBookService
{
    Task<List<BookResponse>> GetAll();
    Task<BookResponse?> GetById(Guid id);
    Task<List<BookResponse>> GetByAuthor(Guid authorId);
    Task<BookResponse> Create(BookRequest request);
    Task<BookResponse?> Update(Guid id, BookRequest request);
    Task<bool> Delete(Guid id);
}
