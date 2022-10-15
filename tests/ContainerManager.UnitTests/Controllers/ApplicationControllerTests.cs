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
	public class ApplicationControllerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<ILogger<ApplicationController>> _loggerMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly Mock<IMapper> _mapperMock;

		public ApplicationControllerTests()
		{
			_fixture = new Fixture();
			_loggerMock = new Mock<ILogger<ApplicationController>>();
			_mediatorMock = new Mock<IMediator>();
			_mapperMock = new Mock<IMapper>();
		}

		[Fact]
		public async Task Get_UserHasNoApplications_ReturnsNotFoundResponse()
		{
			// Arrange		
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByUserIdQuery<Application>>(), CancellationToken.None)).ReturnsAsync(new List<Application>());

			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Get();

			// Assert
			Assert.IsType<NotFoundObjectResult>(applicationResponse);
			Assert.IsType<ErrorResponse>(((NotFoundObjectResult)applicationResponse).Value);

			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByUserIdQuery<Application>>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Get_UserHasApplications_ReturnsOKResponseWithApplications()
		{
			// Arrange		
			var Applications = _fixture.CreateMany<Application>();
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByUserIdQuery<Application>>(), CancellationToken.None)).ReturnsAsync(Applications);

			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Get();

			// Assert
			Assert.IsType<OkObjectResult>(applicationResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByUserIdQuery<Application>>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Post_ValidApplicationRequest_ReturnsOKResponseWithApplication()
		{
			// Arrange		
			var application = _fixture.Create<Application>();
			var applicationRequest = _fixture.Create<ApplicationRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateApplicationCommand>(It.IsAny<ApplicationRequest>()))
			  .Returns(new CreateApplicationCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateApplicationCommand>(), CancellationToken.None)).ReturnsAsync(application);


			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Post(applicationRequest);

			// Assert
			Assert.IsType<CreatedResult>(applicationResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateApplicationCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Post_ExistingApplicationRequest_ReturnsConflictResponse()
		{
			// Arrange				
			var applicationRequest = _fixture.Create<ApplicationRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateApplicationCommand>(It.IsAny<ApplicationRequest>()))
			  .Returns(new CreateApplicationCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateApplicationCommand>(), CancellationToken.None)).ThrowsAsync(new RecordAlreadyExistsException());

			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Post(applicationRequest);

			// Assert
			Assert.IsType<ConflictObjectResult>(applicationResponse);
			Assert.IsType<ErrorResponse>(((ConflictObjectResult)applicationResponse).Value);

			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateApplicationCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Delete_ValidApplicationRequest_ReturnsOKResponseWithApplication()
		{
			// Arrange	
			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Delete(Guid.NewGuid());

			// Assert
			Assert.IsType<OkResult>(applicationResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<DeleteCommand<Application>>(), CancellationToken.None), Times.Once);
		}


		[Fact]
		public async Task Patch_ValidExistingApplicationRequest_ReturnsOKResponseWithApplication()
		{
			// Arrange		
			var application = _fixture.Create<Application>();
			var applicationRequest = _fixture.Create<ApplicationRequest>();
			_mapperMock.Setup(mapper => mapper.Map<UpdateApplicationCommand>(It.IsAny<ApplicationRequest>()))
			  .Returns(new UpdateApplicationCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<UpdateApplicationCommand>(), CancellationToken.None)).ReturnsAsync(application);


			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Patch(Guid.NewGuid(), applicationRequest);

			// Assert
			Assert.IsType<OkObjectResult>(applicationResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<UpdateApplicationCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Patch_NotExistingApplicationRequest_ReturnsNotFoundResponse()
		{
			// Arrange				
			var applicationRequest = _fixture.Create<ApplicationRequest>();
			_mapperMock.Setup(mapper => mapper.Map<UpdateApplicationCommand>(It.IsAny<ApplicationRequest>()))
			  .Returns(new UpdateApplicationCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<UpdateApplicationCommand>(), CancellationToken.None)).ThrowsAsync(new RecordNotFoundException());

			var applicationController = new ApplicationController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			applicationController.ControllerContext = context;

			// Act
			var applicationResponse = await applicationController.Patch(Guid.NewGuid(), applicationRequest);

			// Assert
			Assert.IsType<NotFoundObjectResult>(applicationResponse);
			Assert.IsType<ErrorResponse>(((NotFoundObjectResult)applicationResponse).Value);

			_mediatorMock.Verify(m => m.Send(It.IsAny<UpdateApplicationCommand>(), CancellationToken.None), Times.Once);
		}

	}
}
