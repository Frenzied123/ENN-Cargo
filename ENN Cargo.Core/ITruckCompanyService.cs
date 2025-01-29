using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public interface ITruckCompanyService
    {
        Task<IEnumerable<TruckCompany>> GetAllAsync();
        Task<TruckCompany> GetByIdAsync(int id);
        Task AddAsync(TruckCompany truckCompany);
        Task UpdateAsync(TruckCompany truckCompany);
        Task RemoveAsync(int id);
    }
}