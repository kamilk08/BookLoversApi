namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class PromotionAvailabilityReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AvailabilityId { get; set; }
    }
}