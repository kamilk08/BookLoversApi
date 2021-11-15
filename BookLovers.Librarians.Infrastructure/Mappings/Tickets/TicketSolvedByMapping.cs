using AutoMapper;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Tickets
{
    public class TicketSolvedByMapping : Profile
    {
        public TicketSolvedByMapping()
        {
            this.CreateMap<TicketReadModel, SolvedBy>()
                .ConstructUsing(p => new SolvedBy(p.LibrarianGuid.GetValueOrDefault()));
        }
    }
}