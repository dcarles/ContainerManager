using Microsoft.AspNetCore.Authorization;

namespace ContainerManager.API.Auth
{
	public class OnlyApiOwnersRequirement : IAuthorizationRequirement
	{
		// This is empty, but you can have a bunch of properties and methods here if you like that you can later access from the AuthorizationHandler.
	}
}
