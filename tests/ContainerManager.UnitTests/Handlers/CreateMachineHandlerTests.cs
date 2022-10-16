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
	public class CreateMachineHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IMachineRepository> _repoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Machine _machine;

		public CreateMachineHandlerTests()
		{
			_fixture = new Fixture();
			_machine = _fixture.Create<Machine>();
			_mapperMock = new Mock<IMapper>();
			_mapperMock.Setup(mapper => mapper.Map<Machine>(It.IsAny<CreateMachineCommand>()))
			  .Returns(_machine);
			_repoMock = new Mock<IMachineRepository>();
		}

		[Fact]
		public async Task HandleAsync_AddAsyncCalled_ReturnsApplication()
		{
			// Arrange	
			var command = _fixture.Create<CreateMachineCommand>();
			var handler = new CreateMachineHandler(_repoMock.Object, _mapperMock.Object);

			//Act
			var machine = await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.AddAsync(_machine), Times.Once);

		}
	}
}
