using AutoMapper;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings
{
    public class CreatedTicketMapping : Profile
    {
        public CreatedTicketMapping()
        {
            this.CreateMap<CreatedTicket, CreatedTicketReadModel>();
            this.CreateMap<CreatedTicketReadModel, CreatedTicket>();
        }
    }
}