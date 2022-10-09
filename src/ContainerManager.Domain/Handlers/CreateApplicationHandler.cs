using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommand, Application>
	{
		private readonly IApplicationRepository _repo;
		private readonly IMapper _mapper;

		public CreateApplicationHandler(IApplicationRepository repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<Application> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
		{
			var application = _mapper.Map<Application>(request);
			await _repo.AddAsync(application);
			return application;
		}
	}
}
