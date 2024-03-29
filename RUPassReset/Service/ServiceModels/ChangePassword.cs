﻿using System.ComponentModel.DataAnnotations;
using RUPassReset.Configuration;

namespace RUPassReset.Service.Models.Password
{
	public class ChangePassword
	{
		public string Token { get; set; }
		public string Username { get; set; }

		[Required(ErrorMessage = "Please provide a new password.")]
		[DataType(DataType.Password)]
		public string PasswordNew        { get; set; }

		[Required(ErrorMessage = "Please confirm your new password.")]
		[DataType(DataType.Password)]
		[Compare("PasswordNew", ErrorMessage = "The new password and confirmation password do not match.")]
		public string PasswordNewConfirm { get; set; }
	}
}