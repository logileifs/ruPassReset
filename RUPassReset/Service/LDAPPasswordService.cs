using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using RUPassReset.Service.DBModels;
using RUPassReset.Service.Models.Password;

namespace RUPassReset.Service
{
	public class LDAPPasswordService
	{
		public static void ResetPassword(string username, string newPassword)
		{
			var dirEntry = new DirectoryEntry("LDAP://hirdc2.hir.is:636", "PasswordReset-ldap", "iez4queev0Aruemeech6");
			dirEntry.Path = "LDAP://OU=People,DC=hir,DC=is";
			dirEntry.AuthenticationType = AuthenticationTypes.SecureSocketsLayer;

			var searcher = new DirectorySearcher(dirEntry);
			searcher.Filter = String.Format("(&(objectClass=user)(sAMAccountName={0}))", username);

			var result = searcher.FindOne();

			if (result != null)
			{
				var userEntry = result.GetDirectoryEntry();
				userEntry.Invoke("SetPassword", new object[] {newPassword});
				userEntry.CommitChanges();
				userEntry.Close();
			}
		}
	}
}