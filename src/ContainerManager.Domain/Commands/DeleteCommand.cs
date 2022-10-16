using MediatR;

namespace ContainerManager.Domain.Commands
{
	public class DeleteCommand<T> : IRequest<Unit>
	{
		public DeleteCommand(Guid id, Guid authUserId)
		{
			Id = id;
			AuthUserId = authUserId;
		}

		public readonly Guid Id;

		public readonly Guid AuthUserId;
	}
}
