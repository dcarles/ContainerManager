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
	public class MachineRepositoryTests
	{
		private readonly IMapper _mapper;
		private readonly IFixture _fixture;

		public MachineRepositoryTests()
		{

			var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new Infrastructure.Entities.DataMappingProfile()));
			_mapper = mapperConfig.CreateMapper();
			_fixture = new Fixture();
		}

		[Fact]
		public async Task AddAsync_ValidMachinePassed_CreatesNewMachine()
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
			var machineId = Guid.NewGuid();
			var machine = _fixture.Build<Machine>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, machineId).
				Create();
			var machineRepository = new MachineRepository(_mapper, dbContext);
			await machineRepository.AddAsync(machine);

			//Assert
			var dbMachine = await machineRepository.GetByIdAsync(machineId);
			dbMachine.Should().NotBeNull();
			dbMachine.Id.Should().Be(machineId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task AddAsync_ExistingMachineIdPassed_ThrowsRecordAlreadyExistsException()
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

			// Act
			var result = new Func<Task>(async () => await machineRepository.AddAsync(machine));

			//Assert
			await result.Should().ThrowAsync<RecordAlreadyExistsException>();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task GetByIdAsync_ValidIdPassed_ReturnsExistingMachine()
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

			//Act
			var dbMachine = await machineRepository.GetByIdAsync(machineId);

			//Assert
			dbMachine.Should().NotBeNull();
			dbMachine.Id.Should().Be(machineId);

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
			var machineRepository = new MachineRepository(_mapper, dbContext);
			var dbMachine = await machineRepository.GetByIdAsync(Guid.NewGuid());

			//Assert
			dbMachine.Should().BeNull();

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}


		[Fact]
		public async Task GetByOwner_ValidUserIdPassed_ReturnsExistingMachines()
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

			//Act
			var machines = await machineRepository.GetByOwner(userId);

			//Assert
			machines.Should().NotBeNull();
			var machineList = machines.ToList();
			machineList.Count.Should().BeGreaterThan(0);
			machineList.FirstOrDefault()?.Id.Should().Be(machineId);

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task DeleteAsync_ValidMachineIdPassed_DeletesExistingMachine()
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

			using (var dbContextDelete = new ContainerManagerDbContext(dbContextOptions))
			{
				// Act
				var deleteMachineRepository = new MachineRepository(_mapper, dbContextDelete);
				await deleteMachineRepository.DeleteAsync(machineId);

				//Assert
				var dbMachine = await deleteMachineRepository.GetByIdAsync(machineId);
				dbMachine.Should().BeNull();

				//Cleanup
				dbContextDelete.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

		[Fact]
		public async Task UpdateOwnership_ValidMachinePassed_UpdateOwnerId()
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
			
			var machineId = Guid.NewGuid();
			var machine = _fixture.Build<Machine>().
				With(u => u.OwnerId, userId).
				With(u => u.Id, machineId).
				Create();
			var machineRepository = new MachineRepository(_mapper, dbContext);
			await machineRepository.AddAsync(machine);

			

			using (var dbContextUpdate = new ContainerManagerDbContext(dbContextOptions))
			{
				//Act
				var machineRepositoryUpdate = new MachineRepository(_mapper, dbContextUpdate);
				await machineRepositoryUpdate.UpdateOwnership(userId, userId2);

				//Assert
				var dbMachine = await machineRepositoryUpdate.GetByIdAsync(machineId);
				dbMachine.Should().NotBeNull();
				dbMachine.OwnerId.Should().Be(userId2);

				//Cleanup
				dbContextUpdate.Database.EnsureDeleted();
			}

			//Cleanup
			dbContext.Database.EnsureDeleted();
		}

	}
}
