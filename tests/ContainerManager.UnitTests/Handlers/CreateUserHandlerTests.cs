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
	public class CreateUserHandlerTests
	{
		private readonly IFixture _fixture;
		private readonly Mock<IUserRepository> _repoMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly User _user;

		public CreateUserHandlerTests()
		{			
			_fixture = new Fixture();
			_user = _fixture.Create<User>();
			_mapperMock = new Mock<IMapper>();
			_mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<CreateUserCommand>()))
			  .Returns(_user);
			_repoMock = new Mock<IUserRepository>();
		}

		[Fact]
		public async Task HandleAsync_AddAsyncCalled_ReturnsApplication()
		{
			// Arrange	
			var command = _fixture.Create<CreateUserCommand>();
			var handler = new CreateUserHandler(_repoMock.Object, _mapperMock.Object);

			//Act
			var user = await handler.Handle(command, new CancellationToken());

			//Assert
			_repoMock.Verify(r => r.AddAsync(_user), Times.Once);

		}
	}
}
