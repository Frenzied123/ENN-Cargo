using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;namespace ENN_Cargo.Models
{
    public class AssignShipmentViewModel
    {
        public int ShipmentId { get; set; }        [Required(ErrorMessage = "Please select a driver.")]
        public int? DriverId { get; set; }        [Required(ErrorMessage = "Please select a vehicle.")]
        public int? VehicleId { get; set; }        public List<SelectListItem> DriverOptions { get; set; }        public List<SelectListItem> VehicleOptions { get; set; }
    }
}