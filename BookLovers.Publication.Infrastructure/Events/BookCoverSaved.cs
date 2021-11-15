using System;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;

namespace BookLovers.Publication.Infrastructure.Events
{
    public class BookCoverSaved : IInfrastructureEvent
    {
        public Guid BookGuid { get; }

        public string ImageUrl { get; }

        public string FileName { get; }

        public string MimeType { get; }

        public BookCoverSaved(Guid bookGuid, string imageUrl, string fileName, string mimeType)
        {
            this.BookGuid = bookGuid;
            this.ImageUrl = imageUrl;
            this.FileName = fileName;
            this.MimeType = mimeType;
        }
    }
}