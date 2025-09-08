using Microsoft.EntityFrameworkCore;
using EmailPush.Domain.Interfaces;
using EmailPush.Infrastructure.Data;

namespace EmailPush.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    // Pagination support
    public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await _dbSet.Skip(skip).Take(pageSize).ToListAsync();
    }

    public virtual async Task<int> GetTotalCountAsync()
    {
        return await _dbSet.CountAsync();
    }
}