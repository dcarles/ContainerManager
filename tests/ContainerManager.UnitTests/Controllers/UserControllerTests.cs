using AutoFixture;
using AutoMapper;
using ContainerManager.API.Controllers;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
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
	public class UserControllerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<ILogger<UserController>> _loggerMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly Mock<IMapper> _mapperMock;

		public UserControllerTests()
		{
			_fixture = new Fixture();
			_loggerMock = new Mock<ILogger<UserController>>();
			_mediatorMock = new Mock<IMediator>();
			_mapperMock = new Mock<IMapper>();
		}

		[Fact]
		public async Task Get_InvalidUser_ReturnsNotFoundResponse()
		{
			// Arrange		
			var userId = Guid.NewGuid();
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None)).ReturnsAsync((User)null);
			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer, userId);
			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Get(userId);

			// Assert
			Assert.IsType<NotFoundObjectResult>(userResponse);
			Assert.IsType <ErrorResponse>(((NotFoundObjectResult)userResponse).Value);

			_mediatorMock.Verify(m=>m.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None),Times.Once);
		}

		[Fact]
		public async Task Get_OtherUser_ConsumerRole_ReturnsForbiddenResponse()
		{
			// Arrange		
			var authUserId = Guid.NewGuid();
			var otherUserId = Guid.NewGuid();
			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer, authUserId);
			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Get(otherUserId);

			// Assert
			Assert.IsType<ForbidResult>(userResponse);			

			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None), Times.Never);
		}

		[Fact]
		public async Task Get_OtherUser_ApiOwner_ReturnsOKResponseWithUser()
		{
			// Arrange		
			var authUserId = Guid.NewGuid();
			var otherUserId = Guid.NewGuid();
			var user = _fixture.Create<User>();
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None)).ReturnsAsync(user);
			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.ApiOwner, authUserId);
			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Get(otherUserId);

			// Assert
			Assert.IsType<OkObjectResult>(userResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None), Times.Once);
		}


		[Fact]
		public async Task Get_ExistingUser_ReturnsOKResponseWithUser()
		{
			// Arrange		
			var userId = Guid.NewGuid();
			var user = _fixture.Create<User>();
			_mediatorMock.Setup(med => med.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None)).ReturnsAsync(user);
			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer, userId);
			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Get(userId);

			// Assert
			Assert.IsType<OkObjectResult>(userResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<GetByIdQuery<User>>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Register_ValidUserRequest_ReturnsOKResponseWithUser()
		{
			// Arrange		
			var user = _fixture.Create<User>();
			var userRequest = _fixture.Create<UserRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateUserCommand>(It.IsAny<UserRequest>()))
			  .Returns(new CreateUserCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None)).ReturnsAsync(user);

			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			
			var context = new ControllerContext
			{
				HttpContext = new DefaultHttpContext() {  }
			};

			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Register(userRequest);

			// Assert
			Assert.IsType<CreatedResult>(userResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Register_ExistingUserRequest_ReturnsConflictResponse()
		{
			// Arrange				
			var userRequest = _fixture.Create<UserRequest>();
			_mapperMock.Setup(mapper => mapper.Map<CreateUserCommand>(It.IsAny<UserRequest>()))
			  .Returns(new CreateUserCommand());
			_mediatorMock.Setup(med => med.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None)).ThrowsAsync(new RecordAlreadyExistsException());

			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
		
			// Act
			var userResponse = await userControllerUnderTest.Register(userRequest);

			// Assert
			Assert.IsType<ConflictObjectResult>(userResponse);
			Assert.IsType<ErrorResponse>(((ConflictObjectResult)userResponse).Value);

			_mediatorMock.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None), Times.Once);
		}

		[Fact]
		public async Task Delete_ValidUserRequest_ReturnsOKResponseWithUser()
		{
			// Arrange	
			var userControllerUnderTest = new UserController(_loggerMock.Object, _mediatorMock.Object, _mapperMock.Object);
			var context = ClaimsMockHelper.GetControllerContext(UserRole.Consumer);
			userControllerUnderTest.ControllerContext = context;

			// Act
			var userResponse = await userControllerUnderTest.Delete(Guid.NewGuid());

			// Assert
			Assert.IsType<OkResult>(userResponse);
			_mediatorMock.Verify(m => m.Send(It.IsAny<DeleteCommand<User>>(), CancellationToken.None), Times.Once);
		}

	}
}
