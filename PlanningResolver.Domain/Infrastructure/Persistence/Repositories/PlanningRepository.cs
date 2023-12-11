using Microsoft.EntityFrameworkCore;
using PlaninngResolver.Domain.Interfaces;

namespace PlaninngResolver.Domain.Infrastructure.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly PlanningDbContext _context;

    public Repository(PlanningDbContext context)
    {
        _context = context;
        UnitOfWork = context;
    }
   
    public IUnitOfWork UnitOfWork { get; }
    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>();
    }

    public Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
       await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
    

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Attach(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Detach(TEntity entity)
    {
        throw new NotImplementedException();
    }
    

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
       return query.FirstOrDefaultAsync();
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.SingleAsync();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return query.ToListAsync();
    }
    
}