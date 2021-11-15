using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketIssuedByMapping : Profile
    {
        public TicketIssuedByMapping()
        {
            this.CreateMap<TicketReadModel, IssuedBy>()
                .ConstructUsing(p => new IssuedBy(p.TicketOwnerId, p.TicketOwnerGuid));
        }
    }
}