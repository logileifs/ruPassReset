using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RUPassReset.Service
{
	public class PasswordRecovery
	{
		public int      ID          { get; set; }
		public string   Token       { get; set; }
		public string   Username    { get; set; }
		public DateTime TimeStamp   { get; set; }
		public string   CreatedByIP { get; set; }
		public string   UsedByIP    { get; set; }
	}
}