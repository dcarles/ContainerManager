using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;

namespace ContainerManager.Domain.Handlers
{
	public class CreateMachineHandler : IRequestHandler<CreateMachineCommand, Machine>
	{
		private readonly IMachineRepository _repo;
		private readonly IMapper _mapper;

		public CreateMachineHandler(IMachineRepository repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<Machine> Handle(CreateMachineCommand request, CancellationToken cancellationToken)
		{
			var machine = _mapper.Map<Machine>(request);
			await _repo.AddAsync(machine);
			return machine;
		}
	}
}
