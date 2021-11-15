using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    internal class AuthorsMapping : Profile
    {
        public AuthorsMapping()
        {
            this.CreateMap<AuthorReadModel, AuthorDto>();
        }
    }
}