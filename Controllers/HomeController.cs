using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IdentityManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityManagementSystem.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[Authorize]
	public IActionResult Secret()
	{
		if (User.IsInRole(Roles.Admin))
		{
			return View(new string[] { "Welcome", "Admin" });
		}
		return View(new string[] { "Welcome" });
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
