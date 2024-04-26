using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
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
                if (VM.Number <= 0 || ParkingSlotExists == true)
                {
                    ModelState.AddModelError("Number", "Parking Slot exists or Number is not valid");
                    return View(VM);
                }
                var parkingSlot = VM.MapToModel();
                _slotService.Insert(parkingSlot);
                return RedirectToAction(nameof(Index), new { ParkingZoneId = parkingSlot.ParkingZoneId });
            }
            return View(VM);
        }

        public IActionResult Edit(int? id)
        {
            var parkingSlot = _slotService.GetById(id);
            if (parkingSlot is null)
            {
                return NotFound();
            }
            var VM = new EditVM(parkingSlot);
            return View(VM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditVM VM)
        {
            if(id != VM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                var parkingSlot = _slotService.GetById(id);
                var ParkingSlotExists = _slotService.ParkingSlotExits(VM.ParkingZoneId, VM.Number);
                if (VM.Number <= 0 || ParkingSlotExists == true && VM.Number != parkingSlot.Number)
                {
                    ModelState.AddModelError("Number", "Parking Slot exists or Number is not valid");
                    return View(VM);
                }
                parkingSlot = VM.MapToModel(parkingSlot);
                _slotService.Update(parkingSlot);
                return RedirectToAction(nameof(Index), new { ParkingZoneId = parkingSlot.ParkingZoneId });
            }
            return View(VM);
        }
    }
}
