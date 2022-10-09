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
	public class GetMachineByIdHandler : IRequestHandler<GetMachineByIdQuery, Machine>
	{
		private readonly IMachineRepository _repo;	

		public GetMachineByIdHandler(IMachineRepository repo)
		{
			_repo = repo;			
		}

		public async Task<Machine> Handle(GetMachineByIdQuery request, CancellationToken cancellationToken)
		{
			return await _repo.GetByIdAsync(request.Id);
		}
	}
}
