using System;
using FluentValidation;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Validation
{
    internal class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return CompositionRoot.Kernel.Get(validatorType) as IValidator;
        }
    }
}