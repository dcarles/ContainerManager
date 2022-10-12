namespace ContainerManager.Domain.Exceptions
{
	public class RecordAlreadyExistsException : Exception
	{
		public RecordAlreadyExistsException(string? message = null, Exception? inner = null) : base(message, inner)
		{
		}
	}
}
