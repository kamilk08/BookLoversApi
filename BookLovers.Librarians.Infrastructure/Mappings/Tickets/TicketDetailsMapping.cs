using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketDetailsMapping : Profile
    {
        public TicketDetailsMapping()
        {
            this.CreateMap<TicketReadModel, TicketDetails>()
                .ConstructUsing(c => new TicketDetails(c.Title, c.Description, c.Date));
        }
    }
}