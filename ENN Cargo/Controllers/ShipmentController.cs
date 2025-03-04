using ENN_Cargo.Core;
using ENN_Cargo.DataAccess.Repository.IRepository;
using ENN_Cargo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;
    private readonly ICompanyStockService _companyStockService;
    private readonly IRepository<CompanyStock> _companyStockRepository;
    private readonly IRepository<CompanyStocks_Shipments> _companyStocksShipmentsRepository;

    private static readonly Dictionary<string, List<string>> PredefinedTowns = new()
    {
        { "Bulgaria", new() { "Sofia", "Plovdiv", "Varna" } },
        { "Turkey", new() { "Istanbul", "Ankara", "Izmir" } },
        { "Romania", new() { "Bucharest", "Cluj-Napoca", "Timisoara" } }
    };
    private static readonly List<string> PredefinedCountries = new() { "Bulgaria", "Turkey", "Romania" };
    public ShipmentController(
        IShipmentService shipmentService,
        ICompanyStockService companyStockService,
        IRepository<CompanyStock> companyStockRepository,
        IRepository<CompanyStocks_Shipments> companyStocksShipmentsRepository)
    {
        _shipmentService = shipmentService;
        _companyStockService = companyStockService;
        _companyStockRepository = companyStockRepository;
        _companyStocksShipmentsRepository = companyStocksShipmentsRepository;
    }
    [HttpGet]
    public async Task<IActionResult> ListOfShipments(ShipmentViewModel model)
    {
        var shipments = (await _shipmentService.GetAllAsync()).ToList();
        shipments = ApplyFilters(model, shipments);
        model.Shipments = shipments;
        model.FromCountries = new SelectList(PredefinedCountries, model.SelectedFromCountry);
        model.ToCountries = new SelectList(PredefinedCountries, model.SelectedToCountry);
        model.FromTowns = new SelectList(
            !string.IsNullOrEmpty(model.SelectedFromCountry) && PredefinedTowns.ContainsKey(model.SelectedFromCountry)
                ? PredefinedTowns[model.SelectedFromCountry]
                : Enumerable.Empty<string>(),
            model.SelectedFromTown
        );
        model.ToTowns = new SelectList(
            !string.IsNullOrEmpty(model.SelectedToCountry) && PredefinedTowns.ContainsKey(model.SelectedToCountry)
                ? PredefinedTowns[model.SelectedToCountry]
                : Enumerable.Empty<string>(),
            model.SelectedToTown
        );
        model.PredefinedTowns = PredefinedTowns;

        return View(model);
    }
    private static List<Shipment> ApplyFilters(ShipmentViewModel model, List<Shipment> shipments)
    {
        if (model.MinWeight.HasValue) shipments = shipments.Where(s => s.Weight >= model.MinWeight).ToList();
        if (model.MaxWeight.HasValue) shipments = shipments.Where(s => s.Weight <= model.MaxWeight).ToList();
        if (!string.IsNullOrEmpty(model.SelectedFromCountry)) shipments = shipments.Where(s => s.FromCountry == model.SelectedFromCountry).ToList();
        if (!string.IsNullOrEmpty(model.SelectedFromTown)) shipments = shipments.Where(s => s.FromTown == model.SelectedFromTown).ToList();
        if (!string.IsNullOrEmpty(model.SelectedToCountry)) shipments = shipments.Where(s => s.ToCountry == model.SelectedToCountry).ToList();
        if (!string.IsNullOrEmpty(model.SelectedToTown)) shipments = shipments.Where(s => s.ToTown == model.SelectedToTown).ToList();
        if (model.PickUpDate.HasValue) shipments = shipments.Where(s => s.PickUpDate >= model.PickUpDate).ToList();
        if (model.DeliveryDate.HasValue) shipments = shipments.Where(s => s.DeliveryDate >= model.DeliveryDate).ToList();
        return shipments;
    }
    public async Task<IActionResult> AddShipment()
    {
        var model = await PopulateDropdowns(new ShipmentViewModel());
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddShipment(ShipmentViewModel model)
    {
        if (!string.IsNullOrEmpty(model.Description) &&
            model.Weight != 0 &&
            !string.IsNullOrEmpty(model.SelectedFromCountry) &&
            !string.IsNullOrEmpty(model.SelectedFromTown) &&
            !string.IsNullOrEmpty(model.FromAddress) &&
            !string.IsNullOrEmpty(model.SelectedToCountry) &&
            !string.IsNullOrEmpty(model.SelectedToTown) &&
            !string.IsNullOrEmpty(model.ToAddress) &&
            model.PickUpDate.HasValue &&
            model.DeliveryDate.HasValue)
        {
            var shipment = new Shipment
            {
                Description = model.Description,
                Weight = model.Weight,
                FromCountry = model.SelectedFromCountry,
                FromTown = model.SelectedFromTown,
                FromAddress = model.FromAddress,
                ToCountry = model.SelectedToCountry,
                ToTown = model.SelectedToTown,
                ToAddress = model.ToAddress,
                PickUpDate = model.PickUpDate.Value,
                DeliveryDate = model.DeliveryDate.Value,
                Status = "Pending" 
            };
            await _shipmentService.AddAsync(shipment, model.SelectedCompanyStockId.GetValueOrDefault());
            return RedirectToAction("ListOfShipments");
        }
        model = await PopulateDropdowns(model);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AssignShipmentToCompany(int shipmentId, int companyStockId)
    {
        if (!(await _companyStocksShipmentsRepository.GetAllAsync()).Any(cs => cs.Shipment_Id == shipmentId))
        {
            await _companyStocksShipmentsRepository.AddAsync(new CompanyStocks_Shipments { Shipment_Id = shipmentId, CompanyStock_Id = companyStockId });
        }
        return RedirectToAction(nameof(ListOfShipments));
    }
    [HttpGet]
    public async Task<IActionResult> UpdateShipment(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);
        if (shipment == null) return NotFound();
        var companyStocks = await _companyStockRepository.GetAllAsync();
        var existingAssignment = (await _companyStocksShipmentsRepository.GetAllAsync()).FirstOrDefault(cs => cs.Shipment_Id == shipment.Id);
        var model = new ShipmentViewModel
        {
            Shipment = shipment,
            Description = shipment.Description,
            Weight = shipment.Weight,
            FromAddress = shipment.FromAddress,
            ToAddress = shipment.ToAddress,
            PickUpDate = shipment.PickUpDate,
            DeliveryDate = shipment.DeliveryDate,
            Status = shipment.Status,
            SelectedFromCountry = shipment.FromCountry,
            SelectedFromTown = shipment.FromTown,
            SelectedToCountry = shipment.ToCountry,
            SelectedToTown = shipment.ToTown,
            SelectedCompanyStockId = existingAssignment?.CompanyStock_Id,
            FromCountries = new SelectList(PredefinedCountries, shipment.FromCountry),
            ToCountries = new SelectList(PredefinedCountries, shipment.ToCountry),
            FromTowns = new SelectList(PredefinedTowns.GetValueOrDefault(shipment.FromCountry, new List<string>()), shipment.FromTown),
            ToTowns = new SelectList(PredefinedTowns.GetValueOrDefault(shipment.ToCountry, new List<string>()), shipment.ToTown),
            CompanyStocks = new SelectList(companyStocks, "Id", "Name", existingAssignment?.CompanyStock_Id),
            PredefinedTowns = PredefinedTowns
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateShipment(ShipmentViewModel model)
    {
        if (model.Shipment?.Id > 0 &&
            !string.IsNullOrEmpty(model.Description) &&
            model.Weight != 0 &&
            !string.IsNullOrEmpty(model.SelectedFromCountry) &&
            !string.IsNullOrEmpty(model.SelectedFromTown) &&
            !string.IsNullOrEmpty(model.FromAddress) &&
            !string.IsNullOrEmpty(model.SelectedToCountry) &&
            !string.IsNullOrEmpty(model.SelectedToTown) &&
            !string.IsNullOrEmpty(model.ToAddress) &&
            model.PickUpDate.HasValue &&
            model.DeliveryDate.HasValue &&
            !string.IsNullOrEmpty(model.Status))
        {
            var shipment = await _shipmentService.GetByIdAsync(model.Shipment.Id);
            if (shipment == null) return NotFound();
            shipment.Description = model.Description;
            shipment.Weight = model.Weight;
            shipment.FromCountry = model.SelectedFromCountry;
            shipment.FromTown = model.SelectedFromTown;
            shipment.FromAddress = model.FromAddress;
            shipment.ToCountry = model.SelectedToCountry;
            shipment.ToTown = model.SelectedToTown;
            shipment.ToAddress = model.ToAddress;
            shipment.PickUpDate = model.PickUpDate.Value;
            shipment.DeliveryDate = model.DeliveryDate.Value;
            shipment.Status = model.Status;
            await _shipmentService.UpdateAsync(shipment);
            await UpdateCompanyStockAssignment(model.SelectedCompanyStockId, shipment.Id);
            return RedirectToAction("ListOfShipments");
        }
        model = await PopulateDropdowns(model);
        return View(model);
    }
    private async Task UpdateCompanyStockAssignment(int? companyStockId, int shipmentId)
    {
        var existingAssignment = (await _companyStocksShipmentsRepository.GetAllAsync()).FirstOrDefault(cs => cs.Shipment_Id == shipmentId);
        if (existingAssignment != null)
        {
            if (companyStockId.HasValue)
                existingAssignment.CompanyStock_Id = companyStockId.Value;
            else
                await _companyStocksShipmentsRepository.RemoveAsync(existingAssignment); // Optionally remove if no company stock is selected
        }
        else if (companyStockId.HasValue)
        {
            await _companyStocksShipmentsRepository.AddAsync(new CompanyStocks_Shipments { Shipment_Id = shipmentId, CompanyStock_Id = companyStockId.Value });
        }
    }
    private async Task<ShipmentViewModel> PopulateDropdowns(ShipmentViewModel model)
    {
        model.PredefinedTowns = PredefinedTowns;
        model.FromCountries = new SelectList(PredefinedCountries);
        model.ToCountries = new SelectList(PredefinedCountries);
        model.FromTowns = new SelectList(
            model.SelectedFromCountry != null && PredefinedTowns.ContainsKey(model.SelectedFromCountry)
                ? PredefinedTowns[model.SelectedFromCountry]
                : new List<string>(),
            model.SelectedFromTown
        );
        model.ToTowns = new SelectList(
            model.SelectedToCountry != null && PredefinedTowns.ContainsKey(model.SelectedToCountry)
                ? PredefinedTowns[model.SelectedToCountry]
                : new List<string>(),
            model.SelectedToTown
        );
        var companyStocks = await _companyStockRepository.GetAllAsync();
        model.CompanyStocks = new SelectList(companyStocks, "Id", "Name", model.SelectedCompanyStockId);
        return model;
    }
    [HttpGet]
    public IActionResult GetTowns(string country)
    {
        if (string.IsNullOrEmpty(country) || !PredefinedTowns.ContainsKey(country))
        {
            return Json(new List<string>());
        }
        return Json(PredefinedTowns[country]);
    }
    private List<string> GetTownsByCountry(string country)
    {
        var predefinedTowns = new Dictionary<string, List<string>>
    {
        { "Bulgaria", new List<string> { "Sofia", "Plovdiv", "Varna" } },
        { "Turkey", new List<string> { "Istanbul", "Ankara", "Izmir" } },
        { "Romania", new List<string> { "Bucharest", "Cluj-Napoca", "Timisoara" } }
    };
        return predefinedTowns.ContainsKey(country) ? predefinedTowns[country] : new List<string>();
    }
}