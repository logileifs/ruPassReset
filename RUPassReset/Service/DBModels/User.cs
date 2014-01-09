using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.DBModels
{
	public class User
	{
		public int    ID       { get; set; }
		public string Username { get; set; }
		public string SSN      { get; set; }
		public string Email    { get; set; }
	}
}