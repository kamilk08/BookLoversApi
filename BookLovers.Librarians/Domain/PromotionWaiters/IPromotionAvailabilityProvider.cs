namespace BookLovers.Librarians.Domain.PromotionWaiters
{
    public interface IPromotionAvailabilityProvider
    {
        PromotionAvailability GetPromotionAvailability(int availabilityId);
    }
}