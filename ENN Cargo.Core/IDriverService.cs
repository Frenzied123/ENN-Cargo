using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;namespace ENN_Cargo.Core
{
    public interface IDriverService
    {
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<IEnumerable<Driver>> GetFilteredDriversAsync(int? minExperience, int? maxExperience, string sortByExperience, string sortByTruckCompany, int? truckCompanyId);
        Task<Driver> GetByIdAsync(int id);
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task RemoveAsync(int id);
    }
}