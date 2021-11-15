using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure.Providers
{
    internal class SeriesCollectionSortingProvider :
        BaseProvider<IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter>>
    {
        public override Type Type => typeof(IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter>);

        protected override IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<ISeriesCollectionSorter>()
                .ToDictionary(k => k.SortingType, v => v);
        }
    }
}