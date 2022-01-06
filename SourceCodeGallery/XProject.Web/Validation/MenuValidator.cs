using FluentValidation;
using XProject.Domain.Entities;
using Resources;

namespace XProject.Web.Validation
{
    public class MenuValidator : AbstractValidator<Menu>
    {
        public MenuValidator()
        {
            RuleFor(m => m.Title).NotEmpty().WithMessage(Resource.TheFieldShouldNotBeEmpty);
        }
    }
}
