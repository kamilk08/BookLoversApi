using AutoMapper;
using BookLovers.Bookcases.Events.Shelf;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Mappings
{
    public class ShelfMapping : Profile
    {
        public ShelfMapping()
        {
            CreateMap<CustomShelfCreated, ShelfReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.ShelfGuid))
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ForMember(dest => dest.Bookcase, opt => opt.Ignore());

            CreateMap<CoreShelfCreated, ShelfReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.ShelfGuid))
                .ForMember(dest => dest.Bookcase, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            CreateMap<ShelfReadModel, int>().ConvertUsing(src => src.Id);

            CreateMap<ShelfReadModel, ShelfDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Publications, opt => opt.MapFrom(p => p.Books))
                .ForMember(dest => dest.ShelfName, opt => opt.MapFrom(p => p.ShelfName))
                .ForMember(
                    dest => dest.PublicationsCount,
                    opt => opt.MapFrom(p => p.Books.Count));
        }
    }
}