using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> repository;

        public VehicleService(IRepository<Vehicle> _repository)
        {
            repository = _repository;
        }

        public async Task AddAsync(Vehicle entity)
        {
            await repository.AddAsync(entity);
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await repository.AddAsync(vehicle);
        }

        public async Task<IEnumerable<Vehicle>> AllByAsync(Expression<Func<Vehicle, bool>> predicate)
        {
            return await repository.AllByAsync(predicate);
        }

        public async Task<IEnumerable<Vehicle>> AllWithIncludeAsync(params Expression<Func<Vehicle, object>>[] include)
        {
            return await repository.AllWithIncludeAsync(include);
        }

        public async Task<Vehicle> FindAsync(Expression<Func<Vehicle, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Vehicle> GetByIdAsync(Expression<Func<Vehicle, bool>> filter)
        {
            return await repository.GetByIdAsync(filter);
        }
        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(Vehicle entity)
        {
            await repository.RemoveAsync(entity);
        }
        public async Task RemoveAsync(int id)
        {
            var vehicle = await repository.GetByIdAsync(x => x.Id == id);
            if (vehicle != null)
            {
                await repository.RemoveAsync(vehicle);
            }
        }

        public async Task UpdateAsync(Vehicle entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            await repository.UpdateAsync(vehicle);
        }
    }
}