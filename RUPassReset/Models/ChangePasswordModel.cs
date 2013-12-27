using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RUPassReset.Models
{
	public class ChangePasswordModel
	{
		[Required]
		public string UserName           { get; set; }
		[Required]
		public string PasswordOld        { get; set; }
		[Required]
		public string PasswordNew        { get; set; }
		[Required]
		public string PasswordNewConfirm { get; set; }
	}
}