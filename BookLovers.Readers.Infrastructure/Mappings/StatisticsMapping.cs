using AutoMapper;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    internal class StatisticsMapping : Profile
    {
        public StatisticsMapping()
        {
            this.CreateMap<StatisticsReadModel, ReaderStatisticsDto>()
                .ForMember(dest => dest.Followers, opt => opt.MapFrom(p => p.FollowersCount))
                .ForMember(dest => dest.Followings, opt => opt.MapFrom(p => p.FollowingsCount))
                .ForMember(dest => dest.AddedAuthors, opt => opt.MapFrom(p => p.AddedAuthors))
                .ForMember(dest => dest.AddedBooks, opt => opt.MapFrom(p => p.AddedBooks))
                .ForMember(dest => dest.AddedQuotes, opt => opt.MapFrom(p => p.AddedQuotes))
                .ForMember(dest => dest.GivenLikes, opt => opt.MapFrom(p => p.GivenLikes))
                .ForMember(dest => dest.ReceivedLikes, opt => opt.MapFrom(p => p.ReceivedLikes))
                .ForMember(dest => dest.GivenLikes, opt => opt.MapFrom(p => p.GivenLikes))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.Reader.ReaderId));
        }
    }
}