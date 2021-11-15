using AutoMapper;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Persistence.Providers
{
    internal class PromotionAvailabilityProvider : IPromotionAvailabilityProvider
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public PromotionAvailabilityProvider(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public PromotionAvailability GetPromotionAvailability(int availabilityId)
        {
            var readModel = this._context.PromotionAvailabilities.Single(p => p.AvailabilityId == availabilityId);

            return this._mapper.Map<PromotionAvailabilityReadModel, PromotionAvailability>(readModel);
        }
    }
}