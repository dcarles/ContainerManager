using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class DeleteUserHandler : IRequestHandler<DeleteCommand<User>, Unit>
	{
		private readonly IUserRepository _repo;

		public DeleteUserHandler(IUserRepository repo)
		{
			_repo = repo;
		}

		public async Task<Unit> Handle(DeleteCommand<User> request, CancellationToken cancellationToken)
		{		
			await _repo.DeleteAsync(request.Id);
			return Unit.Value;
		}
	}
}
