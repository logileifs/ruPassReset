using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.DBModels
{
	public class UserMap : EntityTypeConfiguration<User>
	{
		public UserMap()
		{
			this.ToTable("Users");

			this.HasKey(t => t.ID);

			this.Property(t => t.ID).HasColumnName("ID_User");
			this.Property(t => t.Username).HasColumnName("sUser");
			this.Property(t => t.SSN).HasColumnName("sReference");
			this.Property(t => t.Email).HasColumnName("sEmail");
		}
	}
}