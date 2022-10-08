using ContainerManager.Domain.Models;
using ContainerManager.Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

		internal readonly Guid Id;

		internal readonly string ApiKey;

	}
}
