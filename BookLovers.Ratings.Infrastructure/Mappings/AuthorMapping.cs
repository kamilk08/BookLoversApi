using AutoMapper;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Mappings
{
    internal class AuthorMapping : Profile
    {
        public AuthorMapping()
        {
            CreateMap<Author, AuthorReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.AuthorGuid, opt => opt.MapFrom(p => p.Identification.AuthorGuid))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(p => p.Identification.AuthorId))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(Author.Relations.Books));

            CreateMap<AuthorReadModel, AuthorIdentification>()
                .ConstructUsing(p => new AuthorIdentification(p.AuthorGuid, p.AuthorId));

            CreateMap<AuthorReadModel, Author>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p))
                .ForMember(Author.Relations.Books, opt => opt.MapFrom(p => p.Books));

            CreateMap<AuthorReadModel, RatingsDto>()
                .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(p => p.AuthorId))
                .ForMember(dest => dest.Average, opt => opt.MapFrom(p => p.Average))
                .ForMember(dest => dest.RatingsCount, opt => opt.MapFrom(p => p.RatingsCount));

            CreateMap<AuthorReadModel, AuthorDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Average, opt => opt.MapFrom(p => p.Average));
        }
    }
}