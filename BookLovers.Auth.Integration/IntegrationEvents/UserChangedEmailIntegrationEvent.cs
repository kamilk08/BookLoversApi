using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Auth.Integration.IntegrationEvents
{
    public class UserChangedEmailIntegrationEvent : IIntegrationEvent
    {
        public string Email { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private UserChangedEmailIntegrationEvent()
        {
        }

        public UserChangedEmailIntegrationEvent(Guid readerGuid, string email)
        {
            this.Email = email;
            this.ReaderGuid = readerGuid;
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
        }
    }
}