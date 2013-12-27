using System.ComponentModel.DataAnnotations;

namespace RUPassReset.Service.Models.Password
{
	public class Password
	{
		[Required(ErrorMessage = "Please provide a username.")]
		public string UserName { get; set; }
	}
}