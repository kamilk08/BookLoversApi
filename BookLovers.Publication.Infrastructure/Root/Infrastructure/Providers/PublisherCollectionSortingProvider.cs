using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure.Providers
{
    internal class PublisherCollectionSortingProvider :
        BaseProvider<IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter>>
    {
        public override Type Type => typeof(IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter>);

        protected override IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IPublisherCollectionSorter>()
                .ToDictionary(k => k.SortingType, v => v);
        }
    }
}