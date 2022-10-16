using ContainerManager.Domain.Models;

namespace ContainerManager.API.ViewModels
{
	public class MachineRequest
	{
		public string? Name { get; set; }
		public OSType? OS { get; set; }
	}
}
