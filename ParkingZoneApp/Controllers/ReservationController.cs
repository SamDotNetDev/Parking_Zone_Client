using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using ParkingZoneApp.ViewModels.Reservation;

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

        [HttpPost]
        public IActionResult FreeSlots(FreeSlotsVM freeSlotsVM)
        {
            var zone = _zoneService.GetById(freeSlotsVM.ParkingZoneId);
            ViewData["ParkingZoneName"] = zone.Name;
            freeSlotsVM.ParkingSlots = _slotService
                .GetFreeByParkingZoneIdAndPeriod(freeSlotsVM.ParkingZoneId, freeSlotsVM.StartTime, freeSlotsVM.Duration)
                .Select(x => new ListItemVM(x));
            return View(freeSlotsVM);
        }
    }
}
