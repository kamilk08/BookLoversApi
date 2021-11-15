using BookLovers.Base.Infrastructure;
using FluentValidation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Validation
{
    internal class ValidationModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ValidatorFactory>().ToSelf().InSingletonScope();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(AbstractValidator<>)).BindAllInterfaces());

            this.Bind<IValidationDecorator<ReadersModule>>()
                .To<ModuleValidationDecorator>();
        }
    }
}