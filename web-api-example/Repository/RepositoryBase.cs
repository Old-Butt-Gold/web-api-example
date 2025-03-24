using System.Linq.Expressions;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class RepositoryBase<T, TKey> : IRepositoryBase<T> 
    where T : BaseModel<TKey>
{
    protected readonly RepositoryContext RepositoryContext;
    protected readonly DbSet<T> DbSet;
    
    public RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
        DbSet = RepositoryContext.Set<T>();
    }
    
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges
            ? DbSet
                .AsNoTracking()
            : DbSet;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges)
    {
        return !trackChanges
            ? DbSet
                .Where(expression)
                .AsNoTracking()
            : DbSet
                .Where(expression);
    }
    

    public void Create(T entity) => DbSet.Add(entity);
    public void Update(T entity) => DbSet.Update(entity);
    public void Delete(T entity) => DbSet.Remove(entity);
}