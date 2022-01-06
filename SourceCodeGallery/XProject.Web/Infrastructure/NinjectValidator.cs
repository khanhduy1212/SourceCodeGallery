using FluentValidation;
using Ninject.Modules;

namespace XProject.Web.Infrastructure
{
    public class NinjectValidator : NinjectModule
    {
        public override void Load()
        {
            AssemblyScanner.FindValidatorsInAssemblyContaining<IValidator>()
                           .ForEach(match => Bind(match.InterfaceType).To(match.ValidatorType));
        }
    }
}