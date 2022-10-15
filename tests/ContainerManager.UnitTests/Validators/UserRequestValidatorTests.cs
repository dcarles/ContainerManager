using AutoFixture;
using ContainerManager.API.Validation;
using ContainerManager.API.ViewModels;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using Xunit;


namespace ContainerManager.UnitTests.Validators
{
	public class UserRequestValidatorTests
	{
		private readonly UserRequestValidator _validator;
		private readonly IFixture _fixture;
		private readonly UserRequest _userRequest;

		public UserRequestValidatorTests()
		{
			_fixture = new Fixture();
			_userRequest = _fixture.Create<UserRequest>();
			_validator = new UserRequestValidator();
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Firstname_IsNullOrEmpty_ShouldHaveError(string name)
		{
			_userRequest.FirstName = name;
			var result = _validator.TestValidate(_userRequest);
			result.ShouldHaveValidationErrorFor(request=>request.FirstName);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Lastname_IsNullOrEmpty_ShouldHaveError(string name)
		{
			_userRequest.LastName = name;
			var result = _validator.TestValidate(_userRequest);
			result.ShouldHaveValidationErrorFor(request => request.LastName);
		}


		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Email_IsNullOrEmpty_ShouldHaveError(string name)
		{
			_userRequest.Email = name;
			var result = _validator.TestValidate(_userRequest);
			result.ShouldHaveValidationErrorFor(request => request.Email);
		}

		[Fact]
		public void Email_IsInvalidFormat_ShouldHaveError()
		{
			_userRequest.Email = "SomeRandomText";
			var result = _validator.TestValidate(_userRequest);
			result.ShouldHaveValidationErrorFor(request => request.Email);
		}

	}
}
