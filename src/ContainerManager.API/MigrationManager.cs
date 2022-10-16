using ContainerManager.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerManager.API
{
	public static class MigrationManager
	{
		public static IWebHost MigrateDatabase(this IWebHost webHost)
		{
			using (var scope = webHost.Services.CreateScope())
			{
				using (var appContext = scope.ServiceProvider.GetRequiredService<ContainerManagerDbContext>())
				{
					appContext.Database.Migrate();
				}
			}

			return webHost;
		}
	}
}
