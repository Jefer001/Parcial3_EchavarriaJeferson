using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WashingCar.DAL;
using WashingCar.DAL.Entities;
using WashingCar.Helpers;

namespace WashingCar.Services
{
    public class UserHelper : IUserHelper
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region Builder
        public UserHelper(DataBaseContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Public methods
        public async Task AddRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(string email)
        {
            return await _context.Users.FindAsync(email);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users
                .FindAsync(email);
        }
        #endregion
    }
}
