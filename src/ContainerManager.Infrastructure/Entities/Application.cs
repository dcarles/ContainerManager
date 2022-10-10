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
		public string Command { get; set; }
		public string Args { get; set; }
		public string WorkingDirectory { get; set; }
		public ApplicationState State { get; set; }
		public int CPULimit { get; set; }
		public int MemoryMBLimit { get; set; }
		public Guid OwnerId { get; set; }
		public Guid MachineId { get; set; }
		public virtual User Owner { get; set; }
		public virtual Machine Machine { get; set; }

	}
}
