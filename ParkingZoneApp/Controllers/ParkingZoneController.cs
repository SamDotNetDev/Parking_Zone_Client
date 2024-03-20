using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Controllers
{
    public class ParkingZoneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingZoneController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParkingZone
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parkin_Zone.ToListAsync());
        }

        // GET: ParkingZone/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingZoneModel = await _context.Parkin_Zone
                .FirstOrDefaultAsync(m => m.ParkingZoneId == id);
            if (parkingZoneModel == null)
            {
                return NotFound();
            }

            return View(parkingZoneModel);
        }

        // GET: ParkingZone/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingZone/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParkingZoneId,ParkingZoneName,Adress,ParkingZoneDateOfEstablishment")] ParkingZoneModel parkingZoneModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingZoneModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZoneModel);
        }

        // GET: ParkingZone/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingZoneModel = await _context.Parkin_Zone.FindAsync(id);
            if (parkingZoneModel == null)
            {
                return NotFound();
            }
            return View(parkingZoneModel);
        }

        // POST: ParkingZone/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParkingZoneId,ParkingZoneName,Adress,ParkingZoneDateOfEstablishment")] ParkingZoneModel parkingZoneModel)
        {
            if (id != parkingZoneModel.ParkingZoneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingZoneModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingZoneModelExists(parkingZoneModel.ParkingZoneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parkingZoneModel);
        }

        // GET: ParkingZone/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingZoneModel = await _context.Parkin_Zone
                .FirstOrDefaultAsync(m => m.ParkingZoneId == id);
            if (parkingZoneModel == null)
            {
                return NotFound();
            }

            return View(parkingZoneModel);
        }

        // POST: ParkingZone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingZoneModel = await _context.Parkin_Zone.FindAsync(id);
            if (parkingZoneModel != null)
            {
                _context.Parkin_Zone.Remove(parkingZoneModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneModelExists(int id)
        {
            return _context.Parkin_Zone.Any(e => e.ParkingZoneId == id);
        }
    }
}
