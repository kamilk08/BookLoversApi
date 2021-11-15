using System.Collections.Generic;
using AutoMapper;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mappings
{
    internal class BookMapping : Profile
    {
        public BookMapping()
        {
            this.CreateMap<BookCreated, BookReadModel>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(p => p.BooksCategory))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(p => p.CategoryName))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(p => p.SubCategoryId))
                .ForMember(dest => dest.SubCategory, opt => opt.MapFrom(p => p.SubCategoryName))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(p => p.AggregateGuid))
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(p => p.BookDescription))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(p => p.Language))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(p => p.LanguageId))
                .ForMember(dest => dest.CoverType, opt => opt.MapFrom(p => p.CoverType))
                .ForMember(dest => dest.CoverTypeId, opt => opt.MapFrom(p => p.CoverTypeId))
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Reader, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(p => p.BookStatus))
                .ForMember(
                    dest => dest.HashTags,
                    opt => opt.MapFrom(p => JsonConvert.SerializeObject(p.HashTags)));

            this.CreateMap<BookReadModel, int>().ConstructUsing(p => p.Id);

            this.CreateMap<BookReadModel, BookDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(p => p.Category))
                .ForMember(dest => dest.ReaderId, opt => opt.MapFrom(p => p.Reader.ReaderId))
                .ForMember(dest => dest.BookStatus, opt => opt.MapFrom(p => p.Status))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(p => p.Authors))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(p => p.Language))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(p => p.SubCategory))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(p => p.Reviews))
                .ForMember(dest => dest.ReaderGuid, opt => opt.MapFrom(p => p.Reader.ReaderGuid)).ForMember(
                    dest => dest.BookHashTags,
                    opt => opt.MapFrom(p => JsonConvert.DeserializeObject<IList<string>>(p.HashTags)));
        }
    }
}