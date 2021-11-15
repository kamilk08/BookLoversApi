using System;
using FluentValidation;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Validation
{
    internal class ValidatorsFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return CompositionRoot.Kernel.Get(validatorType) as IValidator;
        }
    }
}