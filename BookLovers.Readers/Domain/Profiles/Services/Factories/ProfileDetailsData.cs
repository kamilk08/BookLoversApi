namespace BookLovers.Readers.Domain.Profiles.Services.Factories
{
    public class ProfileDetailsData
    {
        public string Country { get; private set; }

        public string City { get; private set; }

        public string WebSite { get; private set; }

        public string AboutUser { get; private set; }

        private ProfileDetailsData()
        {
        }

        public static ProfileDetailsData Initialize()
        {
            return new ProfileDetailsData();
        }

        public ProfileDetailsData WithCountry(string country)
        {
            Country = country;

            return this;
        }

        public ProfileDetailsData WithCity(string city)
        {
            City = city;

            return this;
        }

        public ProfileDetailsData WithWebSite(string webSite)
        {
            WebSite = webSite;

            return this;
        }

        public ProfileDetailsData WithAboutUser(string aboutUser)
        {
            AboutUser = aboutUser;

            return this;
        }
    }
}