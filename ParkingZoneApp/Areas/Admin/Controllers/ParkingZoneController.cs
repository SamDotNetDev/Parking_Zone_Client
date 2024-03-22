using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var AllParkingZone = _repository.GetAll().ToList();
            return View(AllParkingZone);
        }

        // GET: Admin/ParkingZone/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();

            var ParkingZoneById = _repository.GetById(id);
            if (ParkingZoneById is not null)
            {
                return View(ParkingZoneById);
            }

            return NotFound();
        }

        // GET: Admin/ParkingZone/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ParkingZone parkingZone)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(parkingZone);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZone);
        }

        // GET: Admin/ParkingZone/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            var ParkingZoneById = _repository.GetById(id);
            if (ParkingZoneById is not null)
            {
                return View(ParkingZoneById);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ParkingZone parkingZone)
        {
            if (id != parkingZone.Id || !ParkingZoneExists(id)) 
                return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(parkingZone);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZone);
        }

        // GET: Admin/ParkingZone/Delete/5
        public IActionResult Delete(int id)
        {
            var ParkingZoneById = _repository.GetById(id);
            if (ParkingZoneById is not null)
            {
                return View(ParkingZoneById);
            }

            return NotFound();
        }

        // POST: Admin/ParkingZone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!ParkingZoneExists(id)||id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            _repository.Delete(id);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneExists(int id)
        {
            var ParkingZoneById = _repository.GetById(id);
            return true ? ParkingZoneById != null : false;
        }
    }
}
