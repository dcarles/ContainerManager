using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static ContainerManager.Domain.Models.User;

namespace ContainerManager.API.Auth
{	

    public class OnlyApiOwnersAuthorizationHandler : AuthorizationHandler<OnlyApiOwnersRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyApiOwnersRequirement requirement)
        {
            if (context.User.IsInRole(UserRole.ApiOwner.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
