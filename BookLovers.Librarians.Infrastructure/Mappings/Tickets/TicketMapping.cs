using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketMapping : Profile
    {
        public TicketMapping()
        {
            this.CreateMap<Ticket, TicketReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(p => p.TicketContent.Content))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(p => p.TicketDetails.Date))
                .ForMember(dest => dest.Decision, opt => opt.MapFrom(p => p.Decision.Name))
                .ForMember(dest => dest.DecisionValue, opt => opt.MapFrom(p => p.Decision.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(p => p.TicketDetails.Description))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(p => p.TicketDetails.Title))
                .ForMember(dest => dest.LibrarianGuid, opt => opt.MapFrom(p => p.SolvedBy.LibrarianGuid))
                .ForMember(dest => dest.TicketConcern, opt => opt.MapFrom(p => p.TicketContent.TicketConcern.Name))
                .ForMember(dest => dest.TicketConcernValue,
                    opt => opt.MapFrom(p => p.TicketContent.TicketConcern.Value))
                .ForMember(dest => dest.TicketOwnerGuid, opt => opt.MapFrom(p => p.IssuedBy.TicketOwnerGuid))
                .ForMember(dest => dest.TicketOwnerId, opt => opt.MapFrom(p => p.IssuedBy.TicketOwnerId))
                .ForMember(dest => dest.TicketObjectGuid, opt => opt.MapFrom(p => p.TicketContent.TicketObjectGuid));

            this.CreateMap<TicketReadModel, Ticket>().ForMember(dest => dest.Decision, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.IssuedBy, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.SolvedBy, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.TicketContent,
                    opt => opt.MapFrom(p => new TicketContent(p.TicketObjectGuid, p.Content,
                        new TicketConcern(p.TicketConcernValue, p.TicketConcern))))
                .ForMember(dest => dest.TicketDetails, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.TicketState, opt => opt.MapFrom(p => p));
        }
    }
}