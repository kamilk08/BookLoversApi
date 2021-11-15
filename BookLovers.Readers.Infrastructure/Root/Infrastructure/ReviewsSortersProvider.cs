using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Readers.Infrastructure.Root.Infrastructure
{
    internal class ReviewsSortersProvider :
        BaseProvider<IDictionary<ReviewsSortingType, IReviewsSorter>>
    {
        public override Type Type => typeof(IDictionary<ReviewsSortingType, IReviewsSorter>);

        protected override IDictionary<ReviewsSortingType, IReviewsSorter> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IReviewsSorter>()
                .ToDictionary(k => k.SortingType, v => v);
        }
    }
}