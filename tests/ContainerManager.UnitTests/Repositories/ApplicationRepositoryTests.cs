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
	public class ApplicationRepositoryTests
	{
		private readonly IMapper _mapper;
		private readonly IFixture _fixture;

		public ApplicationRepositoryTests()
		{

			var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new Infrastructure.Entities.DataMappingProfile()));
			_mapper = mapperConfig.CreateMapper();
			_fixture = new Fixture();
		}

		[Fact]
		public async Task AddAsync_ValidApplicationPassed_CreatesNewApplication()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);

			// Act
			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			//Assert
			var dbApplication = await applicationRepository.GetByIdAsync(applicationId);
			dbApplication.Should().NotBeNull();
			dbApplication.Id.Should().Be(applicationId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task AddAsync_ExistingapplicationIdPassed_ThrowsRecordAlreadyExistsException()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);
			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			// Act
			var result = new Func<Task>(async () => await applicationRepository.AddAsync(application));

			//Assert
			await result.Should().ThrowAsync<RecordAlreadyExistsException>();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task GetByIdAsync_ValidIdPassed_ReturnsExistingApplication()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);
			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			//Act
			var dbApplication = await applicationRepository.GetByIdAsync(applicationId);

			//Assert
			dbApplication.Should().NotBeNull();
			dbApplication.Id.Should().Be(applicationId);

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
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			var dbApplication = await applicationRepository.GetByIdAsync(Guid.NewGuid());

			//Assert
			dbApplication.Should().BeNull();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task GetByOwner_ValidUserIdPassed_ReturnsExistingApplications()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);
			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			//Act
			var applications = await applicationRepository.GetByOwner(userId);

			//Assert
			applications.Should().NotBeNull();
			var applicationList = applications.ToList();
			applicationList.Count.Should().BeGreaterThan(0);
			applicationList.FirstOrDefault()?.Id.Should().Be(applicationId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task DeleteAsync_ValidapplicationIdPassed_DeletesExistingApplication()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);
			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			using (var dbContextDelete = new ContainerManagerDbContext(dbContextOptions))
			{
				// Act
				var deleteApplicationRepository = new ApplicationRepository(_mapper, dbContextDelete);
				await deleteApplicationRepository.DeleteAsync(applicationId);

				//Assert
				var dbApplication = await deleteApplicationRepository.GetByIdAsync(applicationId);
				dbApplication.Should().BeNull();

				//Cleanup
				dbContextDelete.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task UpdateOwnership_ValidApplicationPassed_UpdateOwnerId()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userId2 = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			var user2 = _fixture.Build<User>().With(u => u.Id, userId2).Create();
			await userRepository.AddAsync(user);
			await userRepository.AddAsync(user2);

			var applicationId = Guid.NewGuid();
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);


			using (var dbContextAppUpdate = new ContainerManagerDbContext(dbContextOptions))
			{
				//Act
				var applicationRepositoryUpdate = new ApplicationRepository(_mapper, dbContextAppUpdate);
				await applicationRepositoryUpdate.UpdateOwnership(userId, userId2);

				//Assert
				var dbApplication = await applicationRepositoryUpdate.GetByIdAsync(applicationId);
				dbApplication.Should().NotBeNull();
				dbApplication.OwnerId.Should().Be(userId2);

				//Cleanup
				dbContextAppUpdate.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task UpdateMachineId_ValidApplicationPassed_UpdateMachineId()
		{
			//Arrange
			var dbContextOptions = new DbContextOptionsBuilder<ContainerManagerDbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;

			using var dbContext = new ContainerManagerDbContext(dbContextOptions);
			var userId = Guid.NewGuid();
			var userRepository = new UserRepository(_mapper, dbContext);
			var user = _fixture.Build<User>().With(u => u.Id, userId).Create();
			await userRepository.AddAsync(user);

			var machineId = Guid.NewGuid();
			var machine = _fixture.Build<Machine>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, machineId).
				Create();
			var machineRepository = new MachineRepository(_mapper, dbContext);
			await machineRepository.AddAsync(machine);

			var applicationId = Guid.NewGuid();
			var machineDb = await machineRepository.GetByIdAsync(machineId);
			var application = _fixture.Build<Application>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, applicationId).
				Create();
			var applicationRepository = new ApplicationRepository(_mapper, dbContext);
			await applicationRepository.AddAsync(application);

			using (var dbContextAppUpdate = new ContainerManagerDbContext(dbContextOptions))
			{
				//Act
				application.Machine = machine;
				var applicationRepositoryUpdate = new ApplicationRepository(_mapper, dbContextAppUpdate);			
				await applicationRepositoryUpdate.UpdateAsync(application);

				//Assert
				var dbApplication = await applicationRepositoryUpdate.GetByIdAsync(applicationId);
				dbApplication.Should().NotBeNull();
				dbApplication.Machine.Should().NotBeNull();
				dbApplication.Machine.Id.Should().Be(machineId);
				dbApplication.Machine.Name.Should().Be(machine.Name);

				//Cleanup
				dbContextAppUpdate.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

	}
}
