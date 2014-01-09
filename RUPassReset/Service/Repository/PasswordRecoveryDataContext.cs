using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RUPassReset.Service.Models;

namespace RUPassReset.Service.Repository
{
	public class PasswordRecoveryDataContext : DbContext
	{
		public PasswordRecoveryDataContext() : base("name=PasswordRecoveryDataContext")
		{
			Database.SetInitializer<PasswordRecoveryDataContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Configurations.Add(new PasswordRecoveryMap());
		}

		public DbSet<PasswordRecovery> PasswordRecovery { get; set; }
	}
}