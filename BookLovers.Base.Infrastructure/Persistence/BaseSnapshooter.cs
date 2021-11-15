using System;
using BookLovers.Base.Domain;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public abstract class BaseSnapshooter
    {
        private const int InvalidFrequency = 0;
        internal const int MinFrequency = 10;
        internal const int MaxFrequency = 100;

        private readonly SnapshotSettings _settings;
        protected readonly IMementoFactory MementoFactory;

        protected BaseSnapshooter(IMementoFactory mementoFactory, SnapshotSettings settings)
        {
            if (settings.SnapshotFrequency <= 0)
                throw new ArgumentException("Frequency cannot be less or equal to zero");

            _settings = settings.SnapshotFrequency >= MinFrequency
                        && settings.SnapshotFrequency <= MaxFrequency
                ? settings
                : throw new ArgumentException(
                    $"Frequency cannot be less then {MinFrequency} or greater then {MaxFrequency}");

            MementoFactory = mementoFactory;
        }

        public abstract ISnapshot MakeSnapshot<TAggregate>(TAggregate aggregate)
            where TAggregate : class;

        protected bool CanMakeSnapshot(Type aggregateType, int currentVersion, int comittedChanges)
        {
            return IsDecoratedByAttribute(aggregateType) && ShouldMakeSnapshot(currentVersion, comittedChanges);
        }

        private bool IsDecoratedByAttribute(Type aggregateType)
        {
            return Attribute.IsDefined(aggregateType, typeof(AllowSnapshot)) && !aggregateType.IsInterface;
        }

        private bool ShouldMakeSnapshot(int currentAggregateVersion, int comittedChanges)
        {
            return currentAggregateVersion >= _settings.SnapshotFrequency &&
                   (comittedChanges >= _settings.SnapshotFrequency ||
                    currentAggregateVersion %
                    _settings.SnapshotFrequency <= comittedChanges ||
                    currentAggregateVersion %
                    _settings.SnapshotFrequency == 0);
        }
    }
}