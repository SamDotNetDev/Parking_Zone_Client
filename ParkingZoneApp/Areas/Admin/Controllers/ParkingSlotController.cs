using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Areas.Admin
{
    [Authorize]
    [Area("Admin")]
    public class ParkingSlotController : Controller
    {
        private readonly IParkingSlotService _slotService;
        private readonly IParkingZoneService _zoneService;

        public ParkingSlotController(IParkingSlotService slotService, IParkingZoneService zoneService)
        {
            _slotService = slotService;
            _zoneService = zoneService;
        }

        public IActionResult Index(int ParkingZoneId)
        {
            var VMs = _slotService.GetByParkingZoneId(ParkingZoneId)
                .Select(x => new ListItemVM(x));
            ViewData["Name"] = _zoneService.GetById(ParkingZoneId).Name;
            ViewData["ParkingZoneId"] = ParkingZoneId;
            return View(VMs);
        }

        public IActionResult Create(int ParkingZoneId)
        {
            CreateVM createVM = new()
            {
                ParkingZoneId = ParkingZoneId
            };
            return View(createVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateVM VM)
        {
            if (ModelState.IsValid)
            {
                var ParkingSlotExists = _slotService.ParkingSlotExits(VM.ParkingZoneId, VM.Number);
                if(VM.Number <= 0)
                {
                    ModelState.AddModelError("Number", "Number can not be 0 or negative");
                    return View(VM);
                }
                else if (ParkingSlotExists == true)
                {
                    ModelState.AddModelError("Number", "Parking Slot Exists");
                    return View(VM);
                }
                var parkingSlot = VM.MapToModel();
                _slotService.Insert(parkingSlot);
                return RedirectToAction(nameof(Index), new { ParkingZoneId = parkingSlot.ParkingZoneId });
            }
            return View(VM);
        }
    }
}
