
using System.Collections.Generic;
using RUPassReset.Service.Repository;
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Service
{
	public class PasswordService
	{
		private readonly PasswordRecoveryDataContext _passCtx;
		private EmailService _emailService;

		public PasswordService()
		{
			_passCtx = new PasswordRecoveryDataContext();
			_emailService = new EmailService();
		}

		public IEnumerable<PasswordRecovery> reset()
		{
			return _passCtx.PasswordRecovery;
		}
	}
}