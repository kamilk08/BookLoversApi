using AutoMapper;
using BookLovers.Auth.Domain.Tokens;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class TokenMapping : Profile
    {
        public TokenMapping()
        {
            CreateMap<RefreshToken, RefreshTokenReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.TokenGuid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(
                    dest => dest.AudienceGuid,
                    opt => opt.MapFrom(p => p.TokenIdentification.AudienceGuid))
                .ForMember(
                    dest => dest.UserGuid,
                    opt => opt.MapFrom(p => p.TokenIdentification.UserGuid))
                .ForMember(
                    dest => dest.IssuedAt,
                    opt => opt.MapFrom(p => p.TokenDetails.IssuedAt))
                .ForMember(
                    dest => dest.Expires,
                    opt => opt.MapFrom(p => p.TokenDetails.ExpiresAt))
                .ForMember(
                    dest => dest.RevokedAt,
                    opt => opt.MapFrom(p => p.TokenDetails.RevokedAt))
                .ForMember(
                    dest => dest.ProtectedTicket,
                    opt => opt.MapFrom(p => p.TokenDetails.ProtectedTicket))
                .ForMember(
                    dest => dest.Hash,
                    opt => opt.MapFrom(p => p.TokenSecurity.Hash))
                .ForMember(
                    dest => dest.Salt,
                    opt => opt.MapFrom(p => p.TokenSecurity.Salt))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(p => p.Status));

            CreateMap<RefreshTokenReadModel, RefreshTokenIdentification>()
                .ConstructUsing(p => new RefreshTokenIdentification(p.UserGuid, p.AudienceGuid));

            CreateMap<RefreshTokenReadModel, RefreshTokenDetails>()
                .ConstructUsing(p => new RefreshTokenDetails(p.IssuedAt, p.Expires, p.ProtectedTicket));

            CreateMap<RefreshTokenReadModel, RefreshTokenSecurity>()
                .ConstructUsing(p => new RefreshTokenSecurity(p.Hash, p.Salt));

            CreateMap<RefreshTokenReadModel, RefreshToken>()
                .ForMember(
                    dest => dest.TokenIdentification,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.TokenDetails,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.TokenSecurity,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(p => p.Id))
                .ForMember(
                    dest => dest.Guid,
                    opt => opt.MapFrom(p => p.TokenGuid))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(p => p.Status));
        }
    }
}