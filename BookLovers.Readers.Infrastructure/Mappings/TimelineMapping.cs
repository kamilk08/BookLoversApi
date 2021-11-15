using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class TimelineMapping : Profile
    {
        public TimelineMapping()
        {
            this.CreateMap<TimeLineReadModel, TimeLineDto>()
                .ForMember(
                    dest => dest.ActivitiesCount,
                    opt => opt.MapFrom(p => p.Actvities.Count));
        }
    }
}