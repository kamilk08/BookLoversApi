using FluentValidation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Validation
{
    internal class ValidationModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(AbstractValidator<>))
                    .BindAllInterfaces());

            Bind<ValidatorFactory>().ToSelf();
        }
    }
}