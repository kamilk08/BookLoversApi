using AutoMapper;
using BookLovers.Publication.Infrastructure.Dtos;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            this.CreateMap<ReviewReadModel, int>()
                .ConstructUsing(p => p.Id);

            this.CreateMap<ReviewReadModel, ReviewDto>();
        }
    }
}