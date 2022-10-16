using AutoFixture;
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
	public class DeleteUserHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IUserRepository> _repoMock;
		private readonly Mock<IMachineRepository> _machineRepoMock;
		private readonly Mock<IApplicationRepository> _applicationRepoMock;

		public DeleteUserHandlerTests()
		{
			_fixture = new Fixture();
			_repoMock = new Mock<IUserRepository>();
			_machineRepoMock = new Mock<IMachineRepository>();
			_applicationRepoMock = new Mock<IApplicationRepository>();
		}

		[Fact]
		public async Task HandleAsync_DeleteAsyncCalled_UpdateOwnershipCalled_ReturnsSuccesfully()
		{
			// Arrange	
			var command = _fixture.Create<DeleteCommand<User>>();
			var handler = new DeleteUserHandler(_repoMock.Object, _applicationRepoMock.Object, _machineRepoMock.Object);

			//Act
			await handler.Handle(command, new CancellationToken());

			//Assert
			_applicationRepoMock.Verify(r => r.UpdateOwnership(command.Id, command.AuthUserId), Times.Once);
			_machineRepoMock.Verify(r => r.UpdateOwnership(command.Id, command.AuthUserId), Times.Once);
			_repoMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);

		}
	}
}
