using System;
using System.Collections.Generic;

namespace ContainerManager.Domain.Models
{
	public class Application : BaseModel
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

		public class ResourceLimitSpecification
		{
			public int CPU { get; set; }
			public int MemoryMB { get; set; }
		}

		public class EntryPointSpecification
		{
			public string Command { get; set; }
			public string Args { get; set; }
			public string WorkingDirectory { get; set; }
		}

		public class EnvironmentVariable
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}

		public enum ApplicationState
		{
			Created,
			Running,
			Removing,
			Exited
		}

	}


}
