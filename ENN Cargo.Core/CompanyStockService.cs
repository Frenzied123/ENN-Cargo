using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class CompanyStockService : ICompanyStockService
    {
        private readonly IRepository<CompanyStock> repository;

        public CompanyStockService(IRepository<CompanyStock> _repository)
        {
            repository = _repository;
        }

        public async Task AddAsync(CompanyStock entity)
        {
            await repository.AddAsync(entity);
        }

        public async Task<IEnumerable<CompanyStock>> AllByAsync(Expression<Func<CompanyStock, bool>> predicate)
        {
            return await repository.AllByAsync(predicate);
        }

        public async Task<IEnumerable<CompanyStock>> AllWithIncludeAsync(params Expression<Func<CompanyStock, object>>[] include)
        {
            return await repository.AllWithIncludeAsync(include);
        }

        public async Task<CompanyStock> FindAsync(Expression<Func<CompanyStock, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }

        public async Task<IEnumerable<CompanyStock>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<CompanyStock> GetByIdAsync(Expression<Func<CompanyStock, bool>> filter)
        {
            return await repository.GetByIdAsync(filter);
        }

        public async Task<CompanyStock> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(CompanyStock entity)
        {
            await repository.RemoveAsync(entity);
        }

        public async Task RemoveAsync(int id)
        {
            var companyStock = await repository.GetByIdAsync(x => x.Id == id);
            if (companyStock != null)
            {
                await repository.RemoveAsync(companyStock);
            }
        }

        public async Task UpdateAsync(CompanyStock entity)
        {
            await repository.UpdateAsync(entity);
        }
    }
}