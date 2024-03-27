using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Areas.Admin
{
    [Authorize]
    [Area("Admin")]
    public class ParkingZoneController : Controller
    {
        private readonly IParkingZoneRepository _repository;

        public ParkingZoneController(IParkingZoneRepository repository)
        {
            _repository = repository;
        }

        // GET: Admin/ParkingZone
        public IActionResult Index()
        {
            var ParkingZone = _repository.GetAll().ToList();
            return View(ParkingZone);
        }

        // GET: Admin/ParkingZone/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ParkingZone = _repository.GetById(id);
            if (ParkingZone is null)
            {
                return NotFound();
            }

            return View(ParkingZone);
        }

        // GET: Admin/ParkingZone/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkingZone parkingZone)
        {
            if (ModelState.IsValid)
            {
                parkingZone.DateOfEstablishment = DateTime.Now;
                _repository.Insert(parkingZone);
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZone);
        }

        // GET: Admin/ParkingZone/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ParkingZone = _repository.GetById(id);
            if (ParkingZone is null)
            {
                return NotFound();
            }

            return View(ParkingZone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ParkingZone parkingZone)
        {
            if (id != parkingZone.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(parkingZone);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingZoneExists(parkingZone.Id))
                        return NotFound();

                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZone);
        }

        // GET: Admin/ParkingZone/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ParkingZone = _repository.GetById(id);
            if (ParkingZone is null)
                return NotFound();

            return View(ParkingZone);
        }

        // POST: Admin/ParkingZone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var ParkingZone = _repository.GetById(id);
            _repository.Delete(ParkingZone);
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(int id)
        {
            var ParkingZone = _repository.GetById(id);
            return true ? ParkingZone != null : false;
        }
    }
}
