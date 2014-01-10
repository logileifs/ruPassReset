using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.DBModels
{
	public class UserDTO
	{
		public string Name { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string SecondaryEmail { get; set; }
	}
}