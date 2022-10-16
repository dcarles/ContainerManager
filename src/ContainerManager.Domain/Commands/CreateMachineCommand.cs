using ContainerManager.Domain.Models;
using MediatR;

namespace ContainerManager.Domain.Commands
{
	public class CreateMachineCommand : IRequest<Machine>
	{
		public CreateMachineCommand()
		{
			Id = Guid.NewGuid();
		}

		public string Name { get; set; }
		public OSType OS { get; set; }

		public readonly Guid Id;
		public Guid OwnerId { get; set; }

	}
}
