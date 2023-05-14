using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WashingCar.DAL;
using WashingCar.DAL.Entities;

namespace WashingCar.Controllers
{
    public class ServicesController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public ServicesController(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Service actions
        // GET: Services
        public async Task<IActionResult> Index()
        {
            return _context.Services != null ?
                        View(await _context.Services
                        .Include(v => v.Vehicles)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Services'  is null.");
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Services == null) return NotFound();

            var service = await _context.Services
                .Include(v => v.Vehicles)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (service == null) return NotFound();

            return View(service);;
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Id = Guid.NewGuid();
                    _context.Add(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ViewBag.ErrorMessage("Ya existe un servicio con el mismo nombre.");
                    //ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Services == null) return NotFound();

            var service = await _context.Services.FindAsync(id);

            if (service == null) return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Service service)
        {
            if (id != service.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un servicio con el mismo nombre.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Services == null) return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Services == null) return Problem("Entity set 'DataBaseContext.Services'  is null.");

            var service = await _context.Services.FindAsync(id);

            if (service != null) _context.Services.Remove(service);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
