using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;

namespace ParkingZoneApp.Areas.Admin
{
    [Authorize]
    [Area("Admin")]
    public class ParkingSlotsController : Controller
    {
        private readonly IParkingSlotsService _slotService;
        private readonly IParkingZoneService _zoneService;

        public ParkingSlotsController(IParkingSlotsService slotService, IParkingZoneService zoneService)
        {
            _slotService = slotService;
            _zoneService = zoneService;

        }

        public IActionResult Index(int ParkingZoneId)
        {
            var VMs = _slotService.GetByParkingZoneId(ParkingZoneId)
                .Select(x=> new ListItemVM(x));
            ViewData["Name"] = _zoneService.GetById(ParkingZoneId).Name;
            return View(VMs);
        }
    }
}
