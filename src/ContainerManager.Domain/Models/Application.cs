using System;
using System.Collections.Generic;

namespace ContainerManager.Domain.Models
{
	public class Application : BaseModel
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
		public User Owner { get; set; }
		public Machine Machine { get; set; }


		public enum ApplicationState
		{
			Created,
			Running,
			Removing,
			Exited
		}

	}


}
