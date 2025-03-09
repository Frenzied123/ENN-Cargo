using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;namespace ENN_Cargo.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> AllByAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> AllWithIncludeAsync(params Expression<Func<T, object>>[] include);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}