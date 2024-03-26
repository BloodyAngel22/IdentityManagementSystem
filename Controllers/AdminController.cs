using IdentityManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityManagementSystem.Controllers
{
	public class AdminController : Controller
	{
		private UserManager<AppUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		private readonly ILogger<AdminController> _logger;

		private IUserValidator<AppUser> _userValidator;
		private IPasswordValidator<AppUser> _passwordValidator;
		private IPasswordHasher<AppUser> _passwordHasher;

		public AdminController(ILogger<AdminController> logger, UserManager<AppUser> userManager, IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator, IPasswordHasher<AppUser> passwordHasher, RoleManager<IdentityRole> roleManager)
		{
			_logger = logger;
			_userManager = userManager;
			_userValidator = userValidator;
			_passwordValidator = passwordValidator;
			_passwordHasher = passwordHasher;
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			return View(_userManager.Users);
		}

		[HttpGet]
		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(CreateModel model, string role)
		{
			model.Role = role;
			await RoleCreate();

			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = model.Name,
					Email = model.Email
				};
				IdentityResult result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

					if (!roleResult.Succeeded)
					{
						foreach (var error in roleResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
					else
					{
						return RedirectToAction(nameof(Index));
					}
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user != null)
			{
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return RedirectToAction(nameof(Error));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user != null)
			{
				return View(user);
			}
			return RedirectToAction(nameof(Error));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AppUser model, string password)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);

				if (user != null)
				{
					user.UserName = model.UserName;

					var validUser = await _userValidator.ValidateAsync(_userManager, user);
					if (!validUser.Succeeded)
					{
						foreach (var error in validUser.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
					else
					{
						user.Email = model.Email;
						var validEmail = await _userValidator.ValidateAsync(_userManager, user);

						if (!validEmail.Succeeded)
						{
							foreach (var error in validEmail.Errors)
							{
								ModelState.AddModelError(string.Empty, error.Description);
							}
						}
						else
						{
							var validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);

							if (!validPass.Succeeded)
							{
								foreach (var error in validPass.Errors)
								{
									ModelState.AddModelError(string.Empty, error.Description);
								}
							}
							else
							{
								user.PasswordHash = _passwordHasher.HashPassword(user, password);
								var result = await _userManager.UpdateAsync(user);
								if (result.Succeeded)
								{
									return RedirectToAction(nameof(Index));
								}
								foreach (var error in result.Errors)
								{
									ModelState.AddModelError(string.Empty, error.Description);
								}
							}
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "User Not Found");
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