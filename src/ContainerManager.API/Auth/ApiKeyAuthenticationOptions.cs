using Microsoft.AspNetCore.Authentication;
using System;

namespace ContainerManager.API.Auth
{
	public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
	{
		public const string DefaultScheme = "API Key";
		public string Scheme => DefaultScheme;
		public string AuthenticationType = DefaultScheme;
	}

	public static class AuthenticationBuilderExtensions
	{
		public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
		{
			return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
		}
	}
}
