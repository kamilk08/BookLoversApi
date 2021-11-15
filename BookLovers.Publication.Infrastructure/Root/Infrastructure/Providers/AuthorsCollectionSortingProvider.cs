using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure.Providers
{
    internal class AuthorsCollectionSortingProvider :
        BaseProvider<IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter>>
    {
        public override Type Type => typeof(IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter>);

        protected override IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IAuthorCollectionSorter>()
                .ToDictionary(
                    k => k.SorType,
                    v => v);
        }
    }
}