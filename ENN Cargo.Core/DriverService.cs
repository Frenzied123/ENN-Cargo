using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver> repository;

        public DriverService(IRepository<Driver> _repository)
        {
            repository = _repository;
        }

        public async Task AddAsync(Driver driver)
        {
            await repository.AddAsync(driver);
        }

        public async Task<IEnumerable<Driver>> AllByAsync(Expression<Func<Driver, bool>> predicate)
        {
            var result = await repository.AllByAsync(predicate);
            return result.ToList();
        }

        public async Task<IEnumerable<Driver>> AllWithIncludeAsync(params Expression<Func<Driver, object>>[] include)
        {
            var result = await repository.AllWithIncludeAsync(include);
            return result.ToList();
        }

        public async Task<Driver> FindAsync(Expression<Func<Driver, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            var result = await repository.GetAllAsync();
            return result.ToList();
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            var result = await repository.GetAllAsync();
            return result.ToList();
        }


        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await repository.GetByIdAsync(d => d.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var driver = await repository.GetByIdAsync(d => d.Id == id);
            if (driver != null)
            {
                await repository.RemoveAsync(driver);
            }
        }

        public async Task UpdateAsync(Driver driver)
        {
            await repository.UpdateAsync(driver);
        }
    }
}
