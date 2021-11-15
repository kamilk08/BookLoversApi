using System;

namespace BookLovers.Readers.Domain.Readers
{
    public class ReaderSocials : BookLovers.Base.Domain.ValueObject.ValueObject<ReaderSocials>
    {
        public Guid ProfileGuid { get; }

        public Guid NotificationWallGuid { get; }

        public Guid StatisticsGathererGuid { get; }

        private ReaderSocials()
        {
        }

        public ReaderSocials(Guid profileGuid, Guid notificationWallGuid, Guid gathererGuid)
        {
            ProfileGuid = profileGuid;
            NotificationWallGuid = notificationWallGuid;
            StatisticsGathererGuid = gathererGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ProfileGuid.GetHashCode();
            hash = (hash * 23) + this.NotificationWallGuid.GetHashCode();
            hash = (hash * 23) + this.StatisticsGathererGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ReaderSocials obj)
        {
            return ProfileGuid == obj.ProfileGuid
                   && NotificationWallGuid == obj.NotificationWallGuid
                   && StatisticsGathererGuid == obj.StatisticsGathererGuid;
        }
    }
}