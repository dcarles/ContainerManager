using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Entities
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ApiKey { get; set; }
	}
}
