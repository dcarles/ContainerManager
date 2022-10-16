using ContainerManager.Domain.Models;
using ContainerManager.Domain.Services;
using MediatR;

namespace ContainerManager.Domain.Commands
{
	public class CreateUserCommand : IRequest<User>
	{
		public CreateUserCommand()
		{
			Id = Guid.NewGuid();
			ApiKey = ApiKeyGenerator.GenerateApiKey();
		}

		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public readonly Guid Id;

		public readonly string ApiKey;

	}
}
