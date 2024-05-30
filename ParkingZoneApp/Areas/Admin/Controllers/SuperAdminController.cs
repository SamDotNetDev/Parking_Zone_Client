using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using ParkingZoneApp.Data.Migrations;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.SuperAdminVMs;
using System.Security.Claims;

namespace ParkingZoneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public SuperAdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var users = _context.Users.ToList();
            var vms = new List<ListItemVM>();

            foreach (var user in users)
            {
                if(user.Id != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var vm = new ListItemVM(user)
                    {
                        Role = roles
                    };
                    vms.Add(vm);
                }
            }
            vms.OrderBy(x => x.Role).OrderBy(x => x.Name);
            return View(vms);
        }

        public async Task<IActionResult> Promote(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if(user == null)
            {
                return NotFound();
            }

            RoleConverter converter = new();
            PromoteVM vm = new(user);
            var role = await _userManager.GetRolesAsync(user);
            var userRole = converter.ConvertToUserRole(role.First());
            vm.Role = userRole;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Promote(string Id, PromoteVM vm)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if(user == null)
            {
                return NotFound();
            }

            if (Id != vm.Id)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            var result = await _userManager.AddToRoleAsync(user, $"{vm.Role}");
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(result);
            }
        }

        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletConfirmed(string Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(result);
            }
        }
    }
}
