using ENN_Cargo.Models;

namespace ENN_Cargo.Core
{
    public interface ICompanyStockService
    {
        Task<IEnumerable<CompanyStock>> GetAllAsync();
        Task<CompanyStock> GetByIdAsync(int id);
        Task AddAsync(CompanyStock companyStock);
        Task UpdateAsync(CompanyStock companyStock);
        Task RemoveAsync(int id);
        Task<IEnumerable<CompanyStock>> GetSortedCompanyStocksAsync(string sortOrder);
        Task<IEnumerable<CompanyStock>> GetFilteredCompanyStocksAsync(string sortOrder, string countryFilter, string townFilter);
    }
}