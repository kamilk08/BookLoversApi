using System;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;

namespace BookLovers.Publication.Infrastructure.Events
{
    public class AuthorImageSaved : IInfrastructureEvent
    {
        public Guid AuthorGuid { get; }

        public string ImageUrl { get; }

        public string FileName { get; }

        public string MimeType { get; }

        public AuthorImageSaved(Guid authorGuid, string imageUrl, string fileName, string mimeType)
        {
            this.AuthorGuid = authorGuid;
            this.ImageUrl = imageUrl;
            this.FileName = fileName;
            this.MimeType = mimeType;
        }
    }
}