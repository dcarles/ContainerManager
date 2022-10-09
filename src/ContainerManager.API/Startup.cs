using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Handlers;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure.Entities;
using ContainerManager.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ContainerManager.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
			.AddMvcCore()
			.AddAuthorization();

			services.AddValidatorsFromAssemblyContaining<Startup>();

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContainerManager.API", Version = "v1" });
			});

			services.AddHealthChecks();

			services.AddMediatR(typeof(CreateUserHandler).Assembly);
			services.AddAutoMapper(typeof(ApiMappingProfile), typeof(DataMappingProfile), typeof(CommandMappingProfile));

			services.AddSingleton<IUserRepository, UserRepository>();
			services.AddSingleton<IApplicationRepository, ApplicationRepository>();
			services.AddSingleton<IMachineRepository, MachineRepository>();

			services.AddHttpClient();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContainerManager.API v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
