using ContainerManager.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ContainerManager.UnitTests.Controllers
{
	internal static class ClaimsMockHelper
	{

		internal static ControllerContext GetControllerContext(UserRole role, Guid? userId = null)
		{
			//  Context Mock
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, userId != null ? userId.ToString() : Guid.NewGuid().ToString())
			};
			claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
			var identity = new ClaimsIdentity(claims, "ApiKey");
			var identities = new List<ClaimsIdentity> { identity };
			var principal = new ClaimsPrincipal(identities);
			var context = new ControllerContext
			{
				HttpContext = new DefaultHttpContext() { User = principal }
			};

			return context;
		}
	}
}
