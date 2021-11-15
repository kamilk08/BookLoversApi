using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.ResolvedTickets
{
    public class ResolvedTicketJustificationMapping : Profile
    {
        public ResolvedTicketJustificationMapping()
        {
            this.CreateMap<ResolvedTicketReadModel, DecisionJustification>()
                .ConstructUsing(c => new DecisionJustification(c.Justification, c.Date));
        }
    }
}