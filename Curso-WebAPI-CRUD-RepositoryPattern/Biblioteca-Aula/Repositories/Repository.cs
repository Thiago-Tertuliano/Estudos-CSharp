using System.Linq.Expressions;
using Biblioteca_Aula.Data;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Aula.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LibraryContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(LibraryContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<T> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
