using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ResolvedTickets
{
    public class ResolvedTicketDecisionMapping : Profile
    {
        public ResolvedTicketDecisionMapping()
        {
            this.CreateMap<ResolvedTicketReadModel, Decision>()
                .ConstructUsing(c => new Decision(c.DecisionValue, c.DecisionName));
        }
    }
}