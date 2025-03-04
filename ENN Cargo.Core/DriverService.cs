using ENN_Cargo.Core;
using ENN_Cargo.DataAccess;
using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using Microsoft.EntityFrameworkCore;

public class DriverService : IDriverService
{
    private readonly IRepository<Driver> _driverRepository;

    public DriverService(IRepository<Driver> driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<IEnumerable<Driver>> GetAllAsync()
    {
        return await _driverRepository.AllWithIncludeAsync(d => d.User, d => d.TruckCompany);
    }

    public async Task<Driver> GetByIdAsync(int id)
    {
        return await _driverRepository.GetByIdAsync(x => x.Id == id);
    }

    public async Task AddAsync(Driver driver)
    {
        if (driver == null)
            throw new ArgumentNullException(nameof(driver));

        await _driverRepository.AddAsync(driver);
    }

    public async Task UpdateAsync(Driver driver)
    {
        if (driver == null)
            throw new ArgumentNullException(nameof(driver));

        await _driverRepository.UpdateAsync(driver);
    }

    public async Task RemoveAsync(int id)
    {
        var driver = await _driverRepository.GetByIdAsync(x => x.Id == id);
        if (driver != null)
        {
            await _driverRepository.RemoveAsync(driver);
        }
    }

    public async Task<IEnumerable<Driver>> GetFilteredDriversAsync(
        int? minExperience,
        int? maxExperience,
        string sortByExperience,
        string sortByTruckCompany,
        int? truckCompanyId)
    {
        var drivers = await _driverRepository.AllWithIncludeAsync(d => d.User, d => d.TruckCompany);
        var query = drivers.AsQueryable();

        if (minExperience.HasValue)
            query = query.Where(d => d.Experience >= minExperience.Value);
        if (maxExperience.HasValue)
            query = query.Where(d => d.Experience <= maxExperience.Value);
        if (truckCompanyId.HasValue)
            query = query.Where(d => d.TruckCompany_Id == truckCompanyId.Value);

        if (!string.IsNullOrEmpty(sortByExperience))
        {
            if (sortByExperience == "Low-High")
                query = query.OrderBy(d => d.Experience);
            else if (sortByExperience == "High-Low")
                query = query.OrderByDescending(d => d.Experience);
        }

        if (!string.IsNullOrEmpty(sortByTruckCompany))
        {
            if (sortByTruckCompany == "A-Z")
                query = query.OrderBy(d => d.TruckCompany != null ? d.TruckCompany.Name : string.Empty);
            else if (sortByTruckCompany == "Z-A")
                query = query.OrderByDescending(d => d.TruckCompany != null ? d.TruckCompany.Name : string.Empty);
        }

        return query.ToList();
    }
}