using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class TruckCompanyService : ITruckCompanyService
    {
        private readonly IRepository<TruckCompany> repository;

        public TruckCompanyService(IRepository<TruckCompany> _repository)
        {
            repository = _repository;
        }
        public async Task<IEnumerable<TruckCompany>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<TruckCompany> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task AddAsync(TruckCompany entity)
        {
            await repository.AddAsync(entity);
        }

        public async Task UpdateAsync(TruckCompany entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task RemoveAsync(int id)
        {
            var truckCompany = await repository.GetByIdAsync(x => x.Id == id);
            if (truckCompany != null)
            {
                await repository.RemoveAsync(truckCompany);
            }
        }

        public async Task<IEnumerable<TruckCompany>> GetFilteredTruckCompaniesAsync(int? minDrivers, int? maxDrivers, int? minVehicles, int? maxVehicles, string country, string town)
        {
            var truckCompanies = await repository.GetAllAsync();
            var query = truckCompanies.AsQueryable();

            if (minDrivers.HasValue)
            {
                query = query.Where(x => x.Drivers.Count >= minDrivers);
            }
            if (maxDrivers.HasValue)
            {
                query = query.Where(x => x.Drivers.Count <= maxDrivers);
            }
            if (minVehicles.HasValue)
            {
                query = query.Where(x => x.Vehicles.Count >= minVehicles);
            }
            if (maxVehicles.HasValue)
            {
                query = query.Where(x => x.Vehicles.Count <= maxVehicles);
            }
            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(x => x.Country == country);
            }
            if (!string.IsNullOrEmpty(town))
            {
                query = query.Where(x => x.Town == town);
            }

            return query.ToList();
        }
    }
}