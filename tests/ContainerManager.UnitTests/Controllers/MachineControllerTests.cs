using AutoFixture;
using AutoMapper;
using ContainerManager.API.Controllers;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContainerManager.UnitTests.Controllers
{
	public class MachineControllerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<ILogger<MachineController>> _loggerMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly Mock<IMapper> _mapperMock;

		public MachineControllerTests()
		{
			_fixture = new Fixture();
			_loggerMock = new Mock<ILogger<MachineController>>();
			_mediatorMock = new Mock<IMediator>();
			_mapperMock = new Mock<IMapper>();
		}

		[Fact]
		public async Task Get_UserHasNoMachines_ReturnsNotFoundResponse()
		{
			// Arrange		
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByUserIdQuery<Machine>>(), CancellationToken.None)).ReturnsAsync(new List<Machine>());

			var machineControllerUnderTest = new MachineController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			machineControllerUnderTest.ControllerContext = context;

			// Act
			var machineResponse = await machineControllerUnderTest.Get();

			// Assert
			Assert.IsType<NotFoundObjectResult>(machineResponse);
			Assert.IsType <ErrorResponse>(((NotFoundObjectResult)machineResponse).Value);

			_mediatorMock.Verify(m=>m.Send(It.IsAny<GetByUserIdQuery<Machine>>(), CancellationToken.None),Times.Once);
		}

		[Fact]
		public async Task Get_UserHasMachines_ReturnsOKResponseWithMachines()
		{
			// Arrange		
			var machines = _fixture.CreateMany<Machine>();
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByUserIdQuery<Machine>>(), CancellationToken.None)).ReturnsAsync(machines);

			var machineControllerUnderTest = new MachineController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			machineControllerUnderTest.ControllerContext = context;

			// Act
			var machineResponse = await machineControllerUnderTest.Get();

			// Assert
			Assert.IsType<OkObjectResult>(machineResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByUserIdQuery<Machine>>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Post_ValidMachineRequest_ReturnsOKResponseWithMachine()
		{
			// Arrange		
			var machine = _fixture.Create<Machine>();
			var machineRequest = _fixture.Create<MachineRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateMachineCommand>(It.IsAny<MachineRequest>()))
			  .Returns(new CreateMachineCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateMachineCommand>(), CancellationToken.None)).ReturnsAsync(machine);


			var machineControllerUnderTest = new MachineController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			machineControllerUnderTest.ControllerContext = context;

			// Act
			var machineResponse = await machineControllerUnderTest.Post(machineRequest);

			// Assert
			Assert.IsType<CreatedResult>(machineResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateMachineCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Post_ExistingMachineRequest_ReturnsConflictResponse()
		{
			// Arrange				
			var machineRequest = _fixture.Create<MachineRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateMachineCommand>(It.IsAny<MachineRequest>()))
			  .Returns(new CreateMachineCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateMachineCommand>(), CancellationToken.None)).ThrowsAsync(new RecordAlreadyExistsException());

			var machineControllerUnderTest = new MachineController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			machineControllerUnderTest.ControllerContext = context;

			// Act
			var machineResponse = await machineControllerUnderTest.Post(machineRequest);

			// Assert
			Assert.IsType<ConflictObjectResult>(machineResponse);
			Assert.IsType<ErrorResponse>(((ConflictObjectResult)machineResponse).Value);

			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateMachineCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Delete_ValidMachineRequest_ReturnsOKResponseWithMachine()
		{
			// Arrange	
			var machineControllerUnderTest = new MachineController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			machineControllerUnderTest.ControllerContext = context;

			// Act
			var machineResponse = await machineControllerUnderTest.Delete(Guid.NewGuid());

			// Assert
			Assert.IsType<OkResult>(machineResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<DeleteCommand<Machine>>(), CancellationToken.None), Times.Once);
		}

	}
}
