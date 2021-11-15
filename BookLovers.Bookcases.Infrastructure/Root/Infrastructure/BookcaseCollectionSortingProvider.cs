using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Bookcases.Infrastructure.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Bookcases.Infrastructure.Root.Infrastructure
{
    internal class BookcaseCollectionSortingProvider :
        BaseProvider<IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter>>
    {
        public override Type Type => typeof(IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter>);

        protected override IDictionary<BookcaseCollectionSortType, IBookcaseCollectionSorter> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IBookcaseCollectionSorter>()
                .ToDictionary(k => k.SortType, v => v);
        }
    }
}