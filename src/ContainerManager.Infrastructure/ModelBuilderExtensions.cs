using ContainerManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerManager.Infrastructure
{
	public static class ModelBuilderExtensions
	{
		public static void Seed(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = new Guid("a6de3afd-5c74-407b-8a19-75c37027e610"),
					ApiKey = "testOwnerApiKey3264",
					Email = "danielcarles@gmail.com",
					FirstName = "Daniel",
					LastName = "Carles",
					Role = Domain.Models.UserRole.ApiOwner
				}, new User
				{
					Id = new Guid("9a484f14-3234-440d-bf99-3fcf2adeaf95"),
					ApiKey = "testConsumerApiKey3264",
					Email = "danielcarles-consumer@gmail.com",
					FirstName = "Daniel",
					LastName = "Carles",
					Role = Domain.Models.UserRole.Consumer
				}
			);

		}
	}
}
