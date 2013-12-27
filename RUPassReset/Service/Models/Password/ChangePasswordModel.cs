using System.ComponentModel.DataAnnotations;

namespace RUPassReset.Service.Models.Password
{
	public class ChangePasswordModel : PasswordModel
	{
		[Required(ErrorMessage = "Please provide your old password.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Please provide a new password.")]
		[StringLength(100, ErrorMessage = "The new password must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string PasswordNew        { get; set; }

		[Required(ErrorMessage = "Please confirm your new password.")]
		[DataType(DataType.Password)]
		[Compare("PasswordNew", ErrorMessage = "The new password and confirmation password do not match.")]
		public string PasswordNewConfirm { get; set; }
	}
}