using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Domain.Authors.Services.Editors;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Domain.Providers
{
    internal class AuthorEditorsProvider : BaseProvider<IList<IAuthorEditor>>
    {
        public override Type Type => typeof(IList<IAuthorEditor>);

        protected override IList<IAuthorEditor> CreateInstance(IContext context)
        {
            return context.Kernel.GetAll<IAuthorEditor>().ToList();
        }
    }
}