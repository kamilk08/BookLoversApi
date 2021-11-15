using AutoMapper;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Librarians
{
    public class LibrarianMapping : Profile
    {
        public LibrarianMapping()
        {
            this.CreateMap<Librarian, LibrarianReadModel>()
                .ForMember(dest => dest.Tickets,
                    opt => opt.MapFrom(p => p.Tickets));

            this.CreateMap<LibrarianReadModel, Librarian>()
                .ForMember(
                Librarian.Relations.ResolvedTickets,
                opt => opt.MapFrom(p => p.Tickets));
        }
    }
}