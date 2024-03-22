using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
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
            return View(await _context.ParkingZone.ToListAsync());
        }

        // GET: ParkingZone/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingZoneModel = await _context.ParkingZone
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Address,DateOfEstablishment")] ParkingZone parkingZoneModel)
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

            var parkingZoneModel = await _context.ParkingZone.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,DateOfEstablishment")] ParkingZone parkingZoneModel)
        {
            if (id != parkingZoneModel.Id)
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
                    if (!ParkingZoneModelExists(parkingZoneModel.Id))
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

            var parkingZoneModel = await _context.ParkingZone
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var parkingZoneModel = await _context.ParkingZone.FindAsync(id);
            if (parkingZoneModel != null)
            {
                _context.ParkingZone.Remove(parkingZoneModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingZoneModelExists(int id)
        {
            return _context.ParkingZone.Any(e => e.Id == id);
        }
    }
}
