using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.Exceptions
{
	public class TooManyTriesException : Exception
	{
		public TooManyTriesException() {}
		public TooManyTriesException(string message):base(message) {}
		public TooManyTriesException(string message, Exception ex):base(message, ex) {}
	}
}