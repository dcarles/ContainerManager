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
	public class GetMachinesByUserIdHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IMachineRepository> _repoMock;		

		public GetMachinesByUserIdHandlerTests()
		{			
			_fixture = new Fixture();			
			_repoMock = new Mock<IMachineRepository>();
		}

		[Fact]
		public async Task HandleAsync_GetByOwnerCalled_ReturnsSuccessfully()
		{
			// Arrange	
			var command = _fixture.Create<GetByUserIdQuery<Machine>>();
			var handler = new GetMachinesByUserIdHandler(_repoMock.Object);

			//Act
			await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.GetByOwner(command.Id), Times.Once);

		}
	}
}
