using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WashingCar.DAL;
using WashingCar.DAL.Entities;

namespace WashingCar.Controllers
{
    public class VehiclesController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public VehiclesController(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Vehicle actions
        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return _context.Services != null ?
                        View(await _context.Vehicles
                        .Include(v => v.VehicleDetails)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Services'  is null.");
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Vehicles == null) return NotFound();

            var vehicle = await _context.Vehicles          
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (vehicle == null) return NotFound();

            return View(vehicle); ;
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
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
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un vehiculo con la mismo número de placa.");
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
            if (vehicle == null)
            {
                return NotFound();
            }
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
                        ModelState.AddModelError(string.Empty, "Ya existe un vehiculo con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vehicles == null) return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vehicles == null) return Problem("Entity set 'DataBaseContext.Vehicle'  is null.");

            var service = await _context.Vehicles.FindAsync(id);

            if (service != null) _context.Vehicles.Remove(service);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
