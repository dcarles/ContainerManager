using System;

namespace ContainerManager.API.ViewModels
{
	public class ApplicationRequest
	{
		public string Name { get; set; }
		public int? Port { get; set; }
		public string Image { get; set; }
		public string Command { get; set; }
		public string? Args { get; set; }
		public string WorkingDirectory { get; set; }
		public int? CPULimit { get; set; }
		public int? MemoryMBLimit { get; set; }
		public Guid? MachineId { get; set; }
	}

}
