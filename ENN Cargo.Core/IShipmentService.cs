using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public interface IShipmentService
    {
        Task<IEnumerable<Shipment>> GetAllAsync();
        Task<Shipment> GetByIdAsync(int id);
        Task AddAsync(Shipment shipment);
        Task UpdateAsync(Shipment shipment);
        Task RemoveAsync(int id);
    }
}