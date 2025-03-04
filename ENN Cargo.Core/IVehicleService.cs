using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENN_Cargo.Models;

namespace ENN_Cargo.Core
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task RemoveAsync(int id);
        Task<IEnumerable<Vehicle>> AllByAsync(Expression<Func<Vehicle, bool>> predicate);
    }
}