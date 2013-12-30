
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Service
{
	public class PasswordService
	{
		private EmailService _emailService;

		public PasswordService()
		{
			_emailService = new EmailService();
		}

		public void changePassword(User user, string newPassword)
		{
			// todo, change the password
			_emailService.sendPasswordChangedConfirmation(user);
		}
	}
}