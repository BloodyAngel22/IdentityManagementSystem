using IdentityManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagementSystem.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUser> _userManager;
		private SignInManager<AppUser> _signInManager;
		private RoleManager<IdentityRole> _roleManager;
		private readonly ILogger<AccountController> _logger;

		public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.Name);

				if (user != null)
				{
					await _signInManager.SignOutAsync();
					var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			}	
			return View(model);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model, string password)
		{
			await RoleCreate();

			if (model.Password != password)
			{
				ModelState.AddModelError(string.Empty, "Passwords do not match.");
			}
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = model.Name,
					Email = model.Email
				};

				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);

					if (roleResult.Succeeded)
					{
						return RedirectToAction(nameof(Login));
					}
					foreach (var error in roleResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View("Error!");
		}

		private async Task RoleCreate()
		{
			var roles = new List<IdentityRole>()
			{
				new IdentityRole { Name = Roles.Admin },
				new IdentityRole { Name = Roles.User }
			};

			if (await _roleManager.RoleExistsAsync(roles[0].Name) && await _roleManager.RoleExistsAsync(roles[1].Name))
			{
				return;
			}

			foreach (var role in roles)
			{
				var result = await _roleManager.CreateAsync(role);
			}
		}
	}
}