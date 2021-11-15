using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Persistence.Providers
{
    public class TicketConcernProvider : ITicketConcernProvider, ITicketConcernChecker
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public TicketConcernProvider(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public TicketConcern GetTicketConcern(int concernType)
        {
            var concern = this._context.TickerConcerns.SingleOrDefault(p => p.Value == concernType);

            return this._mapper.Map<TicketConcernReadModel, TicketConcern>(concern);
        }

        public bool IsConcernValid(int concernType)
        {
            return this._context.TickerConcerns.AsNoTracking().Any(a => a.Value == concernType);
        }
    }
}