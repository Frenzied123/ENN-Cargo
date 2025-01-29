using ENN_Cargo.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ENN_CargoApplicationDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(ENN_CargoApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> AllByAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> AllWithIncludeAsync(params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = dbSet;
            foreach (var x in include)
            {
                query = query.Include(x);
            }
            return await query.ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task RemoveAsync(T entity)
        {
            dbContext.Remove(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Update(entity);
        }
    }
}
