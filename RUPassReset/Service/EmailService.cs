using System;
using System.Net.Mail;
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Service
{
	public class EmailService
	{
		public void sendPasswordResetEmail(User user, string key)
		{
			var message = new MailMessage();
			message.To.Add(user.Email);
			message.Subject = "Resetting your password";
			message.Body = String.Format("<p>Hi {0}! Please click <a href='http://mbl.is'>here</a>' to reset your password.</p>", user.Username);
			message.IsBodyHtml = true;
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
			var smtp = new SmtpClient("smtp.ru.is");
			message.From = new System.Net.Mail.MailAddress("no-reply@ru.is");
			smtp.Send(message);
		}
	}
}