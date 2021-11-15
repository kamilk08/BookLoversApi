using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Events.Statistics;
using BookLovers.Readers.Mementos;

namespace BookLovers.Readers.Domain.Statistics
{
    [AllowSnapshot]
    public class StatisticsGatherer :
        EventSourcedAggregateRoot,
        IHandle<StatisticsGathererCreated>,
        IHandle<GathererStatisticChanged>
    {
        private List<IStatistic> _statistics = new List<IStatistic>();

        public Guid ReaderGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        public IReadOnlyList<IStatistic> Statistics => _statistics.ToList();

        private StatisticsGatherer()
        {
        }

        public StatisticsGatherer(Guid gathererGuid, Guid readerGuid, Guid profileGuid)
        {
            Guid = gathererGuid;
            ReaderGuid = readerGuid;
            ProfileGuid = profileGuid;

            ApplyChange(new StatisticsGathererCreated(Guid, profileGuid, readerGuid));
        }

        public void ChangeStatistic(StatisticType statisticType, StatisticStep step)
        {
            var @event = new GathererStatisticChanged(Guid, statisticType.Value, step.Value);

            ApplyChange(@event);
        }

        public void ChangeStatistic(int statisticType, int step)
        {
            var @event = new GathererStatisticChanged(Guid, statisticType, step);

            ApplyChange(@event);
        }

        public Dictionary<int, int> ToStatisticsDictionary()
        {
            return _statistics.ToDictionary(k => k.Type.Value, v => v.CurrentValue);
        }

        public IStatistic GetStatistic(int statisticType)
        {
            return _statistics.Find(p => p.Type.Value == statisticType);
        }

        public IStatistic GetStatistic(StatisticType statisticType)
        {
            return _statistics.Find(p => p.Type.Value == statisticType.Value);
        }

        void IHandle<StatisticsGathererCreated>.Handle(
            StatisticsGathererCreated @event)
        {
            AggregateStatus = AggregateStatus.Active;
            Guid = @event.AggregateGuid;
            ReaderGuid = @event.ReaderGuid;
            ProfileGuid = @event.ProfileGuid;

            foreach (var availableStatistic in StatisticType.AvailableStatistics)
                _statistics.Add(availableStatistic.Default());
        }

        void IHandle<GathererStatisticChanged>.Handle(
            GathererStatisticChanged @event)
        {
            var statistic = GetStatistic(@event.StatisticType);

            statistic = @event.StatisticStep == StatisticStep.Increase.Value
                ? statistic.Increase()
                : statistic.Decrease();

            var index = _statistics.FindIndex(p => p.Type == statistic.Type);

            _statistics[index] = statistic;

            @event.Statistics = ToStatisticsDictionary();
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var statisticsGathererMemento = memento as IStatisticsGathererMemento;

            Guid = statisticsGathererMemento.AggregateGuid;
            AggregateStatus = AggregateStates.Get(statisticsGathererMemento.AggregateStatus);
            LastCommittedVersion = statisticsGathererMemento.LastCommittedVersion;
            Version = statisticsGathererMemento.Version;

            ReaderGuid = statisticsGathererMemento.ReaderGuid;
            ProfileGuid = statisticsGathererMemento.ProfileGuid;

            foreach (var availableStatistic in StatisticType.AvailableStatistics)
            {
                int statisticValue = statisticsGathererMemento.GetStatisticValue(availableStatistic.Type);

                _statistics.Add(availableStatistic.WithValue(statisticValue));
            }
        }
    }
}