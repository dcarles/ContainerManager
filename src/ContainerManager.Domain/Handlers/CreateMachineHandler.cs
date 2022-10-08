using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class CreateMachineHandler : IRequestHandler<CreateMachineCommand, Machine>
	{
		public Task<Machine> Handle(CreateMachineCommand request, CancellationToken cancellationToken)
		{
			var machine = new Machine();
			machine.Name = request.Name;
			machine.Id = request.Id;
			machine.OS = request.OS;

			return Task.FromResult(machine);
		}
	}
}
