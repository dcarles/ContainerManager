using AutoFixture;
using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Handlers;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContainerManager.UnitTests.Handlers
{
	public class UpdateApplicationHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IApplicationRepository> _repoMock;
		private readonly Mock<IMachineRepository> _machineRepoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Application _application;

		public UpdateApplicationHandlerTests()
		{
			_fixture = new Fixture();
			_application = _fixture.Create<Application>();
			_mapperMock = new Mock<IMapper>();
			_mapperMock.Setup(mapper => mapper.Map<Application>(It.IsAny<UpdateApplicationCommand>()))
			  .Returns(_application);
			_repoMock = new Mock<IApplicationRepository>();
			_machineRepoMock = new Mock<IMachineRepository>();
		}

		[Fact]
		public async Task HandleAsync_ExistingMachine_UpdateAsyncCalled_ReturnsApplication()
		{
			// Arrange	
			var command = _fixture.Create<UpdateApplicationCommand>();
			var machine = _fixture.Create<Machine>();
			_machineRepoMock.Setup(m => m.GetByIdAsync(command.MachineId)).ReturnsAsync(machine);

			var handler = new UpdateApplicationHandler(_repoMock.Object, _machineRepoMock.Object, _mapperMock.Object);

			//Act
			var application = await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.UpdateAsync(_application), Times.Once);

		}

		[Fact]
		public async Task HandleAsync_InvalidMachine_UpdateAsyncNotCalled_ThrowsRecordNotFoundException()
		{
			// Arrange	
			var command = _fixture.Create<UpdateApplicationCommand>();
			_machineRepoMock.Setup(m => m.GetByIdAsync(command.MachineId)).ReturnsAsync((Machine)null);

			var handler = new UpdateApplicationHandler(_repoMock.Object, _machineRepoMock.Object, _mapperMock.Object);

			//Act
			var result = new Func<Task>(async () => await handler.Handle(command, new CancellationToken()));

			//Assert
			await result.Should().ThrowAsync<RecordNotFoundException>();
			_repoMock.Verify(r => r.UpdateAsync(_application), Times.Never);
		}
	}
}
