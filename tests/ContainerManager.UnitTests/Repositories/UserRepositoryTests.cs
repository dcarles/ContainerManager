using AutoFixture;
using AutoMapper;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using ContainerManager.Infrastructure;
using ContainerManager.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ContainerManager.UnitTests.Repositories
{
	public class UserRepositoryTests
	{
		private readonly IMapper _mapper;
		private readonly IFixture _fixture;

		public UserRepositoryTests()
		{

			var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new Infrastructure.Entities.DataMappingProfile()));
			_mapper = mapperConfig.CreateMapper();
			_fixture = new Fixture();
		}

		[Fact]
		public async Task AddAsync_ValidUserPassed_CreatesNewUser()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;
			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
		
			// Act
			var userId = Guid.NewGuid();
			var user = _fixture.Build<User>().
			
				With(u => u.Id, userId).
				Create();
			var userRepository = new UserRepository(_mapper, dbContext);
			await userRepository.AddAsync(user);

			//Assert
			var dbUser = await userRepository.GetByIdAsync(userId);
			dbUser.Should().NotBeNull();
			dbUser.Id.Should().Be(userId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task AddAsync_ExistingUserIdPassed_ThrowsRecordAlreadyExistsException()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);		
			var userId = Guid.NewGuid();
			var user = _fixture.Build<User>().			
				With(u => u.Id, userId).
				Create();
			var userRepository = new UserRepository(_mapper, dbContext);
			await userRepository.AddAsync(user);

			// Act
			var result = new Func<Task>(async () => await userRepository.AddAsync(user));

			//Assert
			await result.Should().ThrowAsync<RecordAlreadyExistsException>();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task GetByIdAsync_ValidIdPassed_ReturnsExistingUser()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();		
			
			var user = _fixture.Build<User>().				
				With(u => u.Id, userId).
				Create();
			var userRepository = new UserRepository(_mapper, dbContext);
			await userRepository.AddAsync(user);

			//Act
			var dbUser = await userRepository.GetByIdAsync(userId);

			//Assert
			dbUser.Should().NotBeNull();
			dbUser.Id.Should().Be(userId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task GetByIdAsync_InvalidIdPassed_ReturnsNull()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);

			//Act
			var userRepository = new UserRepository(_mapper, dbContext);
			var dbUser = await userRepository.GetByIdAsync(Guid.NewGuid());

			//Assert
			dbUser.Should().BeNull();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task GetByApiKey_ValidApiKeyPassed_ReturnsExistingUser()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var apiKey = "testingApiKey123";

			var user = _fixture.Build<User>().
				With(u => u.Id, userId).
				With(u => u.ApiKey, apiKey).
				Create();
			var userRepository = new UserRepository(_mapper, dbContext);
			await userRepository.AddAsync(user);

			//Act
			var dbUser = await userRepository.GetByApiKey(apiKey);

			//Assert
			dbUser.Should().NotBeNull();
			dbUser.Id.Should().Be(userId);
			dbUser.ApiKey.Should().Be(apiKey);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task DeleteAsync_ValidUserIdPassed_DeletesExistingUser()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var user = _fixture.Build<User>().				
				With(u => u.Id, userId).
				Create();
			var userRepository = new UserRepository(_mapper, dbContext);
			await userRepository.AddAsync(user);

			using (var dbContextDelete = new ContainerManagerDbContext(dbContextOptions))
			{
				// Act
				var deleteUserRepository = new UserRepository(_mapper, dbContextDelete);
				await deleteUserRepository.DeleteAsync(userId);

				//Assert
				var dbUser = await deleteUserRepository.GetByIdAsync(userId);
				dbUser.Should().BeNull();

				//Cleanup
				dbContextDelete.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}		

	}
}
