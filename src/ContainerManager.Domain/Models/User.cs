namespace ContainerManager.Domain.Models
{
	public class User : BaseModel
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ApiKey { get; set; }
		public UserRole Role { get; set; }


		public enum UserRole
		{
			Consumer,
			ApiOwner
		}
	}
}
