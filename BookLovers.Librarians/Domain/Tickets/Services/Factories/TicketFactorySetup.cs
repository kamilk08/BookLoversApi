using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.TicketOwners;

namespace BookLovers.Librarians.Domain.Tickets.Services.Factories
{
    public class TicketFactorySetup
    {
        private readonly TicketFactory _factory;

        public TicketFactorySetup(
            TicketFactory factory,
            ITicketOwnerRepository ticketOwnerRepository,
            ITicketConcernProvider concernProvider,
            IHttpContextAccessor contextAccessor,
            ITicketConcernChecker concernChecker)
        {
            this._factory = factory;
            this._factory
                .Set(ticketOwnerRepository)
                .Set(concernProvider)
                .Set(contextAccessor)
                .Set(concernChecker);
        }

        public TicketFactory GetFactory(TicketFactoryData data)
        {
            return this._factory.Set(data);
        }
    }
}