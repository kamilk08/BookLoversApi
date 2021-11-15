using AutoMapper;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class PasswordResetTokenMapping : Profile
    {
        public PasswordResetTokenMapping()
        {
            CreateMap<PasswordResetTokenReadModel, PasswordResetToken>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Email))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(p => p.Token))
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(p => p.UserGuid))
                .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(p => p.ExpiresAt));

            CreateMap<PasswordResetToken, PasswordResetTokenReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Email))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(p => p.Token))
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(p => p.UserGuid))
                .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(p => p.ExpiresAt));
        }
    }
}