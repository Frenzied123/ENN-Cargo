using ENN_Cargo.Core;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ENN_Cargo.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListOfShipments()
        {
            var shipments = await _shipmentService.GetAllAsync();

            var fromCountries = shipments.Select(x => x.FromCountry).Distinct().ToList();
            var fromCities = shipments.Select(x => x.FromTown).Distinct().ToList();
            var toCountries = shipments.Select(x => x.ToCountry).Distinct().ToList();
            var toCities = shipments.Select(x => x.ToTown).Distinct().ToList();

            var viewModel = new ShipmentFilterViewModel
            {
                Shipments = shipments.ToList(),
                FromCountries = new SelectList(fromCountries),
                FromTowns = new SelectList(fromCities),
                ToCountries = new SelectList(toCountries),
                ToTowns = new SelectList(toCities)
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ListOfShipments(ShipmentFilterViewModel model)
        {
            var filteredShipments = await _shipmentService.GetFilteredShipmentsAsync(
                model.MinWeight, model.MaxWeight,
                model.SelectedFromCountry, model.SelectedFromTown,
                model.SelectedToCountry, model.SelectedToTown,
                model.PickUpDateFrom, model.PickUpDateTo,
                model.DeliveryDateFrom, model.DeliveryDateTo);

            var shipments = await _shipmentService.GetAllAsync();
            var fromCountries = shipments.Select(x => x.FromCountry).Distinct().ToList();
            var fromCities = shipments.Select(x => x.FromTown).Distinct().ToList();
            var toCountries = shipments.Select(x => x.ToCountry).Distinct().ToList();
            var toCities = shipments.Select(x => x.ToTown).Distinct().ToList();

            model.Shipments = filteredShipments.ToList();
            model.FromCountries = new SelectList(fromCountries);
            model.FromTowns = new SelectList(fromCities);
            model.ToCountries = new SelectList(toCountries);
            model.ToTowns = new SelectList(toCities);

            return View(model);
        }
    }
}