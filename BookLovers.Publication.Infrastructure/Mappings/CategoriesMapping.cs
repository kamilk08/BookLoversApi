using AutoMapper;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    public class CategoriesMapping : Profile
    {
        public CategoriesMapping()
        {
            this.CreateMap<SubCategoryReadModel, SubCategoryDto>();
            this.CreateMap<CategoryReadModel, CategoryDto>();
        }
    }
}