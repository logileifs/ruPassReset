using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RUPassReset.Service.DBModels;

namespace RUPassReset.Service.Repositories
{
	public class MyschoolDataContext : DbContext
	{
		public MyschoolDataContext() : base("name=MyschoolDataContext")
		{
			Database.SetInitializer<MyschoolDataContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Configurations.Add(new UserMap());
			modelBuilder.Configurations.Add(new PersonMap());
		}

		// Users table
		public DbSet<User> Users { get; set; }
		// Folk table
		public DbSet<Person> Persons { get; set; }
	}
}