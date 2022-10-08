using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
	{
		public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			var user = new User();
			user.Email = request.Email;
			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.Id = request.Id;
			user.ApiKey = request.ApiKey;

			

			return Task.FromResult(user);
		}
	}
}
