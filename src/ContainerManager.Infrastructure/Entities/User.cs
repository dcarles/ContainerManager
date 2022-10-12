using ContainerManager.Domain.Models;

namespace ContainerManager.Infrastructure.Entities
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ApiKey { get; set; }
		public UserRole Role { get; set; }
	}
}
