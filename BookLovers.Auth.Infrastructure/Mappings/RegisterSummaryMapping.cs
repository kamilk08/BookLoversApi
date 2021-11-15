using AutoMapper;
using BookLovers.Auth.Domain.RegistrationSummaries;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Mappings
{
    internal class RegisterSummaryMapping : Profile
    {
        public RegisterSummaryMapping()
        {
            CreateMap<RegistrationSummary, RegistrationSummaryReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Identification.Email))
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(p => p.Identification.UserGuid))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(p => p.Completion.CreatedAt))
                .ForMember(dest => dest.ExpiredAt, opt => opt.MapFrom(p => p.Completion.ExpiresAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(p => p.Completion.CompletedAt))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status));

            CreateMap<RegistrationSummaryReadModel, RegistrationIdentification>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(p => p.Email))
                .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(p => p.UserGuid));

            CreateMap<RegistrationSummaryReadModel, RegistrationCompletion>()
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(p => p.CompletedAt))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(p => p.CreateAt))
                .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(p => p.ExpiredAt));

            CreateMap<RegistrationSummaryReadModel, RegistrationSummary>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Completion, opt => opt.MapFrom(p => p))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(p => p));
        }
    }
}