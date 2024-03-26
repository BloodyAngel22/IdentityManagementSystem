using System.ComponentModel.DataAnnotations;

namespace IdentityManagementSystem.Models
{
	public class CreateModel 
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		public string Role { get; set; } = string.Empty;
	}
	public class LoginModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
	public class RegisterModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
	public class EditModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}