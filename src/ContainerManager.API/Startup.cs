using ContainerManager.API.Auth;
using ContainerManager.API.Helpers;
using ContainerManager.API.Middlewares;
using ContainerManager.API.Validation;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Handlers;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure;
using ContainerManager.Infrastructure.Entities;
using ContainerManager.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

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
			services.AddMvc();

			services.AddDbContext<ContainerManagerDbContext>(c =>
			  c.UseSqlServer(Configuration.GetConnectionString("ContainersManagerDBConnectionString")));

			services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
				options.JsonSerializerOptions.Converters.Add(new JsonStringConverter());
			});

			services.AddFluentValidationAutoValidation(config =>
			{
				config.DisableDataAnnotationsValidation = true;
				config.ImplicitlyValidateChildProperties = true;
			});

			services.AddValidatorsFromAssemblyContaining<ApplicationRequestValidator>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = $"ContainerManager.API {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}", Version = "v1" });

				c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
				{
					Type = SecuritySchemeType.ApiKey,
					In = ParameterLocation.Header,
					Name = "X-Api-Key",
					Description = "Basic Authorization using unique ApiKey per user",
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference {
								Type = ReferenceType.SecurityScheme,
								Id = "ApiKey"
							}
						},
						Array.Empty<string>()
					}
				});				

				c.DocInclusionPredicate((name, api) => true);
				c.UseInlineDefinitionsForEnums();
				c.TagActionsBy(api => new List<string> { api.GroupName });
				var xmlFile = $"ContainerManager.API.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath, true);
			
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


			//Automatic Handling of validation errors on Api request models
			services.Configure((Action<ApiBehaviorOptions>)(options =>
				options.InvalidModelStateResponseFactory = c =>
				{
					var errors = c.ModelState.Where(v => v.Value?.Errors.Count > 0)
						.Select(v => new ErrorResponseItem { Field = v.Key, Errors = v.Value?.Errors.Select(e => e.ErrorMessage) })
						.ToList();

					return new BadRequestObjectResult(new ErrorResponse
					{
						StatusCode = 400,
						Message = "One or more Validation errors Occurred",
						Errors = errors
					});
				}));


			services.AddHttpClient();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();

				app.UseSwaggerUI(c =>
				{
					c.DisplayRequestDuration();
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContainerManager.API v1");
				});

			}

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseMiddleware<ExceptionMiddleware>();

			app.Map("/healthcheck", s =>
			{
				s.UseRouting();
				s.UseEndpoints(endpoints => endpoints.MapHealthChecks("/", new HealthCheckOptions()
				{
					Predicate = _ => true,
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
				}));
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
