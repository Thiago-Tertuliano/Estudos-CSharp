using System.Linq.Expressions;

namespace E_Commerce.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAll();
    Task<T?> GetById(Guid id);
    Task<List<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(Guid id);
}
