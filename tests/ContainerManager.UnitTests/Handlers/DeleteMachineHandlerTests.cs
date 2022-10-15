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
	public class DeleteMachineHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IMachineRepository> _repoMock;		

		public DeleteMachineHandlerTests()
		{			
			_fixture = new Fixture();			
			_repoMock = new Mock<IMachineRepository>();
		}

		[Fact]
		public async Task HandleAsync_DeleteAsyncCalled_ReturnsSuccesfully()
		{
			// Arrange	
			var command = _fixture.Create<DeleteCommand<Machine>>();
			var handler = new DeleteMachineHandler(_repoMock.Object);

			//Act
			await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);

		}
	}
}
