using System.Linq;
using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.EventHandlers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            this.CreateMap<ProfileReadModel, ProfileDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.Reader.ReaderId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(p => p.FullName.Split(new char[] { ' ' }).First()))
                .ForMember(dest => dest.SecondName, opt => opt.MapFrom(p => p.FullName.Split(new char[] { ' ' }).Last()))
                .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(p => p.JoinedAt))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(p => p.CurrentRole));

            this.CreateMap<AvatarChanged, AvatarReadModel>();
        }
    }
}