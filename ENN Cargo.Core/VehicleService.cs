using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleService(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }
        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(x => x.Id == id);
        }
        public async Task AddAsync(Vehicle vehicle)
        {
            await _vehicleRepository.AddAsync(vehicle);
        }
        public async Task UpdateAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }
        public async Task RemoveAsync(int id)
        {
            var vehicle = await GetByIdAsync(id);
            if (vehicle != null)
            {
                await _vehicleRepository.RemoveAsync(vehicle);
            }
        }
        public async Task<IEnumerable<Vehicle>> AllByAsync(Expression<Func<Vehicle, bool>> predicate)
        {
            return await _vehicleRepository.AllByAsync(predicate);
        }
    }
}