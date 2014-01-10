using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace RUPassReset.Service.DBModels
{
	public class PersonMap : EntityTypeConfiguration<Person>
	{
		public PersonMap()
		{
			this.ToTable("Folk");

			this.HasKey(t => t.ID);

			this.Property(t => t.ID).HasColumnName("ID_Customer");
			this.Property(t => t.SSN).HasColumnName("kennitala");
			this.Property(t => t.Name).HasColumnName("nafn");
			this.Property(t => t.Email).HasColumnName("sEmail");
		}
	}
}