using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos.Followers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class FollowMapping : Profile
    {
        public FollowMapping()
        {
            this.CreateMap<FollowReadModel, int>()
                .ConstructUsing(p => p.Id);

            this.CreateMap<FollowReadModel, FollowDto>()
                .ForMember(dest => dest.Reader, opt => opt.MapFrom(p => p.Followed));
        }
    }
}