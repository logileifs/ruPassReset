using System;
using System.Net.Mail;
using RUPassReset.Configuration;
using RUPassReset.Service.DBModels;

namespace RUPassReset.Service
{
	public class EmailService
	{
		public void sendPasswordResetEmail(UserDTO user, string token)
		{
			var message = new MailMessage();
			message.To.Add(user.SecondaryEmail);
			message.Subject = "How to reset your password";
			message.IsBodyHtml = true;

			var body = String.Format("<p>Dear {0},</p>", user.Name);
			body += "<p>You have requested to reset your password for your account at Reykjavík University. To complete the process, please follow the link below.</p>";
			body += String.Format("<p><a href='{0}{1}'>Reset now</a></p>", RUPassResetConfig.Config.VerifyTokenURL, token);
			body += String.Format("<p>This link will expire {0} hours after this email was sent.</p>", RUPassResetConfig.Config.TokenLifeTime);
			body += ("<p>If you didn't make this request yourself, it could be that somebody else has entered your social security number by mistake and your password is still the same.</p>");
			body += ("<p><br>Reykjavík University IT Support</p>");
			message.Body = body;

			send(message);
		}

		public void sendPasswordChangedConfirmation(UserDTO user)
		{
			var message = new MailMessage();
			message.To.Add(user.SecondaryEmail);
			message.Subject = "Your RU account password has been reset!";
			message.IsBodyHtml = true;

			var body = String.Format("<p>Dear {0},</p>", user.Name);
			body += String.Format("<p>The password for your RU account(<i>{0}</i>) has been successfully reset.</p>", user.Username);
			body += String.Format("<p>If you did not make this change or if you believe an unauthorized person has changed your password, go and <a href='{0}'>reset</a> your password immediately. Then update your security settings for this email account.");
			body += "<p>If you need additional help, go to <a href='http://help.ru.is'>help.ru.is</a></p>";
			body += ("<p><br>Reykjavík University IT Support</p>");
			message.Body = body;

			send(message);
		}

		private void send(MailMessage message)
		{
			var smtp = new SmtpClient(RUPassResetConfig.Config.SMTPEmailServer);
			message.From = new System.Net.Mail.MailAddress(RUPassResetConfig.Config.FromEmailAddress);
			smtp.Send(message);
		}
	}
}