using FluentValidation;
using XProject.Domain.Entities;
using Resources;

namespace XProject.Web.Validation
{
    public class CurrencyValidator : AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage(Resource.TheFieldShouldNotBeEmpty);
            RuleFor(m => m.Symbol).NotEmpty().WithMessage(Resource.TheFieldShouldNotBeEmpty);
        }
    }
}
