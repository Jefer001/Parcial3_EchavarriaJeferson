using WashingCar.DAL.Entities;
using WashingCar.Enum;
using WashingCar.Helpers;

namespace WashingCar.DAL
{
    public class Seeder
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IUserHelper _userHelper;
        #endregion

        #region Builder
        public Seeder(DataBaseContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        #endregion

        #region Public methods
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            PopulateServices();
            await PopulateRolesAsync();
            await PopulateUserAsync("Admin", "Last Name Role", "adminrole@yopmail.com", "Phone 3002323232", "Address Street Fighter", "Doc 102030", UserType.Admin);
            await PopulateUserAsync("User", "Last Name Role", "userrole@yopmail.com", "Phone 3502323232", "AddressStreet Fighter 2", "Doc 405060", UserType.User);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        private void PopulateServices()
        {
            if (!_context.Services.Any())
            {
                _context.Services.Add(new Service { Name = "Lavada simple", Price = 25000 });
                _context.Services.Add(new Service { Name = "Lavada + Polishada", Price = 50000 });
                _context.Services.Add(new Service { Name = "Lavada + Aspirada de Cojinería", Price = 30000 });
                _context.Services.Add(new Service { Name = "Lavada Full", Price = 65000 });
                _context.Services.Add(new Service { Name = "Lavada en seco del Motor", Price = 80000 });
                _context.Services.Add(new Service { Name = "Lavada Chasis", Price = 90000 });
            }
        }

        private async Task PopulateRolesAsync()
        {
            await _userHelper.AddRoleAsync(UserType.Admin.ToString());
            await _userHelper.AddRoleAsync(UserType.User.ToString());
        }

        private async Task PopulateUserAsync(string firstName, string lastName, string email, string phone, string address, string document, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);

            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
        }
        #endregion
    }
}
