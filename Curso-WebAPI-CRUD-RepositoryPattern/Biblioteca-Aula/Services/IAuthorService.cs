using Author.DTOs;

namespace Biblioteca_Aula.Services;

public interface IAuthorService
{
    Task<List<AuthorResponse>> GetAll();
    Task<AuthorResponse?> GetById(Guid id);
    Task<AuthorResponse> Create(AuthorRequest request);
    Task<AuthorResponse?> Update(Guid id, AuthorRequest request);
    Task<bool> Delete(Guid id);
}
