using AutoMapper;
using BookLovers.Readers.Events.Reviews;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Mappings
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            this.CreateMap<ReviewCreated, ReviewReadModel>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Review, opt => opt.MapFrom(p => p.Review))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(p => p.CreatedAt))
                .ForMember(dest => dest.EditedDate, opt => opt.MapFrom(p => p.EditDate))
                .ForMember(dest => dest.Reader, opt => opt.Ignore())
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(p => p.Likes))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.ReviewStatus))
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.MarkedAsSpoilerByReader, opt => opt.MapFrom(p => p.MarkedAsSpoiler))
                .ForMember(dest => dest.MarkedAsSpoilerByOthers, opt => opt.MapFrom(p => p.MarkedAsSpoilerByOthers));

            this.CreateMap<ReviewReadModel, int>().ConvertUsing(p => p.Id);

            this.CreateMap<ReviewReadModel, ReviewDto>()
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.Guid))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(p => p.CreateDate))
                .ForMember(dest => dest.BookGuid, opt => opt.MapFrom(p => p.Book.BookGuid))
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.Reader.Guid))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.Reader.ReaderId))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(p => p.Book.BookId))
                .ForMember(dest => dest.MarkedAsSpoiler, opt => opt.MapFrom(p => p.MarkedAsSpoilerByReader))
                .ForMember(dest => dest.MarkedAsSpoilerByOthers, opt => opt.MapFrom(p => p.MarkedAsSpoilerByOthers))
                .ForMember(dest => dest.AddedBy, opt => opt.MapFrom(p => p.Reader.UserName));

            this.CreateMap<ReviewLikeReadModel, int>().ConvertUsing(p => p.Id);

            this.CreateMap<ReviewLikeReadModel, ReviewLikeDto>()
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.ReaderId));

            this.CreateMap<ReviewReportReadModel, ReviewReportDto>();
        }
    }
}