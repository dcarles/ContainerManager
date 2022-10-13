using ContainerManager.API.ViewModels;
using FluentValidation;

namespace ContainerManager.API.Validation
{
    public class ApplicationRequestValidator : AbstractValidator<ApplicationRequest>
    {
        public ApplicationRequestValidator()
        {          
            RuleSet("Post", () =>
            {
                RuleFor(request => request.Name).NotEmpty();
                RuleFor(request => request.Port).NotNull().GreaterThan(0);
                RuleFor(request => request.Command).NotEmpty();
                RuleFor(request => request.CPULimit).NotNull().GreaterThan(0);
                RuleFor(request => request.MemoryMBLimit).NotNull().GreaterThan(0);
                RuleFor(request => request.Image).NotEmpty();
                RuleFor(request => request.WorkingDirectory).NotEmpty();
                RuleFor(request => request.MachineId).Null();
            });

            RuleSet("Patch", () =>
            {
                RuleFor(request => request.Name).Null();
                RuleFor(request => request.Port).Null();
                RuleFor(request => request.Command).Null();
                RuleFor(request => request.CPULimit).Null();
                RuleFor(request => request.MemoryMBLimit).Null();
                RuleFor(request => request.Image).Null();
                RuleFor(request => request.WorkingDirectory).Null();
                RuleFor(request => request.MachineId).NotEmpty();
            });
        }
    }
}
