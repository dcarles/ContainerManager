using ContainerManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure
{
	public class ContainerManagerDbContext : DbContext
	{

		public ContainerManagerDbContext() : base()
		{

		}

		public ContainerManagerDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Ensure that API Keys are Unique
			modelBuilder.Entity<User>()
				.HasIndex(u => u.ApiKey)
				.IsUnique();

			modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

			modelBuilder.Entity<Machine>()
			.HasIndex(m => m.Name)
			.IsUnique();

			modelBuilder.Entity<Application>()
			.HasIndex(a => a.Name)
			.IsUnique();

			// seed the database
			modelBuilder.Seed();
		}

		public DbSet<Machine> Machines { get; set; }
		public DbSet<Application> Applications { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
