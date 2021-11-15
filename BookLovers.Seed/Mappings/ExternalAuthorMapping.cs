using System.Linq;
using AutoMapper;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.OpenLibrary.Authors;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Seed.Mappings
{
    public class ExternalAuthorMapping : Profile
    {
        public ExternalAuthorMapping()
        {
            CreateMap<AuthorRoot, SeedAuthor>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(p => Sex.Hidden.Value))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(p => p.Name))
                .ForMember(dest => dest.DescriptionSource, opt => opt
                    .MapFrom(p => p.Links.FirstOrDefault()
                                  != default
                        ? p.Links.FirstOrDefault().Url
                        : default))
                .ForMember(dest => dest.AboutAuthor, opt => opt
                    .MapFrom(p => p.Bio == default ? string.Empty : p.Bio.Value))
                .ForMember(dest => dest.WebSite, opt => opt.MapFrom(p =>
                    p.Links.FirstOrDefault() != default
                        ? p.Links.FirstOrDefault().Url
                        : default));
        }
    }
}