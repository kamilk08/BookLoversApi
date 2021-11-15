using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Profiles
{
    public class About : ValueObject<About>
    {
        public DateTime JoinedAt { get; }

        public string WebSite { get; }

        public string AboutYourself { get; }

        private About()
        {
        }

        public About(DateTime joinedAt, string webSite, string aboutYourself)
        {
            JoinedAt = joinedAt;
            WebSite = webSite;
            AboutYourself = aboutYourself;
        }

        public About(string webSite, string aboutYourself)
        {
            JoinedAt = JoinedAt;
            WebSite = webSite;
            AboutYourself = aboutYourself;
        }

        public static About Default(DateTime joinedAt)
        {
            return new About(joinedAt, null, null);
        }

        protected override bool EqualsCore(About obj)
        {
            return AboutYourself == obj.AboutYourself
                   && WebSite == obj.WebSite
                   && JoinedAt == obj.JoinedAt;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AboutYourself.GetHashCode();
            hash = (hash * 23) + this.JoinedAt.GetHashCode();
            hash = (hash * 23) + this.WebSite.GetHashCode();

            return hash;
        }
    }
}