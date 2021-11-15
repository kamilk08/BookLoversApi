using AutoMapper;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.PromotionWaiters
{
    public class PromotionAvailabilityMapping : Profile
    {
        public PromotionAvailabilityMapping()
        {
            this.CreateMap<PromotionAvailabilityReadModel, PromotionAvailability>()
                .ConstructUsing(p => new PromotionAvailability(p.AvailabilityId, p.Name));
        }
    }
}