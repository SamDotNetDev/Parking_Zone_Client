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
    }
}
