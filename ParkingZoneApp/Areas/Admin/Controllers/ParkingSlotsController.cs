using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;

namespace ParkingZoneApp.Areas.Admin
{
    [Authorize]
    [Area("Admin")]
    public class ParkingSlotsController : Controller
    {
        private readonly IParkingSlotsService _service;

        public ParkingSlotsController(IParkingSlotsService service)
        {
            _service = service;
        }

        public IActionResult Index(int ParkingZoneId)
        {
            var parkingSlots = _service.GetAll();
            var Vms = parkingSlots
                .Where(x => x.ParkingZoneId == ParkingZoneId)
                .Select(x => new ListItemVM(x));
            return View(Vms);
        }
    }
}
