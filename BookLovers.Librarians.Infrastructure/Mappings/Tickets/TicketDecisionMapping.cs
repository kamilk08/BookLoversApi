using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketDecisionMapping : Profile
    {
        public TicketDecisionMapping()
        {
            this.CreateMap<TicketReadModel, Decision>()
                .ConstructUsing(p => new Decision(p.DecisionValue, p.Decision));
        }
    }
}