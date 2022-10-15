using AutoFixture;
using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Handlers;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContainerManager.UnitTests.Handlers
{
	public class GetUserByIdHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IUserRepository> _repoMock;		

		public GetUserByIdHandlerTests()
		{			
			_fixture = new Fixture();			
			_repoMock = new Mock<IUserRepository>();
		}

		[Fact]
		public async Task HandleAsync_GetByIdAsyncCalled_ReturnsSuccessfully()
		{
			// Arrange	
			var command = _fixture.Create<GetByIdQuery<User>>();
			var handler = new GetUserByIdHandler(_repoMock.Object);

			//Act
			await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);

		}
	}
}
