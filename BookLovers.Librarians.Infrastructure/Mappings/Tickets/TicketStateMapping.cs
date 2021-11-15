using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketStateMapping : Profile
    {
        public TicketStateMapping()
        {
            this.CreateMap<TicketReadModel, TicketState>()
                .ConstructUsing(p => new TicketState(p.TicketStateValue, p.TicketState));
        }
    }
}