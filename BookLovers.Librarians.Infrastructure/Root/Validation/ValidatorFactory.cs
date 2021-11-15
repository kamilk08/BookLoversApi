using FluentValidation;
using Ninject;
using System;

namespace BookLovers.Librarians.Infrastructure.Root.Validation
{
    internal class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return CompositionRoot.Kernel.Get(validatorType) as IValidator;
        }
    }
}