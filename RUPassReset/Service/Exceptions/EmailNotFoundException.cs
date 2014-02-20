using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.Exceptions
{
	public class EmailNotFoundException : Exception
	{
		public EmailNotFoundException () {}

		public EmailNotFoundException (string message): base (message) {}

		public EmailNotFoundException (string message, Exception inner): base (message, inner) {}
	}
}