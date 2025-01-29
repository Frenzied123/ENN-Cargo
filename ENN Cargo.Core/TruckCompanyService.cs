using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class TruckCompanyService : ITruckCompanyService
    {
        private readonly IRepository<TruckCompany> repository;

        public TruckCompanyService(IRepository<TruckCompany> _repository)
        {
            repository = _repository;
        }
        public async Task AddAsync(TruckCompany entity)
        {
            await repository.AddAsync(entity);
        }

        public async Task<IEnumerable<TruckCompany>> AllByAsync(Expression<Func<TruckCompany, bool>> predicate)
        {
            return await repository.AllByAsync(predicate);
        }

        public async Task<IEnumerable<TruckCompany>> AllWithIncludeAsync(params Expression<Func<TruckCompany, object>>[] include)
        {
            return await repository.AllWithIncludeAsync(include);
        }

        public async Task<TruckCompany> FindAsync(Expression<Func<TruckCompany, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }
        public async Task<IEnumerable<TruckCompany>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<TruckCompany> GetByIdAsync(Expression<Func<TruckCompany, bool>> filter)
        {
            return await repository.GetByIdAsync(filter);
        }
        public async Task<TruckCompany> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(TruckCompany entity)
        {
            await repository.RemoveAsync(entity);
        }
        public async Task RemoveAsync(int id)
        {
            var truckCompany = await repository.GetByIdAsync(x => x.Id == id);
            if (truckCompany != null)
            {
                await repository.RemoveAsync(truckCompany);
            }
        }

        public async Task UpdateAsync(TruckCompany entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task UpdateTruckCompanyAsync(TruckCompany truckCompany)
        {
            await repository.UpdateAsync(truckCompany);
        }
    }
}