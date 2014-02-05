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

		[ConfigurationProperty("maxAttempts")]
		public int MaxAttempts
		{
			get { return (int)this["maxAttempts"]; }
		}

		[ConfigurationProperty("maxAttemptsGagTime")]
		public int MaxAttemptsGagTime
		{
			get { return (int)this["maxAttemptsGagTime"]; }
		}

		[ConfigurationProperty("ADDomain")]
		public string ADDomain
		{
			get { return (string)this["ADDomain"]; }
		}

		[ConfigurationProperty("ADDefaultOU")]
		public string ADDefaultOU
		{
			get { return (string)this["ADDefaultOU"]; }
		}

		[ConfigurationProperty("ADDefaultRootOU")]
		public string ADDefaultRootOU
		{
			get { return (string)this["ADDefaultRootOU"]; }
		}

		[ConfigurationProperty("ADServiceUser")]
		public string ADServiceUser
		{
			get { return (string)this["ADServiceUser"]; }
		}

		[ConfigurationProperty("ADServicePassword")]
		public string ADServicePassword
		{
			get { return (string)this["ADServicePassword"]; }
		}
	}
}