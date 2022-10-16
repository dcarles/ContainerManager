
using ContainerManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

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


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			 => optionsBuilder
		.UseLazyLoadingProxies();


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Application>()
		   .HasOne(s => s.Machine)
		   .WithMany()
		   .HasForeignKey(a => a.MachineId).OnDelete(DeleteBehavior.SetNull);

			// Ensure that API Keys are Unique
			modelBuilder.Entity<User>()
				.HasIndex(u => u.ApiKey)
				.IsUnique();

			modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

			modelBuilder.Entity<User>()
			.Property(e => e.Role)
			.HasConversion(
				v => v.ToString(),
				v => (Domain.Models.UserRole)Enum.Parse(typeof(Domain.Models.UserRole), v));

			modelBuilder.Entity<Machine>()
			.HasIndex(m => m.Name)
			.IsUnique();

			modelBuilder.Entity<Machine>()
			.Property(e => e.OS)
			.HasConversion(
				v => v.ToString(),
				v => (Domain.Models.OSType)Enum.Parse(typeof(Domain.Models.OSType), v));

			modelBuilder.Entity<Application>()
			.HasIndex(a => a.Name)
			.IsUnique();

			modelBuilder.Entity<Application>()
			.Property(e => e.State)
			.HasConversion(
				v => v.ToString(),
				v => (Domain.Models.ApplicationState)Enum.Parse(typeof(Domain.Models.ApplicationState), v));

			// seed the database
			modelBuilder.Seed();
		}

		public DbSet<Machine> Machines { get; set; }
		public DbSet<Application> Applications { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
