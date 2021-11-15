using System;

namespace BookLovers.Seed.Models.Configuration
{
    public class OpenLibrarySeedConfiguration
    {
        public SourceType SourceType { get; set; }

        public DateTime? PublishedFrom { get; set; }

        public DateTime? PublishedTill { get; set; }

        public bool OnlyWithAuthors { get; set; }

        public bool OnlyWithDescription { get; set; }

        public bool OnlyWithCovers { get; set; }

        public bool OnlyWithIsbn { get; set; }

        public int Limit { get; set; }

        private OpenLibrarySeedConfiguration()
        {
        }

        public static OpenLibrarySeedConfiguration Initialize()
        {
            return new OpenLibrarySeedConfiguration();
        }

        public static OpenLibrarySeedConfiguration Default()
        {
            return new OpenLibrarySeedConfiguration()
                .WithSource(SourceType.OpenLibrary).WithLimit(100);
        }

        public OpenLibrarySeedConfiguration PublicationRange(
            DateTime from,
            DateTime till)
        {
            this.PublishedFrom = new DateTime?(from);
            this.PublishedTill = new DateTime?(till);
            return this;
        }

        public OpenLibrarySeedConfiguration WithDescription()
        {
            this.OnlyWithDescription = true;

            return this;
        }

        public OpenLibrarySeedConfiguration WithCovers()
        {
            this.OnlyWithCovers = true;

            return this;
        }

        public OpenLibrarySeedConfiguration WithSource(SourceType sourceType)
        {
            this.SourceType = sourceType;

            return this;
        }

        public OpenLibrarySeedConfiguration WithLimit(int limit)
        {
            this.Limit = limit;

            return this;
        }

        public OpenLibrarySeedConfiguration WithAuthors()
        {
            this.OnlyWithAuthors = true;

            return this;
        }

        public OpenLibrarySeedConfiguration WithIsbn()
        {
            this.OnlyWithIsbn = true;

            return this;
        }
    }
}