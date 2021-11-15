using AutoMapper;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings
{
    public class TicketOwnerMapping : Profile
    {
        public TicketOwnerMapping()
        {
            this.CreateMap<TicketOwner, TicketOwnerReadModel>();
            
            this.CreateMap<TicketOwnerReadModel, TicketOwner>()
                .ForMember(TicketOwner.Relations.CreatedTickets,
                opt => opt.MapFrom(p => p.Tickets));
            
            this.CreateMap<TicketOwnerReadModel, TicketOwnerDto>()
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId));
        }
    }
}