using System;

namespace BookLovers.Readers.Domain.Profiles.Services.Factories
{
    public class ProfileData
    {
        public Guid ProfileGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public ProfileContentData ContentData { get; private set; }

        public ProfileDetailsData DetailsData { get; private set; }

        public DateTime JoinedAt { get; private set; }

        private ProfileData()
        {
        }

        public static ProfileData Initialize()
        {
            return new ProfileData();
        }

        public ProfileData WithProfile(Guid profileGuid)
        {
            ProfileGuid = profileGuid;
            return this;
        }

        public ProfileData WithReader(Guid readerGuid)
        {
            ReaderGuid = readerGuid;
            return this;
        }

        public ProfileData WithContent(ProfileContentData content)
        {
            ContentData = content;
            return this;
        }

        public ProfileData WithDetails(ProfileDetailsData details)
        {
            DetailsData = details;
            return this;
        }

        public ProfileData WithJoinedAt(DateTime joinedAt)
        {
            JoinedAt = joinedAt;
            return this;
        }
    }
}