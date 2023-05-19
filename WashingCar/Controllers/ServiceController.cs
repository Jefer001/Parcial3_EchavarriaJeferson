using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WashingCar.DAL;
using WashingCar.DAL.Entities;
using WashingCar.Models;

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
                        .Include(s => s.Vehicles)
                        .ToListAsync()) :
                        Problem("Entity set 'DataBaseContext.Services'  is null.");
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Services == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Vehicles)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (service == null) return NotFound();

            return View(service); ;
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
                        ModelState.AddModelError(string.Empty, "Ya existe un servicio con el mismo nombre.");
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

        #region Vehicle actions
        [HttpGet]
        public async Task<IActionResult> CreateVehicle(Guid? serviceId)
        {
            if (serviceId == null) return NotFound();

            Service service = await _context.Services
                .FirstOrDefaultAsync(c => c.Id == serviceId);

            if (serviceId == null) return NotFound();

            VehicleViewModel vehicleViewModel = new()
            {
                ServiceId = service.Id
            };
            return View(vehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(VehicleViewModel vehicleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Vehicle vehicle = new()
                    {
                        CreationDate = DateTime.Now,
                        Service = await _context.Services.FirstOrDefaultAsync(s => s.Id.Equals(vehicleViewModel.ServiceId)),
                        NumberPlate = vehicleViewModel.NumberPlate,
                        Owner = vehicleViewModel.Owner,
                        DeliveryDate = null
                    };

                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = vehicleViewModel.ServiceId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un vehiculo con la misma placa");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(vehicleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditVehicle(Guid? vehicleId)
        {
            if (vehicleId == null || _context.Vehicles == null) return NotFound();

            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.Service)
                .FirstOrDefaultAsync(s => s.Id.Equals(vehicleId));

            if (vehicle == null) return NotFound();

            VehicleViewModel vehicleViewModel = new()
            {
                ServiceId = vehicle.Service.Id,
                Id = vehicle.Id,
                Name = vehicle.Name,
                CreationDate = vehicle.CreationDate
            };
            return View(vehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(Guid serviceId, VehicleViewModel vehicleViewModel)
        {
            if (serviceId != vehicleViewModel.ServiceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Vehicle vehicle = new()
                    {
                        Id = vehicleViewModel.Id,
                        Name = vehicleViewModel.Name,
                        Owner = vehicleViewModel.Owner,
                        NumberPlate = vehicleViewModel.NumberPlate,
                        CreationDate = vehicleViewModel.CreationDate,
                        DeliveryDate = DateTime.Now
                    };
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { Id = vehicleViewModel.ServiceId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        ModelState.AddModelError(string.Empty, "Ya existe un vehiculo con la misma placa.");
                    else
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(vehicleViewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> DetailsState(Guid? stateId)
        //{
        //    if (stateId == null || _context.States == null) return NotFound();

        //    var state = await _context.States
        //        .Include(c => c.Country)
        //        .Include(c => c.Cities)
        //        .FirstOrDefaultAsync(s => s.Id == stateId);

        //    if (state == null) return NotFound();

        //    return View(state);
        //}

        //public async Task<IActionResult> DeleteState(Guid? stateId)
        //{
        //    if (stateId == null || _context.States == null) return NotFound();

        //    var state = await _context.States
        //        .Include(c => c.Country)
        //        .Include(c => c.Cities)
        //        .FirstOrDefaultAsync(c => c.Id == stateId);

        //    if (state == null) return NotFound();

        //    return View(state);
        //}

        //[HttpPost, ActionName("DeleteState")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteStateConfirmed(Guid stateId)
        //{
        //    if (_context.States == null) return Problem("Entity set 'DataBaseContext.State'  is null.");

        //    var state = await _context.States
        //        .Include(c => c.Country)
        //        .Include(c => c.Cities)
        //        .FirstOrDefaultAsync(c => c.Id == stateId);

        //    if (state != null) _context.States.Remove(state);

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Details), new { id = state.Country.Id });
        //}
        #endregion
    }
}