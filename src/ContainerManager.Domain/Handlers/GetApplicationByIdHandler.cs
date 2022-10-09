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
	public class GetApplicationByIdHandler : IRequestHandler<GetApplicationByIdQuery, Application>
	{
		private readonly IApplicationRepository _repo;

		public GetApplicationByIdHandler(IApplicationRepository repo)
		{
			_repo = repo;
		}

		public async Task<Application> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
		{
			return await _repo.GetByIdAsync(request.Id);
		}
	}
}
