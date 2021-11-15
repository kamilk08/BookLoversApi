using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Auth.Integration.IntegrationEvents
{
    public class UserBlockedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid UserGuid { get; private set; }

        public bool IsLibrarian { get; private set; }

        private UserBlockedIntegrationEvent()
        {
        }

        public UserBlockedIntegrationEvent(Guid userGuid, bool isLibrarian)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.UserGuid = userGuid;
            this.IsLibrarian = isLibrarian;
        }
    }
}