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
	public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
	{
		private readonly IUserRepository _repo;

		public GetUserByIdHandler(IUserRepository repo)
		{
			_repo = repo;
		}

		public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{		
			return await _repo.GetByIdAsync(request.Id);		
		}
	}
}
