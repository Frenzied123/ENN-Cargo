using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        Task<IEnumerable<string>> GetAllCountriesAsync();
        Task<IEnumerable<string>> GetAllTownsAsync();
        Task<IEnumerable<string>> GetTownsByCountryAsync(string country);
    }
}