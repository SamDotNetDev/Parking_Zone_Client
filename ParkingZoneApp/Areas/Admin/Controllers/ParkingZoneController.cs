using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingZones;

namespace ParkingZoneApp.Areas.Admin
{
    [Authorize]
    [Area("Admin")]
    public class ParkingZoneController : Controller
    {
        private readonly IParkingZoneService _service;

        public ParkingZoneController(IParkingZoneService service)
        {
            _service = service;
        }

        // GET: Admin/ParkingZone
        public IActionResult Index()
        {
            var parkingZones = _service.GetAll();
            var VM = new ListItemVM().MapToVM(parkingZones);
            return View(VM);
        }

        // GET: Admin/ParkingZone/Details/5
        public IActionResult Details(int? id)
        {
            var parkingZone = _service.GetById(id);
            if (parkingZone is null)
            {
                return NotFound();
            }
            var VM = new DetailsVM().MapToVM(parkingZone);
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
                _service.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }
            return View(VM);
        }

        // GET: Admin/ParkingZone/Edit/5
        public IActionResult Edit(int? id)
        {
            var parkingZone = _service.GetById(id);
            if (parkingZone is null)
            {
                return NotFound();
            }
            var VM = new EditVM().MapToVM(parkingZone);
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
                    var ParkingZone = VM.MapToModel(VM);
                    _service.Update(ParkingZone);
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
            var parkingZone = _service.GetById(id);
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
            var existingParkingZone = _service.GetById(id);
            if (existingParkingZone is null)
            {
                return NotFound();
            }

            _service.Delete(existingParkingZone);
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(int id)
        {
            var ParkingZone = _service.GetById(id);
            return true ? ParkingZone != null : false;
        }
    }
}
