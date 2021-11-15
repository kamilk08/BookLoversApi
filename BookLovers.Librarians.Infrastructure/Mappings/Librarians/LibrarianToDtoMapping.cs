using AutoMapper;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.Librarians
{
    public class LibrarianToDtoMapping : Profile
    {
        public LibrarianToDtoMapping()
        {
            this.CreateMap<LibrarianReadModel, LibrarianDto>()
                .ForMember(dest => dest.ManagedTickets,
                    opt => opt.MapFrom(p => p.Tickets));
        }
    }
}