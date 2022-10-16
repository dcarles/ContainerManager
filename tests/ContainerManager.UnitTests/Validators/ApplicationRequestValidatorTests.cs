using AutoFixture;
using ContainerManager.API.Validation;
using ContainerManager.API.ViewModels;
using FluentValidation.TestHelper;
using System;
using Xunit;


namespace ContainerManager.UnitTests.Validators
{
	public class ApplicationRequestValidatorTests
	{
		private readonly ApplicationRequestValidator _validator;
		private readonly IFixture _fixture;
		private readonly ApplicationRequest _applicationRequest;
		private const string PostRuleSetName = "Post";
		private const string PatchRuleSetName = "Patch";

		public ApplicationRequestValidatorTests()
		{
			_fixture = new Fixture();
			_applicationRequest = _fixture.Create<ApplicationRequest>();
			_validator = new ApplicationRequestValidator();
		}


		#region POST rules

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Post_Name_IsNullOrEmpty_ShouldHaveError(string name)
		{
			_applicationRequest.Name = name;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Name);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(null)]
		public void Post_Port_IsNullOrZero_ShouldHaveError(int? value)
		{
			_applicationRequest.Port = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Port);
		}


		[Theory]
		[InlineData(0)]
		[InlineData(null)]
		public void Post_CPULimit_IsNullOrZero_ShouldHaveError(int? value)
		{
			_applicationRequest.CPULimit = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.CPULimit);
		}


		[Theory]
		[InlineData(0)]
		[InlineData(null)]
		public void Post_MemoryLimit_IsNullOrZero_ShouldHaveError(int? value)
		{
			_applicationRequest.MemoryMBLimit = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.MemoryMBLimit);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Post_Image_IsNullOrEmpty_ShouldHaveError(string value)
		{
			_applicationRequest.Image = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Image);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Post_Command_IsNullOrEmpty_ShouldHaveError(string value)
		{
			_applicationRequest.Command = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Command);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void Post_WorkingDirectory_IsNullOrEmpty_ShouldHaveError(string value)
		{
			_applicationRequest.WorkingDirectory = value;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.WorkingDirectory);
		}

		[Fact]
		public void Post_MachineId_HasValue_ShouldHaveError()
		{
			_applicationRequest.MachineId = Guid.NewGuid();
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PostRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.MachineId);
		}

		#endregion

		#region PATCH rules

		[Fact]
		public void Patch_Name_HasValue_ShouldHaveError()
		{
			_applicationRequest.Name = "someRandomName";
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Name);
		}

		[Fact]
		public void Patch_Port_HasValue_ShouldHaveError()
		{
			_applicationRequest.Port = 8080;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Port);
		}

		[Fact]
		public void Patch_CPULimit_HasValue_ShouldHaveError()
		{
			_applicationRequest.CPULimit = 123;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.CPULimit);
		}

		[Fact]
		public void Patch_MemoryMBLimit_HasValue_ShouldHaveError()
		{
			_applicationRequest.MemoryMBLimit = 1024;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.MemoryMBLimit);
		}

		[Fact]
		public void Patch_Image_HasValue_ShouldHaveError()
		{
			_applicationRequest.Image = "someRandomName";
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Image);
		}

		[Fact]
		public void Patch_Command_HasValue_ShouldHaveError()
		{
			_applicationRequest.Command = "someRandomCommand";
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.Command);
		}

		[Fact]
		public void Patch_WorkingDirectory_HasValue_ShouldHaveError()
		{
			_applicationRequest.WorkingDirectory = "someRandomWorkingDirectory";
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.WorkingDirectory);
		}

		[Fact]
		public void Patch_MachineId_IsNull_ShouldHaveError()
		{
			_applicationRequest.MachineId = null;
			var result = _validator.TestValidate(_applicationRequest, options => options.IncludeRuleSets(PatchRuleSetName));
			result.ShouldHaveValidationErrorFor(request => request.MachineId);
		}

		#endregion
	}
}
