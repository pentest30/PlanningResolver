namespace PlaninngResolver.Domain.Interfaces;

public interface IRepository<TEntity>  where TEntity :class
{
    IUnitOfWork UnitOfWork { get; }

    IQueryable<TEntity> GetAll();

    Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);
    void Attach(TEntity entity);
    void Detach(TEntity entity);

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query);

    Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query);

    Task<List<T>> ToListAsync<T>(IQueryable<T> query);

   

}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}