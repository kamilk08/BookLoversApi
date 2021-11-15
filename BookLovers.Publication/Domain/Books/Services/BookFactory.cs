using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Domain.PublisherCycles.BusinessRules;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookBuilder _bookBuilder;
        private readonly IBookReaderAccessor _bookReaderAccessor;

        private readonly List<Func<Book, Publisher, List<Author>, IBusinessRule>> _rules =
            new List<Func<Book, Publisher, List<Author>, IBusinessRule>>();

        public BookFactory(
            IUnitOfWork unitOfWork,
            BookBuilder bookBuilder,
            IsbnValidatorFactory validatorFactory,
            ITitleUniquenessChecker titleUniquenessChecker,
            IIsbnUniquenessChecker isbnUniquenessChecker,
            IBookReaderAccessor bookReaderAccessor)
        {
            this._bookBuilder = bookBuilder;
            this._unitOfWork = unitOfWork;
            this._bookReaderAccessor = bookReaderAccessor;

            this._rules.Add((book, publisher, authors) => new AggregateMustBeActive(book.AggregateStatus.Value));
            this._rules.Add((book, publisher, authors) =>
                new IsbnNumberMustBeValid(validatorFactory.GetValidator(book.Basics.ISBN), book.Basics.ISBN));

            this._rules.Add((book, publisher, authors) => new BooksCategoryMustBeValid(book.Basics.BookCategory));
            this._rules.Add((book, publisher, authors) => new BookShouldHaveAtleastOneAuthor(book.Authors));
            this._rules.Add((book, publisher, authors) => new BookCannotContainDuplicatedAuthors(book));
            this._rules.Add((book, publisher, authors) => new BookShouldHaveSelectedPublisher(book.Publisher));
            this._rules.Add((book, publisher, authors) => new CoverTypeMustBeValid(book.Cover.CoverType));
            this._rules.Add((book, publisher, authors) => new LanguageTypeMustBeValid(book.Details.Language));
            this._rules.Add((book, publisher, authors) => new PublisherMustBeAvailable(publisher));
            this._rules.Add((book, publisher, authors) => new EachAuthorMustBeAvailable(authors));

            this._rules.Add((book, publisher, authors) =>
                new TitleMustBeUnique(titleUniquenessChecker, book.Basics.Title));
            this._rules.Add((book, publisher, authors) =>
                new IsbnNumberMustBeUnique(isbnUniquenessChecker, book.Basics.ISBN));
        }

        public async Task<Book> CreateBook(BookData bookData)
        {
            var book = this._bookBuilder
                .InitializeBook(bookData.BookGuid, bookData.Authors, bookData.PublisherGuid,
                    this.SetupBookBasics(bookData.BasicsData))
                .AddDetails(bookData.DetailsData.Pages, bookData.DetailsData.Language)
                .AddDescription(bookData.DescriptionData.Content, bookData.DescriptionData.DescriptionSource)
                .AddSeries(bookData.SeriesData.SeriesGuid, bookData.SeriesData.PositionInSeries)
                .AddCover(bookData.CoverData.CoverType, bookData.CoverData.CoverSource)
                .AddHashTags(bookData.BookHashTags)
                .Build();

            var publisher = await this._unitOfWork.GetAsync<Publisher>(bookData.PublisherGuid);
            var authors = await this._unitOfWork.GetMultipleAsync<Author>(bookData.Authors);

            await this.CheckIfSeriesExistAsync(book);
            await this.CheckIfCyclesExists(bookData);
            await this.CheckIfBookReaderExistsAsync(bookData.AddedByGuid);

            foreach (var rule in this._rules)
            {
                if (!rule(book, publisher, authors).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book, publisher, authors).BrokenRuleMessage);
            }

            book.ApplyChange(this.CreateEvent(book, bookData.Cycles, bookData.AddedByGuid));

            return book;
        }

        private async Task CheckIfCyclesExists(BookData bookData)
        {
            var cycles = await this._unitOfWork.GetMultipleAsync<PublisherCycle>(bookData.Cycles);

            foreach (var sourcedAggregateRoot in cycles)
            {
                if (sourcedAggregateRoot.Guid == Guid.Empty)
                    throw new BusinessRuleNotMetException("Cycle does not exist.");
            }
        }

        private async Task CheckIfBookReaderExistsAsync(Guid addedByGuid)
        {
            var bookReader = await this._bookReaderAccessor.GetAggregateGuidAsync(addedByGuid);

            if (bookReader.IsEmpty())
                throw new BusinessRuleNotMetException("Book reader does not exist.");
        }

        private async Task CheckIfSeriesExistAsync(Book book)
        {
            if (book.Series.SeriesGuid != Guid.Empty)
            {
                var series = await _unitOfWork.GetAsync<Series>(book.Series.SeriesGuid.GetValueOrDefault());
                if (series.Guid == Guid.Empty)
                    throw new BusinessRuleNotMetException($"Series does not exist.");
            }
        }

        private BookBasics SetupBookBasics(BookBasicsData basicsData)
        {
            return new BookBasics(
                basicsData.Isbn,
                basicsData.Title,
                basicsData.PublicationDate, basicsData.BookCategory);
        }

        private BookCreated CreateEvent(Book book, List<Guid> cycles, Guid addedBy)
        {
            return BookCreated
                .Initialize().WithAggregate(book.Guid)
                .WithAuthors(book.Authors.Select(s => s.AuthorGuid))
                .WithTitleAndIsbn(book.Basics.Title, book.Basics.ISBN)
                .WithPublisher(book.Publisher.PublisherGuid)
                .WithAddedBy(addedBy)
                .WithPublicationDate(book.Basics.PublicationDate)
                .WithCategory(book.Basics.BookCategory.Category.Value, book.Basics.BookCategory.Category.Name)
                .WithSubCategory(book.Basics.BookCategory.SubCategory.Value, book.Basics.BookCategory.SubCategory.Name)
                .WithDescription(book.Description?.BookDescription, book.Description?.DescriptionSource)
                .WithDetails(book.Details?.Pages, book.Details?.Language?.Value, book.Details?.Language?.Name)
                .WithSeries(book.Series.SeriesGuid, book.Series.PositionInSeries)
                .WithCover(book.Cover.CoverSource, book.Cover.CoverType.Value, book.Cover.CoverType.Name)
                .WithHashTags(book.HashTags.Select(s => s.HashTagContent).ToList())
                .WithCycles(cycles);
        }
    }
}