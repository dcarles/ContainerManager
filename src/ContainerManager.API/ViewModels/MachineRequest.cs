using ContainerManager.Domain.Models;
using System;

namespace ContainerManager.API.ViewModels
{
	public class MachineRequest
	{
		public string Name { get; set; }
		public OSType OS { get; set; }		
	}
}
