using ContainerManager.Domain.Models;
using MediatR;

namespace ContainerManager.Domain.Commands
{
	public class CreateApplicationCommand : IRequest<Application>
	{
		public CreateApplicationCommand()
		{
			Id = Guid.NewGuid();
		}

		public string Name { get; set; }
		public int Port { get; set; }
		public string Image { get; set; }
		public string Command { get; set; }
		public string Args { get; set; }
		public string WorkingDirectory { get; set; }
		public int CPULimit { get; set; }
		public int MemoryMBLimit { get; set; }
		public Guid OwnerId { get; set; }

		public readonly Guid Id;


	}
}
