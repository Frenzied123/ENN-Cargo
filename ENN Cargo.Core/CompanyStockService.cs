using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;

namespace ENN_Cargo.Core
{
    public class CompanyStockService : ICompanyStockService
    {
        private readonly IRepository<CompanyStock> _repository;

        public CompanyStockService(IRepository<CompanyStock> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CompanyStock>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<CompanyStock>> GetSortedCompanyStocksAsync(string sortOrder)
        {
            var companyStocks = await _repository.GetAllAsync();
            if (sortOrder == "A-Z")
            {
                return companyStocks.OrderBy(x => x.Name).ToList();
            }
            else if (sortOrder == "Z-A")
            {
                return companyStocks.OrderByDescending(x => x.Name).ToList();
            }
            return companyStocks.ToList();
        }

        public async Task<CompanyStock> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task AddAsync(CompanyStock companyStock)
        {
            await _repository.AddAsync(companyStock);
        }

        public async Task UpdateAsync(CompanyStock companyStock)
        {
            await _repository.UpdateAsync(companyStock);
        }

        public async Task RemoveAsync(int id)
        {
            var companyStock = await _repository.GetByIdAsync(x => x.Id == id);
            if (companyStock != null)
            {
                await _repository.RemoveAsync(companyStock);
            }
        }

        public async Task<IEnumerable<CompanyStock>> GetFilteredCompanyStocksAsync(string sortOrder, string countryFilter, string townFilter)
        {
            var companyStocks = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(countryFilter))
            {
                companyStocks = companyStocks.Where(x => x.Country.Contains(countryFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(townFilter))
            {
                companyStocks = companyStocks.Where(x => x.Town.Contains(townFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (sortOrder == "A-Z")
            {
                companyStocks = companyStocks.OrderBy(x => x.Name).ToList();
            }
            else if (sortOrder == "Z-A")
            {
                companyStocks = companyStocks.OrderByDescending(x => x.Name).ToList();
            }

            return companyStocks;
        }
    }
}