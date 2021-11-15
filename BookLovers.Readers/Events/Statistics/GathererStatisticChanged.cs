using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Statistics
{
    public class GathererStatisticChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int StatisticType { get; private set; }

        public int StatisticStep { get; private set; }

        public Dictionary<int, int> Statistics { get; internal set; }

        private GathererStatisticChanged()
        {
        }

        [JsonConstructor]
        protected GathererStatisticChanged(
            Guid guid,
            Guid aggregateGuid,
            int statisticType,
            int statisticStep)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            StatisticType = statisticType;
            StatisticStep = statisticStep;
        }

        public GathererStatisticChanged(Guid aggregateGuid, int type, int step)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            StatisticType = type;
            StatisticStep = step;
        }
    }
}