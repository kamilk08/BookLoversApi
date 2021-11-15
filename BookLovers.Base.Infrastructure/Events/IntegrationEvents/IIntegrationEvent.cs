using System;

namespace BookLovers.Base.Infrastructure.Events.IntegrationEvents
{
    public interface IIntegrationEvent
    {
        Guid Guid { get; }

        DateTime OccuredOn { get; }
    }
}