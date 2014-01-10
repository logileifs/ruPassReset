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
			body += String.Format("<p><a href='{0}{1}'>Reset now</a></p>", RUPassResetConfig.Config.ResetURL, token);
			body += String.Format("<p>This link will expire {0} hours after this email was sent.</p>", RUPassResetConfig.Config.TokenLifeTime);
			body += ("<p>If you didn't make this request yourself, it could be that somebody else has entered your social security number by mistake and your password is still the same.</p>");
			body += ("<p><br>Reykjavík University IT Support</p>");
			message.Body = body;
			send(message);
		}

		public void sendPasswordChangedConfirmation(User user)
		{
			var message = new MailMessage();
			message.To.Add(user.Email);
			message.Subject = "Your password has been reset!";
			message.Body = String.Format("<p>Hi {0}! Your password has been successfully reset.</p>", user.Username);
			message.IsBodyHtml = true;
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