using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using static ContainerManager.Domain.Models.Application;

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
		public EntryPointSpecification EntryPoint { get; set; }
		public List<EnvironmentVariable> EnvironmentVariables { get; set; }
		public ResourceLimitSpecification ResourceLimits { get; set; }
		public Guid? MachineId { get; set; }

		public readonly Guid Id;

		public Guid OwnerId { get; set; }

	}
}
