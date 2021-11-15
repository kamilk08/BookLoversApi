using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Domain.Profiles.Services.Factories;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class ProfileEditor : IDomainService<Profile>
    {
        public void EditProfile(Profile profile, ProfileData profileData)
        {
            var address = new Address(profileData.DetailsData.Country, profileData.DetailsData.City);

            var identity = new Identity(profileData.ContentData.FullName, profileData.ContentData.Sex,
                profileData.ContentData.BirthDate);

            var about = new About(profile.About.JoinedAt, profileData.DetailsData.WebSite,
                profileData.DetailsData.AboutUser);

            if (address != profile.Address)
                profile.ChangeAddress(address);

            if (identity != profile.Identity)
                profile.ChangeIdentity(identity);

            if (about != profile.About)
                profile.ChangeAbout(about);
        }
    }
}