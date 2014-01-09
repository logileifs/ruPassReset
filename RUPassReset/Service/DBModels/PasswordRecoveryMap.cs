using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.Models
{
	public class PasswordRecoveryMap : EntityTypeConfiguration<PasswordRecovery>
	{
		public PasswordRecoveryMap()
		{
			this.ToTable("PasswordRecovery");

			this.HasKey(t => t.ID);

			this.Property(t => t.ID).HasColumnName("id");
			this.Property(t => t.Token).HasColumnName("token");
			this.Property(t => t.Username).HasColumnName("username");
			this.Property(t => t.TimeStamp).HasColumnName("timeStamp");
			this.Property(t => t.CreatedByIP).HasColumnName("createdbyIpAddress");
			this.Property(t => t.UsedByIP).HasColumnName("usedByIpAddress");
		}
	}
}