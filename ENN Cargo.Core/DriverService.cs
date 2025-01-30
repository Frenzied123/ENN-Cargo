using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver> _repository;
        private readonly IRepository<TruckCompany> _truckCompanyRepository;
        public DriverService(IRepository<Driver> repository, IRepository<TruckCompany> truckCompanyRepository)
        {
            _repository = repository;
            _truckCompanyRepository = truckCompanyRepository;
        }
        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _repository.AllWithIncludeAsync(x => x.TruckCompany);
        }
        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Driver>> GetFilteredDriversAsync(int? minExperience, int? maxExperience, string sortByExperience, string sortByTruckCompany)
        {
            var drivers = await _repository.AllWithIncludeAsync(x => x.TruckCompany);
            var query = drivers.AsQueryable();
            if (minExperience.HasValue)
            {
                query = query.Where(x => x.Experience >= minExperience.Value);
            }
            if (maxExperience.HasValue)
            {
                query = query.Where(x => x.Experience <= maxExperience.Value);
            }
            if (sortByExperience == "Low-High")
            {
                query = query.OrderBy(x => x.Experience);
            }
            else if (sortByExperience == "High-Low")
            {
                query = query.OrderByDescending(x => x.Experience);
            }
            if (sortByTruckCompany == "A-Z")
            {
                query = query.OrderBy(x => x.TruckCompany.Name);
            }
            else if (sortByTruckCompany == "Z-A")
            {
                query = query.OrderByDescending(x => x.TruckCompany.Name);
            }
            return query.ToList();
        }
        public async Task AddAsync(Driver driver)
        {
            await _repository.AddAsync(driver);
        }
        public async Task UpdateAsync(Driver driver)
        {
            await _repository.UpdateAsync(driver);
        }
        public async Task RemoveAsync(int id)
        {
            var driver = await _repository.GetByIdAsync(x => x.Id == id);
            if (driver != null)
            {
                await _repository.RemoveAsync(driver);
            }
        }
    }
}