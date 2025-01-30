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
        private readonly IRepository<Shipment> repository;
        public ShipmentService(IRepository<Shipment> _repository)
        {
            repository = _repository;
        }
        public async Task<IEnumerable<Shipment>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }
        public async Task AddAsync(Shipment entity)
        {
            await repository.AddAsync(entity);
        }
        public async Task UpdateAsync(Shipment entity)
        {
            await repository.UpdateAsync(entity);
        }
        public async Task RemoveAsync(int id)
        {
            var shipment = await repository.GetByIdAsync(x => x.Id == id);
            if (shipment != null)
            {
                await repository.RemoveAsync(shipment);
            }
        }
        public async Task<IEnumerable<Shipment>> GetFilteredShipmentsAsync(double? minWeight, double? maxWeight,string fromCountry, string fromTown,string toCountry, string toTown,DateTime? pickUpDateFrom, DateTime? pickUpDateTo,DateTime? deliveryDateFrom, DateTime? deliveryDateTo)
        {
            var query = repository.GetAllAsync().Result.AsQueryable();
            if (minWeight.HasValue)
            {
                query = query.Where(x => x.Weight >= minWeight);
            }
            if (maxWeight.HasValue)
            {
                query = query.Where(x => x.Weight <= maxWeight);
            }
            if (!string.IsNullOrEmpty(fromCountry))
            {
                query = query.Where(s => s.FromCountry == fromCountry);
            }
            if (!string.IsNullOrEmpty(fromTown))
            {
                query = query.Where(s => s.FromTown == fromTown);
            }
            if (!string.IsNullOrEmpty(toCountry))
            {
                query = query.Where(s => s.ToCountry == toCountry);
            }
            if (!string.IsNullOrEmpty(toTown))
            {
                query = query.Where(s => s.ToTown == toTown);
            }
            if (pickUpDateFrom.HasValue)
            {
                query = query.Where(x => x.PickUpDate >= pickUpDateFrom);
            }
            if (pickUpDateTo.HasValue)
            {
                query = query.Where(x => x.PickUpDate <= pickUpDateTo);
            }
            if (deliveryDateFrom.HasValue)
            {
                query = query.Where(x => x.DeliveryDate >= deliveryDateFrom);
            }
            if (deliveryDateTo.HasValue)
            {
                query = query.Where(x => x.DeliveryDate <= deliveryDateTo);
            }
            return query.ToList();
        }
    }
}