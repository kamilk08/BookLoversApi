using AutoMapper;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketToDtoMapping : Profile
    {
        public TicketToDtoMapping()
        {
            this.CreateMap<TicketReadModel, TicketDto>()
                .ForMember(dest => dest.TicketDate, opt => opt.MapFrom(p => p.Date))
                .ForMember(dest => dest.TicketData, opt => opt.MapFrom(p => p.Content))
                .ForMember(dest => dest.TicketConcern, opt => opt.MapFrom(p => p.TicketConcernValue))
                .ForMember(dest => dest.SolvedByGuid, opt => opt.MapFrom(p => p.LibrarianGuid.GetValueOrDefault()))
                .ForMember(dest => dest.TicketOwnerGuid, opt => opt.MapFrom(p => p.TicketOwnerGuid))
                .ForMember(dest => dest.TicketOwnerId, opt => opt.MapFrom(p => p.TicketOwnerId))
                .ForMember(dest => dest.TicketState, opt => opt.MapFrom(p => p.TicketStateValue))
                .ForMember(dest => dest.TicketDecision, opt => opt.MapFrom(p => p.DecisionValue));
        }
    }
}