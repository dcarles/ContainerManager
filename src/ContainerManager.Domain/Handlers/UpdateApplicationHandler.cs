using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class UpdateApplicationHandler : IRequestHandler<UpdateApplicationCommand, Application>
	{
		private readonly IApplicationRepository _repo;
		private readonly IMachineRepository _machineRepo;
		private readonly IMapper _mapper;

		public UpdateApplicationHandler(IApplicationRepository repo, IMachineRepository machineRepo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
			_machineRepo = machineRepo;
		}

		public async Task<Application> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
		{
			var machine = await _machineRepo.GetByIdAsync(request.MachineId);

			if(machine == null)
				throw new RecordNotFoundException($"Machine with Id '{request.MachineId}' does not exists");

			var application = _mapper.Map<Application>(request);
			application.Machine = machine;
			await _repo.UpdateAsync(application);
			return application;
		}
	}
}
