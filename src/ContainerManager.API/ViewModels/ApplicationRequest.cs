using System;
using System.Collections.Generic;
using static ContainerManager.Domain.Models.Application;

namespace ContainerManager.API.ViewModels
{
	public class ApplicationRequest
	{
		public string Name { get; set; }
		public int Port { get; set; }
		public string Image { get; set; }
		public EntryPointSpecification EntryPoint { get; set; }				
		public List<EnvironmentVariable> EnvironmentVariables { get; set; }
		public ResourceLimitSpecification ResourceLimits { get; set; }
		public Guid? MachineId { get; set; }
		internal Guid OwnerId { get; set; }
	}

}
