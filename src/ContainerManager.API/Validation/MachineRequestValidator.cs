using ContainerManager.API.ViewModels;
using FluentValidation;

namespace ContainerManager.API.Validation
{
	public class MachineRequestValidator : AbstractValidator<MachineRequest>
	{
		public MachineRequestValidator()
		{
			RuleFor(request => request.Name).NotEmpty();
			RuleFor(request => request.OS).NotEmpty();
		}
	}
}
