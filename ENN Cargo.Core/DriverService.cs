using ENN_Cargo.Core;
using ENN_Cargo.DataAccess;
using ENN_Cargo.Models;
using Microsoft.EntityFrameworkCore;

public class DriverService : IDriverService
{
    private readonly ENN_CargoApplicationDbContext _context;

    public DriverService(ENN_CargoApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Driver>> GetAllDriversAsync()
    {
        return await _context.Drivers.Include(d => d.TruckCompany).ToListAsync();
    }

    public async Task<IEnumerable<Driver>> GetFilteredDriversAsync(
        int? minExperience,
        int? maxExperience,
        string sortByExperience,
        string sortByTruckCompany,
        int? truckCompanyId)
    {
        IQueryable<Driver> query = _context.Drivers.Include(d => d.TruckCompany);

        if (minExperience.HasValue)
            query = query.Where(d => d.Experience >= minExperience.Value);

        if (maxExperience.HasValue)
            query = query.Where(d => d.Experience <= maxExperience.Value);

        if (truckCompanyId.HasValue)
            query = query.Where(d => d.TruckCompany_Id == truckCompanyId.Value);
        if (sortByExperience == "Low-High")
            query = query.OrderBy(d => d.Experience);
        else if (sortByExperience == "High-Low")
            query = query.OrderByDescending(d => d.Experience);

        if (sortByTruckCompany == "A-Z")
            query = query.OrderBy(d => d.TruckCompany.Name);
        else if (sortByTruckCompany == "Z-A")
            query = query.OrderByDescending(d => d.TruckCompany.Name);

        return await query.ToListAsync();
    }

    public async Task<Driver> GetDriverByIdAsync(int id)
    {
        return await _context.Drivers.Include(d => d.TruckCompany).FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Driver driver)
    {
        _context.Drivers.Update(driver);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver != null)
        {
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
        }
    }
}