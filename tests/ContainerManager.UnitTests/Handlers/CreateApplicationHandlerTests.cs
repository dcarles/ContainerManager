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
	public class CreateApplicationHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IApplicationRepository> _repoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Application _application;

		public CreateApplicationHandlerTests()
		{			
			_fixture = new Fixture();
			_application = _fixture.Create<Application>();
			_mapperMock = new Mock<IMapper>();
			_mapperMock.Setup(mapper => mapper.Map<Application>(It.IsAny<CreateApplicationCommand>()))
			  .Returns(_application);
			_repoMock = new Mock<IApplicationRepository>();
		}

		[Fact]
		public async Task HandleAsync_AddAsyncCalled_ReturnsApplication()
		{
			// Arrange	
			var command = _fixture.Create<CreateApplicationCommand>();
			var handler = new CreateApplicationHandler(_repoMock.Object, _mapperMock.Object);

			//Act
			var application = await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.AddAsync(_application), Times.Once);

		}
	}
}
