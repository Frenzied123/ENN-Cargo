using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class CompanyStockService : ICompanyStockService
    {
        private readonly IRepository<CompanyStock> _companyStockRepository;

        public CompanyStockService(IRepository<CompanyStock> companyStockRepository)
        {
            _companyStockRepository = companyStockRepository;
        }

        public async Task<IEnumerable<CompanyStock>> GetAllAsync()
        {
            return await _companyStockRepository.AllWithIncludeAsync(cs => cs.User);
        }

        public async Task<CompanyStock> GetByIdAsync(int id)
        {
            return await _companyStockRepository.GetByIdAsync(x => x.Id == id);
        }

        public async Task AddAsync(CompanyStock companyStock)
        {
            if (companyStock == null)
                throw new ArgumentNullException(nameof(companyStock));

            await _companyStockRepository.AddAsync(companyStock);
        }

        public async Task UpdateAsync(CompanyStock companyStock)
        {
            if (companyStock == null)
                throw new ArgumentNullException(nameof(companyStock));

            await _companyStockRepository.UpdateAsync(companyStock);
        }

        public async Task RemoveAsync(int id)
        {
            var companyStock = await _companyStockRepository.GetByIdAsync(x => x.Id == id);
            if (companyStock != null)
            {
                await _companyStockRepository.RemoveAsync(companyStock);
            }
        }

        public async Task<IEnumerable<string>> GetAllCountriesAsync()
        {
            var predefinedCountries = new List<string> { "USA", "Canada", "Germany", "France", "UK" };
            var companyStocks = await _companyStockRepository.GetAllAsync();

            var existingCountries = companyStocks
                .Where(x => !string.IsNullOrEmpty(x.Country))
                .Select(x => x.Country)
                .Distinct()
                .ToList();

            return predefinedCountries.Union(existingCountries).OrderBy(c => c);
        }

        public async Task<IEnumerable<string>> GetTownsByCountryAsync(string country)
        {
            var predefinedTowns = new Dictionary<string, List<string>>
        {
            { "Canada", new List<string> { "Toronto", "Vancouver" } },
            { "USA", new List<string> { "New York", "Los Angeles" } },
            { "Germany", new List<string> { "Berlin", "Munich" } },
            { "France", new List<string> { "Paris", "Lyon" } },
            { "UK", new List<string> { "London", "Manchester" } }
        };

            var companyStocks = await _companyStockRepository.GetAllAsync();
            var townsInCountry = companyStocks
                .Where(x => x.Country == country && !string.IsNullOrEmpty(x.Town))
                .Select(x => x.Town)
                .Distinct()
                .ToList();

            return predefinedTowns.ContainsKey(country)
                ? townsInCountry.Union(predefinedTowns[country]).Distinct().OrderBy(t => t)
                : townsInCountry.OrderBy(t => t);
        }

        public async Task<IEnumerable<string>> GetAllTownsAsync()
        {
            var predefinedTowns = new List<string> { "New York", "Toronto", "Berlin", "Paris", "London" };
            var companyStocks = await _companyStockRepository.GetAllAsync();

            var existingTowns = companyStocks
                .Where(x => !string.IsNullOrEmpty(x.Town))
                .Select(x => x.Town)
                .Distinct()
                .ToList();

            return predefinedTowns.Union(existingTowns).OrderBy(t => t);
        }
    }
}