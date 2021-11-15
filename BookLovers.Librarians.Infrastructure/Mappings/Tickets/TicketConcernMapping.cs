using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketConcernMapping : Profile
    {
        public TicketConcernMapping()
        {
            this.CreateMap<TicketConcern, TicketConcernReadModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(p => p.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(p => p.Value));

            this.CreateMap<TicketConcernReadModel, TicketConcern>()
                .ConstructUsing(c => new TicketConcern(c.Value, c.Name));
        }
    }
}