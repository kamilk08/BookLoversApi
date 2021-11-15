using System;
using FluentValidation;
using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.Validation
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return CompositionRoot.Kernel.Get(validatorType) as IValidator;
        }
    }
}