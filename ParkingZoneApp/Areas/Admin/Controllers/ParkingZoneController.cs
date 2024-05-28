using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingZonesVMs;

namespace ParkingZoneApp.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ParkingZoneController : Controller
    {
        private readonly IParkingZoneService _zoneService;
        private readonly IReservationService _reservationService;

        public ParkingZoneController(IParkingZoneService zoneService, IReservationService reservationService)
        {
            _zoneService = zoneService;
            _reservationService = reservationService;
        }

        // GET: Admin/ParkingZone
        public IActionResult Index()
        {
            var parkingZones = _zoneService.GetAll();
            var Vms = parkingZones.Select(x => new ListItemVM(x));
            return View(Vms);
        }

        public IActionResult GetZoneFinanceData(PeriodEnum periodOption, int zoneId)
        {
            var zone = _zoneService.GetById(zoneId);

            if (zone is null)
                return NotFound();

            var data = _zoneService.GetZoneFinanceDataByPeriod(periodOption, zone);
            return Json(data);
        }

        // GET: Admin/ParkingZone/Details/5
        public IActionResult Details(int? id)
        {
            var parkingZone = _zoneService.GetById(id);
            if (parkingZone is null)
            {
                return NotFound();
            }
            var VM = new DetailsVM(parkingZone);
            return View(VM);
        }

        // GET: Admin/ParkingZone/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateVM VM)
        {
            if (ModelState.IsValid)
            {
                var parkingZone = VM.MapToModel();
                _zoneService.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }
            return View(VM);
        }

        // GET: Admin/ParkingZone/Edit/5
        public IActionResult Edit(int? id)
        {
            var parkingZone = _zoneService.GetById(id);
            if (parkingZone is null)
            {
                return NotFound();
            }
            var VM = new EditVM(parkingZone);
            return View(VM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditVM VM)
        {
            if (id != VM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var parkingZone = _zoneService.GetById(id);
                    parkingZone = VM.MapToModel(parkingZone);
                    _zoneService.Update(parkingZone);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingZoneExists(VM.Id))
                        return NotFound();

                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Admin/ParkingZone/Delete/5
        public IActionResult Delete(int? id)
        {
            var parkingZone = _zoneService.GetById(id);
            if (parkingZone is null)
            {
                return NotFound();
            }
            return View(parkingZone);
        }

        // POST: Admin/ParkingZone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var existingParkingZone = _zoneService.GetById(id);
            if (existingParkingZone is null)
            {
                return NotFound();
            }

            _zoneService.Delete(existingParkingZone);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CurrentCars(int? id)
        {
            var parkingZone = _zoneService.GetById(id);
            if(parkingZone is null)
            {
                return NotFound();
            }

            ViewData["ParkingZoneName"] = parkingZone.Name;
            var reservations = _reservationService.GetAllReservationsByParkingZoneId(parkingZone.Id);
            var VMs = reservations.Select(x => new ViewModels.ParkingSlotVMs.CurrentCarsVM(x));

            return View(VMs);
        }

        private bool ParkingZoneExists(int? id)
        {
            var ParkingZone = _zoneService.GetById(id);
            return ParkingZone != null;
        }
    }
}
