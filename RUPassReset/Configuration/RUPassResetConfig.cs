using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RUPassReset.Configuration
{
	public class RUPassResetConfig : ConfigurationSection
	{
		private static RUPassResetConfig config = ConfigurationManager.GetSection("RUPassResetConfig") as RUPassResetConfig;

		public static RUPassResetConfig Config
		{
			get { return config; }
		}

		[ConfigurationProperty("tokenSize")]
		public int TokenSize
		{
			get { return (int) this["tokenSize"]; }
		}

		[ConfigurationProperty("tokenLifeTime")]
		public int TokenLifeTime
		{
			get { return (int) this["tokenLifeTime"]; }
		}

		[ConfigurationProperty("verifyTokenURL")]
		public string VerifyTokenURL
		{
			get { return (string)this["verifyTokenURL"]; }
		}

		[ConfigurationProperty("resetURL")]
		public string ResetURL
		{
			get { return (string) this["resetURL"]; }
		}

		[ConfigurationProperty("fromEmailAddress")]
		public string FromEmailAddress
		{
			get { return (string) this["fromEmailAddress"]; }
		}

		[ConfigurationProperty("smtpEmailServer")]
		public string SMTPEmailServer
		{
			get { return (string) this["smtpEmailServer"]; }
		}

		[ConfigurationProperty("minimumPasswordLength")]
		public int MinimumPasswordLength
		{
			get { return (int) this["minimumPasswordLength"]; }
		}
	}
}