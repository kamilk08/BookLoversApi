using System;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;

namespace BookLovers.Readers.Infrastructure.EventHandlers
{
    public class AvatarChanged : IInfrastructureEvent
    {
        public Guid ReaderGuid { get; }

        public string AvatarUrl { get; }

        public string FileName { get; }

        public string MimeType { get; }

        public AvatarChanged(Guid readeGuid, string avatarUrl, string fileName, string mimeType)
        {
            this.ReaderGuid = readeGuid;
            this.AvatarUrl = avatarUrl;
            this.FileName = fileName;
            this.MimeType = mimeType;
        }
    }
}