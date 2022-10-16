using MediatR;

namespace ContainerManager.Domain.Commands
{
	public class GetByIdQuery<T> : IRequest<T>
	{
		public GetByIdQuery(Guid id)
		{
			Id = id;
		}

		public readonly Guid Id;

	}
}
