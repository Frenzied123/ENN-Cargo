using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using System;
using System.Collections.Generic;
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

        public async Task AddAsync(Shipment entity)
        {
            await repository.AddAsync(entity);
        }

        public async Task<IEnumerable<Shipment>> AllByAsync(Expression<Func<Shipment, bool>> predicate)
        {
            return await repository.AllByAsync(predicate);
        }

        public async Task<IEnumerable<Shipment>> AllWithIncludeAsync(params Expression<Func<Shipment, object>>[] include)
        {
            return await repository.AllWithIncludeAsync(include);
        }

        public async Task<Shipment> FindAsync(Expression<Func<Shipment, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }

        public async Task<IEnumerable<Shipment>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Shipment> GetByIdAsync(Expression<Func<Shipment, bool>> filter)
        {
            return await repository.GetByIdAsync(filter);
        }
        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(Shipment entity)
        {
            await repository.RemoveAsync(entity);
        }
        public async Task RemoveAsync(int id)
        {
            var shipment = await repository.GetByIdAsync(x => x.Id == id);
            if (shipment != null)
            {
                await repository.RemoveAsync(shipment);
            }
        }

        public async Task UpdateAsync(Shipment entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task UpdateShipmentAsync(Shipment shipment)
        {
            await repository.UpdateAsync(shipment);
        }
    }
}