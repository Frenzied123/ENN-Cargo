using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task RemoveAsync(int id);
    }
}