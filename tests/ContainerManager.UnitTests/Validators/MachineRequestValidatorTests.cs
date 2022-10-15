using AutoFixture;
using ContainerManager.API.Validation;
using ContainerManager.API.ViewModels;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Xunit;


namespace ContainerManager.UnitTests.Validators
{
	public class MachineRequestValidatorTests
	{
		private readonly MachineRequestValidator _validator;
		private readonly IFixture _fixture;
		private readonly MachineRequest _machineRequest;

		public MachineRequestValidatorTests()
		{
			_fixture = new Fixture();
			_machineRequest = _fixture.Create<MachineRequest>();
			_validator = new MachineRequestValidator();
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Name_IsNullOrEmpty_ShouldHaveError(string name)
		{
			_machineRequest.Name = name;
			var result = _validator.TestValidate(_machineRequest);
			result.ShouldHaveValidationErrorFor(request=>request.Name);
		}

		[Fact]
		public void OS_IsNullOrEmpty_ShouldHaveError()
		{
			_machineRequest.OS = null;
			var result = _validator.TestValidate(_machineRequest);
			result.ShouldHaveValidationErrorFor(request => request.OS);
		}

	}
}
