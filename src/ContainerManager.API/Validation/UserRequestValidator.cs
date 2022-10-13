using ContainerManager.API.ViewModels;
using FluentValidation;

namespace ContainerManager.API.Validation
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(request => request.FirstName).NotEmpty();
            RuleFor(request => request.LastName).NotEmpty();
            RuleFor(request => request.Email).NotEmpty().EmailAddress();
        }
    }
}
