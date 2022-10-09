using System;
using System.Collections.Generic;
using static ContainerManager.Domain.Models.Application;

namespace ContainerManager.Infrastructure.Entities
{
	public class Application : BaseEntity
	{
		public string Name { get; set; }
		public int Port { get; set; }
		public string Image { get; set; }
		public EntryPointSpecification EntryPoint { get; set; }
		public ApplicationState State { get; set; }
		public List<EnvironmentVariable> EnvironmentVariables { get; set; }
		public ResourceLimitSpecification ResourceLimits { get; set; }
		public Guid OwnerId { get; set; }
		public Guid? MachineId { get; set; }

	}
}
