using System;
using FluentValidation;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Validation
{
    internal class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return CompositionRoot.Kernel.Get(validatorType) as IValidator;
        }

        public IValidator CreateValidator<T>()
        {
            return GetValidator<T>();
        }
    }
}