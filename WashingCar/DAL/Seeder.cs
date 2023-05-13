using WashingCar.DAL.Entities;

namespace WashingCar.DAL
{
    public class Seeder
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public Seeder(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await PopulateServicesAsync();

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        private async Task PopulateServicesAsync()
        {
            if (!_context.Services.Any())
            {
                _context.Services.Add(new Service { Name = "Lavada simple", Price = 25000});
                _context.Services.Add(new Service { Name = "Lavada + Polishada", Price = 50000 });
                _context.Services.Add(new Service { Name = "Lavada + Aspirada de Cojinería", Price = 30000 });
                _context.Services.Add(new Service { Name = "Lavada Full", Price = 65000 });
                _context.Services.Add(new Service { Name = "Lavada en seco del Motor", Price = 80000 });
                _context.Services.Add(new Service { Name = "Lavada Chasis", Price = 90000 });
            }
        }
        #endregion
    }
}
