using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Mementos
{
    public interface IStatisticsGathererMemento : IMemento<StatisticsGatherer>, IMemento
    {
        Guid ReaderGuid { get; }

        Guid ProfileGuid { get; }

        Dictionary<int, int> Statistics { get; }

        int GetStatisticValue(StatisticType statisticType);
    }
}