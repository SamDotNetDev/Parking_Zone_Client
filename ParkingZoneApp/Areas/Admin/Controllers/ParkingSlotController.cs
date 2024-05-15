using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
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
                .Select(x => new ListItemVM(x))
                .OrderBy(x => x.Number);
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
            var ParkingSlotExists = _slotService.ParkingSlotExists(VM.ParkingZoneId, VM.Number);
            if (VM.Number <= 0 || ParkingSlotExists == true)
            {
                ModelState.AddModelError("Number", "Parking Slot exists or Number is not valid");
                return View(VM);
            }

            if (ModelState.IsValid)
            {
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

            var parkingSlot = _slotService.GetById(id);
            var ParkingSlotExists = _slotService.ParkingSlotExists(VM.ParkingZoneId, VM.Number);
            if (ParkingSlotExists == true && VM.Number != parkingSlot.Number)
            {
                ModelState.AddModelError("Number", "Parking Slot exists");
                return View(VM);
            }

            if (parkingSlot.IsInUse && parkingSlot.Category != VM.Category)
            {
                ModelState.AddModelError("Category", "Slot is in use, category cannot be modified");
                return View(VM);
            }

            if (ModelState.IsValid) 
            {
                parkingSlot = VM.MapToModel(parkingSlot);
                _slotService.Update(parkingSlot);
                return RedirectToAction(nameof(Index), new { ParkingZoneId = parkingSlot.ParkingZoneId });
            }
            return View(VM);
        }

        public IActionResult Details(int? id)
        {
            var parkingSlot = _slotService.GetById(id);
            if (parkingSlot is null)
            {
                return NotFound();
            }
            var VM = new DetailsVM(parkingSlot);
            ViewData["ParkingZoneId"] = parkingSlot.ParkingZoneId;
            return View(VM);
        }

        public IActionResult Delete(int? id)
        {
            var parkingSlot = _slotService.GetById(id);
            if (parkingSlot is null)
            {
                return NotFound();
            }
            return View(parkingSlot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var existingParkingSlot = _slotService.GetById(id);

            if (existingParkingSlot is null || existingParkingSlot.IsInUse)
            {
                return NotFound();
            }

            _slotService.Delete(existingParkingSlot);
            return RedirectToAction(nameof(Index), new {ParkingZoneId = existingParkingSlot.ParkingZoneId});
        }
    }
}
