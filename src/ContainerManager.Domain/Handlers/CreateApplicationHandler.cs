using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommand, Application>
	{
		public Task<Application> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
		{
			var application = new Application();
			application.Id = request.Id;
			application.Name = request.Name;
			application.EntryPoint = request.EntryPoint;
			application.EnvironmentVariables = request.EnvironmentVariables;
			application.Image = request.Image;
			application.MachineId = request.MachineId;

			return Task.FromResult(application);
		}
	}
}
