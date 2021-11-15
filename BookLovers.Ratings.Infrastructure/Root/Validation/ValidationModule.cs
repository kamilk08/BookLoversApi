using BookLovers.Base.Infrastructure;
using FluentValidation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Validation
{
    internal class ValidationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ValidatorFactory>().ToSelf().InSingletonScope();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(AbstractValidator<>))
                    .BindAllInterfaces());

            Bind<IValidationDecorator<RatingsModule>>()
                .To<ModuleValidationDecorator>();
        }
    }
}