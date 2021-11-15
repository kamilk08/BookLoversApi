using AutoMapper;
using BookLovers.Auth.Domain.Audiences;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    public class AudienceMapping : Profile
    {
        public AudienceMapping()
        {
            CreateMap<Audience, AudienceReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(
                    dest => dest.AudienceGuid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(
                    dest => dest.AudienceName,
                    opt => opt.MapFrom(p => p.AudienceDetails.AudienceType.Name))
                .ForMember(
                    dest => dest.AudienceType,
                    opt => opt.MapFrom(p => p.AudienceDetails.AudienceType.Value))
                .ForMember(
                    dest => dest.Hash,
                    opt => opt.MapFrom(p => p.AudienceSecurity.Hash))
                .ForMember(
                    dest => dest.Salt,
                    opt => opt.MapFrom(p => p.AudienceSecurity.Salt))
                .ForMember(
                    dest => dest.RefreshTokenLifeTime,
                    opt => opt.MapFrom(p => p.AudienceDetails.RefreshTokenLifeTime))
                .ForMember(
                    dest => dest.AudienceState,
                    opt => opt.MapFrom(p => p.AudienceState.Value))
                .ForMember(
                    dest => dest.AudienceStateName,
                    opt => opt.MapFrom(p => p.AudienceState.Name))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(p => p.Status));

            CreateMap<AudienceReadModel, AudienceState>()
                .ConstructUsing(p => new AudienceState(p.AudienceState, p.AudienceStateName));

            CreateMap<AudienceReadModel, AudienceSecurity>()
                .ConstructUsing(p => new AudienceSecurity(p.Hash, p.Salt));

            CreateMap<AudienceReadModel, Audience>()
                .ForMember(
                    dest => dest.AudienceState,
                    opt => opt.MapFrom(p => p))
                .ForMember(
                    dest => dest.AudienceDetails,
                    opt => opt.MapFrom(p => new AudienceDetails(p.AudienceType, p.RefreshTokenLifeTime)))
                .ForMember(
                    dest => dest.AudienceSecurity, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AudienceGuid));
        }
    }
}