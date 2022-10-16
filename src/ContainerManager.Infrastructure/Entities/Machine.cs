using ContainerManager.Domain.Models;

namespace ContainerManager.Infrastructure.Entities
{
	public class Machine : BaseEntity
	{
		public string Name { get; set; }
		public OSType OS { get; set; }
		public Guid OwnerId { get; set; }
	}
}
