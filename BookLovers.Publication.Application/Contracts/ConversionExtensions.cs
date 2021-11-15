using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Application.Contracts
{
    internal static class ConversionExtensions
    {
        internal static BookData ConvertToBookData(this BookWriteModel dto)
        {
            var category = new BookCategory(dto.Basics.Category, dto.Basics.SubCategory);

            var basicsData = BookBasicsData
                .Initialize()
                .WithTitle(dto.Basics.Title)
                .WithIsbn(dto.Basics.Isbn)
                .WithCategory(category)
                .WithDate(dto.Basics.PublicationDate);

            var bookData = BookData
                .Initialize(dto.BookGuid, dto.Authors)
                .WithBasics(basicsData, dto.Basics.PublisherGuid);

            var detailsData = new BookDetailsData(
                dto.Details.Pages.GetValueOrDefault(),
                dto.Details.Language.GetValueOrDefault());

            return bookData
                .WithDetails(detailsData)
                .WithDescription(new BookDescriptionData(dto.Description.Content, dto.Description.DescriptionSource))
                .WithSeries(new BookSeriesData(
                    dto.SeriesGuid.GetValueOrDefault(),
                    dto.PositionInSeries.GetValueOrDefault()))
                .WithCycles(dto.Cycles)
                .WithHashTags(dto.HashTags)
                .WithCover(new BookCoverData(dto.Cover.CoverType, dto.Cover.CoverSource)).AddedBy(dto.AddedBy);
        }

        internal static AuthorData ConvertToAuthorData(
            this AuthorWriteModel dto,
            BookReader bookReader)
        {
            var authorData = AuthorData.Initialize()
                .WithBasics(new FullName(dto.Basics.FirstName, dto.Basics.SecondName), Sexes.Get(dto.Basics.Sex));
            var lifeLength = new LifeLength(
                dto.Details.BirthDate.GetValueOrDefault(),
                dto.Details.DeathDate.GetValueOrDefault());

            return authorData.WithDetails(lifeLength, dto.Details.BirthPlace)
                .WithDescription(dto.Description.AboutAuthor, dto.Description.DescriptionSource,
                    dto.Description.WebSite)
                .WithGuid(dto.AuthorGuid)
                .AddedBy(bookReader)
                .WithBooks(dto.AuthorBooks)
                .WithGenres(dto.AuthorGenres);
        }
    }
}