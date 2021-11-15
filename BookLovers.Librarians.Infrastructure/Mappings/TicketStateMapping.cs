using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings
{
    public class TicketStateMapping : Profile
    {
        public TicketStateMapping()
        {
            this.CreateMap<TicketReadModel, TicketState>()
                .ConstructUsing(c => new TicketState(c.TicketStateValue, c.TicketState));
        }
    }
}