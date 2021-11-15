using AutoMapper;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    public class BookcaseSettingsMapping : Profile
    {
        public BookcaseSettingsMapping()
        {
            CreateMap<SettingsManagerReadModel, BookcaseOptionsDto>()
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(p => p.Capacity))
                .ForMember(dest => dest.Privacy, opt => opt.MapFrom(p => p.Privacy));
        }
    }
}