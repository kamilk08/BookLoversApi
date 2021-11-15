using AutoMapper;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Librarians.Infrastructure.Mappings.PromotionWaiters
{
    public class PromotionWaiterMapping : Profile
    {
        public PromotionWaiterMapping()
        {
            this.CreateMap<PromotionWaiterReadModel, PromotionWaiter>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.ReaderGuid))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.PromotionAvailability,
                    opt => opt.MapFrom(p => p));

            this.CreateMap<PromotionWaiter, PromotionWaiterReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.ReaderGuid))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.Status)).ForMember(
                    dest => dest.PromotionAvailability, opt => opt.MapFrom(p => p.PromotionAvailability.Value));

            this.CreateMap<PromotionWaiterReadModel, PromotionWaiterDto>()
                .ForMember(dest => dest.PromotionStatus,
                    opt => opt.MapFrom(p => p.PromotionAvailability));
        }
    }
}