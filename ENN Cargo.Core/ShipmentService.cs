using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ENN_Cargo.Core
{
    public class ShipmentService : IShipmentService
    {
        private readonly IRepository<Shipment> _shipmentRepository;
        private readonly IRepository<CompanyStocks_Shipments> _companyStocksShipmentsRepository;

        public ShipmentService(IRepository<Shipment> shipmentRepository,
                               IRepository<CompanyStocks_Shipments> companyStocksShipmentsRepository)
        {
            _shipmentRepository = shipmentRepository;
            _companyStocksShipmentsRepository = companyStocksShipmentsRepository;
        }

        public async Task<IEnumerable<Shipment>> GetAllAsync()
        {
            return await _shipmentRepository.GetAllAsync();
        }

        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await _shipmentRepository.GetByIdAsync(x => x.Id == id);
        }

        public async Task AddAsync(Shipment shipment, int companyStockId)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            await _shipmentRepository.AddAsync(shipment);

            var shipmentCompanyStock = new CompanyStocks_Shipments
            {
                Shipment_Id = shipment.Id,
                CompanyStock_Id = companyStockId
            };

            await _companyStocksShipmentsRepository.AddAsync(shipmentCompanyStock);
        }

        public async Task UpdateAsync(Shipment shipment)
        {
            await _shipmentRepository.UpdateAsync(shipment);
        }

        public async Task RemoveAsync(int id)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(x => x.Id == id);
            if (shipment != null)
            {
                await _shipmentRepository.RemoveAsync(shipment);
            }
        }

        public async Task<IEnumerable<Shipment>> GetFilteredShipmentsAsync(double? minWeight, double? maxWeight, string fromCountry, string fromTown, string toCountry, string toTown, DateTime? pickUpDateFrom, DateTime? pickUpDateTo, DateTime? deliveryDateFrom, DateTime? deliveryDateTo)
        {
            var query = (await _shipmentRepository.GetAllAsync()).AsQueryable();

            if (minWeight.HasValue)
                query = query.Where(x => x.Weight >= minWeight);
            if (maxWeight.HasValue)
                query = query.Where(x => x.Weight <= maxWeight);
            if (!string.IsNullOrEmpty(fromCountry))
                query = query.Where(x => x.FromCountry == fromCountry);
            if (!string.IsNullOrEmpty(fromTown))
                query = query.Where(x => x.FromTown == fromTown);
            if (!string.IsNullOrEmpty(toCountry))
                query = query.Where(x => x.ToCountry == toCountry);
            if (!string.IsNullOrEmpty(toTown))
                query = query.Where(x => x.ToTown == toTown);
            if (pickUpDateFrom.HasValue)
                query = query.Where(x => x.PickUpDate >= pickUpDateFrom);
            if (pickUpDateTo.HasValue)
                query = query.Where(x => x.PickUpDate <= pickUpDateTo);
            if (deliveryDateFrom.HasValue)
                query = query.Where(x => x.DeliveryDate >= deliveryDateFrom);
            if (deliveryDateTo.HasValue)
                query = query.Where(x => x.DeliveryDate <= deliveryDateTo);

            return query.ToList();
        }
    }
}