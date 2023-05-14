using Microsoft.AspNetCore.Mvc;
using WashingCar.DAL;
using WashingCar.Helpers;
using WashingCar.Models;
using WashingCar.Services;

namespace WashingCar.Controllers
{
    public class AccountController : Controller
    {
        #region Constants
        private readonly IUserHelper _userHelper;
        #endregion

        #region Builder
        public AccountController(IUserHelper userHelper)
        { 
            _userHelper = userHelper;
        }
        #endregion

        #region Login action
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(loginViewModel);

                if (result.Succeeded) return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }
        #endregion
    }
}
