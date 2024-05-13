using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IParkingSlotService _slotService;
        private readonly IParkingZoneService _zoneService;

        public ReservationController(IReservationService reservationService ,IParkingSlotService slotService, IParkingZoneService zoneService)
        {
            _reservationService = reservationService;
            _slotService = slotService;
            _zoneService = zoneService;
        }

        public IActionResult FreeSlots()
        {
            var zones = _zoneService.GetAll();
            FreeSlotsVM freeSlotsVM = new(zones);
            return View(freeSlotsVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult FreeSlots(FreeSlotsVM freeSlotsVM)
        {
            var zones = _zoneService.GetAll();
            freeSlotsVM.ParkingZones = new SelectList(zones, "Id", "Name");

            if (_reservationService.IsDateInvalid(freeSlotsVM.StartTime))
            {
                ModelState.AddModelError("StartTime", "Start time cannot be in the past.");
                return View(freeSlotsVM);
            }

            freeSlotsVM.ParkingSlots = _slotService
                .GetFreeByParkingZoneIdAndPeriod(freeSlotsVM.ParkingZoneId, freeSlotsVM.StartTime, freeSlotsVM.Duration)
                .Select(x => new ListItemVM(x));

            return View(freeSlotsVM);
        }

        public IActionResult Reserve(int parkingSlotId, DateTime startTime, int duration)
        {
            var parkingSlot = _slotService.GetById(parkingSlotId);

            if (parkingSlot == null)
                return NotFound();

            ReserveVM reserveVM = new(parkingSlot, startTime, duration);

            return View(reserveVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Reserve(ReserveVM reserveVM)
        {
            var ParkingSlot = _slotService.GetById(reserveVM.ParkingSlotId);

            if(ParkingSlot == null)
                return NotFound();

            reserveVM.ParkingSlotNumber = ParkingSlot.Number;
            reserveVM.ParkingZoneName = ParkingSlot.ParkingZone.Name;
            reserveVM.ParkingZoneAddress = ParkingSlot.ParkingZone.Address;

            bool IsSlotFree = _slotService.IsSlotFreeForReservation(ParkingSlot, reserveVM.StartTime, reserveVM.Duration);
            if (!IsSlotFree)
            {
                ModelState.AddModelError("StartTime", "Slot is not available for selected period");
                ModelState.AddModelError("Duration", "Slot is not available for selected period");
                return View(reserveVM);
            }
            if (_reservationService.IsDateInvalid(reserveVM.StartTime))
            {
                ModelState.AddModelError("StartTime", "Start time cannot be in the past.");
                return View(reserveVM);
            }

            var reservation = reserveVM.MapToModel();
            reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _reservationService.Insert(reservation);
            ViewBag.SuccessMessage = "Reservation successful.";

            return View(reserveVM);
        }
    }
}