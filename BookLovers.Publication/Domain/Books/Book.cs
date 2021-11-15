using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Mementos;

namespace BookLovers.Publication.Domain.Books
{
    [AllowSnapshot]
    public class Book :
        EventSourcedAggregateRoot,
        IHandle<BookCreated>,
        IHandle<AuthorAdded>,
        IHandle<BookBasicsChanged>,
        IHandle<BookDescriptionChanged>,
        IHandle<BookDetailsChanged>,
        IHandle<BookArchived>,
        IHandle<AuthorRemoved>,
        IHandle<PublisherChanged>,
        IHandle<SeriesChanged>,
        IHandle<BookSeriesPositionChanged>,
        IHandle<CoverChanged>,
        IHandle<BookReviewAdded>,
        IHandle<BookReviewRemoved>,
        IHandle<BookHashTagsChanged>,
        IHandle<QuoteAddedToBook>,
        IHandle<QuoteRemovedFromBook>,
        IHandle<BookHasNoSeries>
    {
        private List<BookAuthor> _authors = new List<BookAuthor>();
        private List<BookReview> _reviews = new List<BookReview>();
        internal List<BookHashTag> _bookHashTags = new List<BookHashTag>();
        private List<BookQuote> _bookQuotes = new List<BookQuote>();

        public IReadOnlyList<BookAuthor> Authors => this._authors;

        public IReadOnlyList<BookReview> Reviews => this._reviews;

        public IReadOnlyList<BookHashTag> HashTags => this._bookHashTags;

        public IReadOnlyList<BookQuote> BookQuotes => this._bookQuotes;

        public BookBasics Basics { get; private set; }

        public BookPublisher Publisher { get; private set; }

        public BookSeries Series { get; internal set; }

        public Description Description { get; internal set; }

        public Cover Cover { get; internal set; }

        public BookDetails Details { get; internal set; }

        private Book()
        {
        }

        internal Book(Guid bookGuid, List<BookAuthor> authors, Guid publisherGuid,
            BookBasics basics)
        {
            this.Guid = bookGuid;
            this._authors = authors;
            this.Basics = basics;
            this.Publisher = new BookPublisher(publisherGuid);
            this.AggregateStatus = AggregateStatus.Active;
        }

        public void AddAuthor(BookAuthor bookAuthor)
        {
            this.CheckBusinessRules(new AddBookAuthorRules(this, bookAuthor));

            this.ApplyChange(new AuthorAdded(this.Guid, bookAuthor.AuthorGuid));
        }

        public void RemoveAuthor(BookAuthor bookAuthor)
        {
            this.CheckBusinessRules(new RemoveAuthorRules(this, bookAuthor));

            var unknownAuthor = this.DoesBelongToUnknownAuthor();

            this.ApplyChange(new AuthorRemoved(this.Guid, bookAuthor.AuthorGuid, unknownAuthor));
        }

        public void AddReview(BookReview bookReview)
        {
            this.CheckBusinessRules(new AddBookReviewRules(this, bookReview));

            this.ApplyChange(new BookReviewAdded(this.Guid, bookReview.ReviewGuid, bookReview.ReaderGuid));
        }

        public void RemoveReview(BookReview bookReview)
        {
            this.CheckBusinessRules(new RemoveBookReviewRules(this, bookReview));

            this.ApplyChange(new BookReviewRemoved(this.Guid, bookReview.ReviewGuid, bookReview.ReaderGuid));
        }

        public void ChangePublisher(BookPublisher bookPublisher)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));

            this.ApplyChange(new PublisherChanged(this.Guid, bookPublisher.PublisherGuid,
                this.Publisher.PublisherGuid));
        }

        public void ChangeSeries(BookSeries bookSeries)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));
            IEvent @event = null;

            if (bookSeries.SeriesGuid == Guid.Empty)
                @event = new BookHasNoSeries(this.Guid, this.Series.SeriesGuid.GetValueOrDefault());
            else if (SeriesIsDifferent(bookSeries))
            {
                @event = SeriesChanged.Initialize()
                    .WithAggregate(this.Guid)
                    .WithNewSeries(
                        bookSeries.SeriesGuid.Value,
                        bookSeries.PositionInSeries.Value)
                    .WithOldSeries(this.Series.SeriesGuid.Value);
            }
            else
                @event = new BookSeriesPositionChanged(
                    this.Guid,
                    bookSeries.SeriesGuid.Value,
                    bookSeries.PositionInSeries.Value);

            ApplyChange(@event);
        }

        public void AddBookQuote(BookQuote bookQuote)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));

            this.ApplyChange(new QuoteAddedToBook(this.Guid, bookQuote.QuoteGuid));
        }

        public void RemoveBookQuote(BookQuote bookQuote)
        {
            this.CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus.Value));

            this.ApplyChange(new QuoteRemovedFromBook(this.Guid, bookQuote.QuoteGuid));
        }

        public BookReview GetBookReview(Guid readerGuid)
        {
            return this._reviews.Find(p => p.ReaderGuid == readerGuid);
        }

        public BookAuthor GetBookAuthor(Guid authorGuid)
        {
            return this._authors.Find(p => p.AuthorGuid == authorGuid);
        }

        public BookQuote GetBookQuote(Guid quoteGuid)
        {
            return this._bookQuotes.Find(p => p.QuoteGuid == quoteGuid);
        }

        internal bool DoesBelongToUnknownAuthor()
        {
            return this._authors.Count - 1 == 0;
        }

        private bool SeriesIsDifferent(BookSeries bookSeries)
        {
            return this.Series.SeriesGuid != bookSeries.SeriesGuid;
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var bookMemento = memento as IBookMemento;
            this.Guid = bookMemento.AggregateGuid;
            this.AggregateStatus = AggregateStates.Get(bookMemento.AggregateStatus);
            this.Version = bookMemento.Version;
            this.LastCommittedVersion = bookMemento.LastCommittedVersion;

            this._authors = bookMemento.Authors.Select(s => new BookAuthor(s)).ToList();
            this._reviews = bookMemento.BookReviews.Select(p => new BookReview(p.Key, p.Value)).ToList();

            this.Publisher = new BookPublisher(bookMemento.PublisherGuid);
            this.Series = new BookSeries(bookMemento.SeriesGuid, bookMemento.PositionInSeries);
            this.Basics = new BookBasics(bookMemento.Isbn, bookMemento.Title, bookMemento.PublicationDate,
                bookMemento.BookCategory, bookMemento.BookSubCategory);
            this.Description = new Description(bookMemento.Description, bookMemento.DescriptionSource);
            this.Cover = new Cover(bookMemento.CoverType, bookMemento.CoverSource);
            this.Details = new BookDetails(bookMemento.Pages, bookMemento.Language);

            this._bookQuotes = bookMemento.Quotes.Select(s => new BookQuote(s)).ToList();
        }

        void IHandle<AuthorAdded>.Handle(AuthorAdded @event)
        {
            this._authors.Add(new BookAuthor(@event.AuthorGuid));
        }

        void IHandle<BookBasicsChanged>.Handle(BookBasicsChanged @event)
        {
            this.Basics = new BookBasics(
                @event.Isbn,
                @event.Title, @event.PublicationDate, @event.CategoryId, @event.SubCategoryId);
        }

        void IHandle<BookCreated>.Handle(BookCreated @event)
        {
            this.Guid = @event.AggregateGuid;
            this._authors = @event.BookAuthors.Select(s => new BookAuthor(s)).ToList();

            this.Publisher = new BookPublisher(@event.PublisherGuid);
            this.Series = new BookSeries(@event.SeriesGuid, @event.PositionInSeries);
            this.Basics = new BookBasics(@event.Isbn, @event.Title, @event.PublicationDate, @event.BooksCategory,
                @event.SubCategoryId);
            this.Details = new BookDetails(@event.Pages, @event.LanguageId);
            this.Description = new Description(@event.BookDescription, @event.DescriptionSource);
            this.Cover = new Cover(@event.CoverTypeId, @event.CoverSource);
            this._bookHashTags = @event.HashTags.Select(s => new BookHashTag(s)).ToList();
            this.AggregateStatus = AggregateStatus.Active;

            this._bookQuotes = new List<BookQuote>();
        }

        void IHandle<BookDescriptionChanged>.Handle(BookDescriptionChanged @event)
        {
            this.Description = new Description(@event.Description, @event.DescriptionSource);
        }

        void IHandle<BookDetailsChanged>.Handle(BookDetailsChanged @event)
        {
            this.Details = new BookDetails(
                @event.Pages.GetValueOrDefault(),
                @event.Language.GetValueOrDefault());
        }

        void IHandle<BookArchived>.Handle(BookArchived @event)
        {
            this.AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<AuthorRemoved>.Handle(AuthorRemoved @event)
        {
            var bookAuthor = this._authors.Find(p => p.AuthorGuid == @event.AuthorGuid);

            this._authors.Remove(bookAuthor);
        }

        void IHandle<PublisherChanged>.Handle(PublisherChanged @event)
        {
            this.Publisher = new BookPublisher(@event.PublisherGuid);
        }

        void IHandle<SeriesChanged>.Handle(SeriesChanged @event)
        {
            this.Series = new BookSeries(@event.SeriesGuid, @event.PositionInSeries);
        }

        void IHandle<CoverChanged>.Handle(CoverChanged @event)
        {
            this.Cover = new Cover(@event.CoverType, @event.PictureSource);
        }

        void IHandle<BookReviewAdded>.Handle(BookReviewAdded @event)
        {
            this._reviews.Add(new BookReview(@event.ReaderGuid, @event.ReviewGuid));
        }

        void IHandle<BookReviewRemoved>.Handle(BookReviewRemoved @event)
        {
            var review = this._reviews.Find(p => p.ReaderGuid == @event.ReaderGuid);

            this._reviews.Remove(review);
        }

        void IHandle<BookHashTagsChanged>.Handle(BookHashTagsChanged @event)
        {
            this._bookHashTags = @event.HashTags.Select(s => new BookHashTag(s))
                .ToList();
        }

        void IHandle<QuoteAddedToBook>.Handle(QuoteAddedToBook @event)
        {
            this._bookQuotes.Add(new BookQuote(@event.QuoteGuid));
        }

        void IHandle<QuoteRemovedFromBook>.Handle(QuoteRemovedFromBook @event)
        {
            var quote = this._bookQuotes.Single(p => p.QuoteGuid == @event.QuoteGuid);

            this._bookQuotes.Remove(quote);
        }

        void IHandle<BookSeriesPositionChanged>.Handle(BookSeriesPositionChanged @event)
        {
            this.Series = new BookSeries(this.Series.SeriesGuid, @event.Position);
        }

        void IHandle<BookHasNoSeries>.Handle(BookHasNoSeries @event)
        {
            this.Series = new BookSeries(Guid.Empty, 0);
        }
    }
}