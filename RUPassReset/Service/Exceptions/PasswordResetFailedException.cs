using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.Exceptions
{
	public class PasswordResetFailedException : Exception
	{
		public PasswordResetFailedException() {}

		public PasswordResetFailedException(string message):base(message) {}

		public PasswordResetFailedException(string message, Exception ex):base(message, ex) {}
	}
}