using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class TimelineActivityMapping : Profile
    {
        public TimelineActivityMapping()
        {
            this.CreateMap<TimeLineActivityReadModel, TimeLineActivityDto>();
        }
    }
}