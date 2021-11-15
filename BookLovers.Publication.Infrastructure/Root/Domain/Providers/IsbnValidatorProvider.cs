using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Domain.Books.IsbnValidation;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Domain.Providers
{
    internal class IsbnValidatorProvider : BaseProvider<IDictionary<IsbnType, IIsbnValidator>>
    {
        public override Type Type => typeof(IDictionary<IsbnType, IIsbnValidator>);

        protected override IDictionary<IsbnType, IIsbnValidator> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IIsbnValidator>()
                .ToDictionary(k => k.Type, v => v);
        }
    }
}