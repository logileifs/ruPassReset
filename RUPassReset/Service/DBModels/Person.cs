using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.DBModels
{
	public class Person
	{
		public int ID { get; set; }
		public string SSN { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}