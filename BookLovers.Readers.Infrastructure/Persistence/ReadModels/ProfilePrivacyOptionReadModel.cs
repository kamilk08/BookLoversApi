namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ProfilePrivacyOptionReadModel
    {
        public int Id { get; set; }

        public string PrivacyTypeName { get; set; }

        public int PrivacyTypeId { get; set; }

        public string PrivacyOptionName { get; set; }

        public int PrivacyTypeOptionId { get; set; }
    }
}