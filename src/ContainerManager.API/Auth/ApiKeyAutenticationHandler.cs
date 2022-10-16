using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ContainerManager.API.Auth
{
	public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
	{
		private const string ProblemDetailsContentType = "application/problem+json";
		private readonly IUserRepository _repo;
		private const string ApiKeyHeaderName = "X-Api-Key";

		public ApiKeyAuthenticationHandler(
			IOptionsMonitor<ApiKeyAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IUserRepository repo) : base(options, logger, encoder, clock)
		{
			_repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
			{
				return AuthenticateResult.NoResult();
			}

			var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

			if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
			{
				return AuthenticateResult.NoResult();
			}

			var existingApiKey = await _repo.GetByApiKey(providedApiKey);

			if (existingApiKey != null)
			{
				var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, existingApiKey.Id.ToString())
			};

				claims.Add(new Claim(ClaimTypes.Role, existingApiKey.Role.ToString()));

				var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
				var identities = new List<ClaimsIdentity> { identity };
				var principal = new ClaimsPrincipal(identities);
				var ticket = new AuthenticationTicket(principal, Options.Scheme);

				return AuthenticateResult.Success(ticket);
			}

			return AuthenticateResult.NoResult();
		}

		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			Response.ContentType = ProblemDetailsContentType;

			await Response.WriteAsync(new ErrorResponse
			{
				StatusCode = Response.StatusCode,
				Message = "Unathorized. Invalid/Missing API Key"
			}.ToString());

		}

		protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
		{
			Response.StatusCode = (int)HttpStatusCode.Forbidden;
			Response.ContentType = ProblemDetailsContentType;

			await Response.WriteAsync(new ErrorResponse
			{
				StatusCode = Response.StatusCode,
				Message = "Access denied. Not enough privileges"
			}.ToString());
		}
	}
}
