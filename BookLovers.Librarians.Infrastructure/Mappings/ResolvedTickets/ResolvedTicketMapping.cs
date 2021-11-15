using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ResolvedTickets
{
    public class ResolvedTicketMapping : Profile
    {
        public ResolvedTicketMapping()
        {
            this.CreateMap<ResolvedTicketReadModel, ResolvedTicket>()
                .ForMember(dest => dest.Decision, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.Justification, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.TicketGuid,
                    opt => opt.MapFrom(p => p.TicketGuid));

            this.CreateMap<ResolvedTicket, ResolvedTicketReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.TicketGuid, opt => opt.MapFrom(p => p.TicketGuid))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(p => p.Justification.Date));
        }
    }
}