using ENN_Cargo.DataAccess;
using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;namespace ENN_Cargo.Core
{
    public class TruckCompanyService : ITruckCompanyService
    {
        private readonly ENN_CargoApplicationDbContext _context;        public TruckCompanyService(ENN_CargoApplicationDbContext context)
        {
            _context = context;
        }        public async Task<IEnumerable<TruckCompany>> GetAllAsync()
        {
            return await _context.TruckCompanies
                .Include(tc => tc.Drivers)
                .Include(tc => tc.Vehicles)
                .Include(tc => tc.TruckCompanies_Shipments)
                .Include(tc => tc.User)
                .ToListAsync();
        }        public async Task<TruckCompany> GetByIdAsync(int id)
        {
            return await _context.TruckCompanies
                .Include(tc => tc.Drivers)
                .Include(tc => tc.Vehicles)
                .Include(tc => tc.TruckCompanies_Shipments)
                .Include(tc => tc.User) 
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }        public async Task AddAsync(TruckCompany entity)
        {
            _context.TruckCompanies.Add(entity);
            await _context.SaveChangesAsync();
        }        public async Task UpdateAsync(TruckCompany entity)
        {
            _context.TruckCompanies.Update(entity);
            await _context.SaveChangesAsync();
        }        public async Task RemoveAsync(int id)
        {
            var truckCompany = await _context.TruckCompanies.FindAsync(id);
            if (truckCompany != null)
            {
                _context.TruckCompanies.Remove(truckCompany);
                await _context.SaveChangesAsync();
            }
        }        public async Task<IEnumerable<TruckCompany>> GetFilteredTruckCompaniesAsync(int? minDrivers, int? maxDrivers, int? minVehicles, int? maxVehicles, string country, string town)
        {
            var query = _context.TruckCompanies
                .Include(tc => tc.Drivers)
                .Include(tc => tc.Vehicles)
                .Include(tc => tc.User)
                .AsQueryable();            if (minDrivers.HasValue)
                query = query.Where(x => x.Drivers.Count >= minDrivers.Value);
            if (maxDrivers.HasValue)
                query = query.Where(x => x.Drivers.Count <= maxDrivers.Value);
            if (minVehicles.HasValue)
                query = query.Where(x => x.Vehicles.Count >= minVehicles.Value);
            if (maxVehicles.HasValue)
                query = query.Where(x => x.Vehicles.Count <= maxVehicles.Value);
            if (!string.IsNullOrEmpty(country))
                query = query.Where(x => x.Country == country);
            if (!string.IsNullOrEmpty(town))
                query = query.Where(x => x.Town == town);            return await query.ToListAsync();
        }
    }
}