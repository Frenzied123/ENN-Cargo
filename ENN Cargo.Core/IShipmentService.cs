using ENN_Cargo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;namespace ENN_Cargo.Core
{
    public interface IShipmentService
    {
        Task<IEnumerable<Shipment>> GetAllAsync();
        Task<Shipment> GetByIdAsync(int id);
        Task AddAsync(Shipment shipment, int companyStockId);
        Task UpdateAsync(Shipment shipment);
        Task RemoveAsync(int id);
        Task<IEnumerable<Shipment>> GetFilteredShipmentsAsync(double? minWeight, double? maxWeight, string fromCountry, string fromCity,string toCountry, string toCity, DateTime? pickUpDateFrom, DateTime? pickUpDateTo,DateTime? deliveryDateFrom, DateTime? deliveryDateTo);
        Task AssignShipmentAsync(int shipmentId, int? driverId, int? vehicleId, int truckCompanyId);
    }
}