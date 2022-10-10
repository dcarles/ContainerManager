using ContainerManager.API.Auth;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Handlers;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure;
using ContainerManager.Infrastructure.Entities;
using ContainerManager.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

			services.AddHealthChecks();

			services.AddDbContext<ContainerManagerDbContext>(c =>
			  c.UseSqlServer(Configuration.GetConnectionString("ContainersManagerDBConnectionString")));
	

			services.AddValidatorsFromAssemblyContaining<Startup>();

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContainerManager.API", Version = "v1" });
			});

			services.AddHealthChecks();

			services.AddMediatR(typeof(CreateUserHandler).Assembly);
			services.AddAutoMapper(typeof(ApiMappingProfile), typeof(DataMappingProfile), typeof(CommandMappingProfile));

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<IMachineRepository, MachineRepository>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
				options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
			})
			.AddApiKeySupport(options => { });

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policies.OnlyApiOwners, policy => policy.Requirements.Add(new OnlyApiOwnersRequirement()));				
			});

			services.AddSingleton<IAuthorizationHandler, OnlyApiOwnersAuthorizationHandler>();

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

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
