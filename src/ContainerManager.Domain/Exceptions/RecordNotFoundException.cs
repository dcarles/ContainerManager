namespace ContainerManager.Domain.Exceptions
{
	public class RecordNotFoundException : Exception
	{
		public RecordNotFoundException(string? message = null, Exception? inner = null) : base(message, inner)
		{
		}
	}
}
