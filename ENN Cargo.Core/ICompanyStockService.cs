using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public interface ICompanyStockService
    {
        Task<IEnumerable<CompanyStock>> GetAllAsync();
        Task<CompanyStock> GetByIdAsync(int id);
        Task AddAsync(CompanyStock companyStock);
        Task UpdateAsync(CompanyStock companyStock);
        Task RemoveAsync(int id);
    }
}