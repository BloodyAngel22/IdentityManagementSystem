using Microsoft.AspNetCore.Identity;

namespace IdentityManagementSystem.Models
{
	public class AppUser : IdentityUser
	{
	}
	public static class Roles
	{
		public const string Admin = "Admin";
		public const string User = "User";
	}
}