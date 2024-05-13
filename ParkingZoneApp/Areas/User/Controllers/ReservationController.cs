using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ReservationController : Controller
    {
        private readonly IParkingZoneService _zoneService;
        private readonly IParkingSlotService _slotService;
        private readonly IReservationService _reservationService;

        public ReservationController(IParkingZoneService zoneService, IParkingSlotService slotService, IReservationService reservationService)
        {
            _zoneService = zoneService;
            _slotService = slotService;
            _reservationService = reservationService;
        }

        public ActionResult Index()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _reservationService.ReservationsByUserId(UserId);
            var reservationsHistoryVMs = reservations
                .Select(x => new ReservationHistoryListItemVM(x))
                .OrderByDescending(x => x.StartTime);
            return View(reservationsHistoryVMs);
        }

        public IActionResult Prolong(int reservationId)
        {
            var reservation = _reservationService.GetById(reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            ProlongVM prolongVM = new(reservation);
            return View(prolongVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Prolong(ProlongVM prolongVM)
        {
            var reservation = _reservationService.GetById(prolongVM.ReservationId);
            var isFree = _slotService.IsSlotFreeForReservation(reservation.ParkingSlot, prolongVM.EndTime, prolongVM.AddHours);
            if (!isFree)
            {
                ModelState.AddModelError("AddHours", "Slot is not free for chosen time period");
                return View(prolongVM);
            }

            _reservationService.ProlongReservation(reservation, prolongVM.AddHours);

            return RedirectToAction("Index");
        }
    }
}
