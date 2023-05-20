using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WashingCar.DAL;
using WashingCar.DAL.Entities;
using WashingCar.Helpers;
using WashingCar.Models;

namespace WashingCar.Controllers
{
    [Authorize(Roles = "User")]
    public class VehiclesController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IDropDownListHelper _dropDownListHelper;
        #endregion

        #region Builder
        public VehiclesController(DataBaseContext context, IDropDownListHelper dropDownListHelper)
        {
            _context = context;
            _dropDownListHelper = dropDownListHelper;
        }
        #endregion

        #region Vehicle actios
        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicles
                .ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Vehicles == null) return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.Service)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public async Task<IActionResult> Create()
        {
            AddVehicleViewModel addVehicleViewModel = new()
            {
                Services = await _dropDownListHelper.GetDDLServicesAsync()
            };
            return View(addVehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.Id = Guid.NewGuid();
                    vehicle.CreationDate = DateTime.Now;
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un vehículo con la misma placa.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Vehicles == null) return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Vehicle vehicle)
        {
            if (id != vehicle.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un vehículo con la misma placa.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vehicles == null) return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id.Equals(id));

            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vehicles == null) return Problem("Entity set 'DataBaseContext.Vehicles'  is null.");

            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle != null) _context.Vehicles.Remove(vehicle);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(Guid id)
        {
            return _context.Vehicles.Any(e => e.Id.Equals(id));
        }
        #endregion
    }
}
