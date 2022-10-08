using System;
using System.Security.Cryptography;

namespace ContainerManager.Domain.Services
{
	public static class ApiKeyGenerator
	{
		public static string GenerateApiKey()
		{
			var key = new byte[32];
			using (var generator = RandomNumberGenerator.Create())
				generator.GetBytes(key);
			return Convert.ToBase64String(key);
		}
	}
}
