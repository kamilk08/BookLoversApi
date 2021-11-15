using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Builders;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookBuilder : IBuilder<Book>, IBuilder
    {
        private readonly ISet<Action<Book>> _builderActions =
            new HashSet<Action<Book>>();

        public Book Book { get; private set; }

        public IReadOnlyCollection<Action<Book>> BuilderActions
        {
            get { return this._builderActions.ToList(); }
        }

        internal BookBuilder InitializeBook(
            Guid bookGuid,
            List<Guid> authors,
            Guid publisherGuid,
            BookBasics bookBasics)
        {
            this.Book = new Book(bookGuid, authors
                .Select(s => new BookAuthor(s)).ToList(), publisherGuid, bookBasics);

            return this;
        }

        internal BookBuilder AddSeries(Guid? seriesGuid, int? positionInSeries)
        {
            this._builderActions.Add(book => book.Series = new BookSeries(seriesGuid, positionInSeries));

            return this;
        }

        internal BookBuilder AddDescription(string description, string sourceDescription)
        {
            this._builderActions.Add(book => book.Description = new Description(description, sourceDescription));

            return this;
        }

        internal BookBuilder AddDetails(int? pages, Language language)
        {
            this._builderActions.Add(book => book.Details = new BookDetails(pages.GetValueOrDefault(), language));

            return this;
        }

        internal BookBuilder AddCover(CoverType coverType, string pictureSource)
        {
            this._builderActions.Add(book => book.Cover = new Cover(coverType, pictureSource));

            return this;
        }

        internal BookBuilder AddHashTags(IList<string> hashTags)
        {
            var bookHashTags = hashTags.Select(s => new BookHashTag(s)).ToList();

            this._builderActions.Add(book => book._bookHashTags = bookHashTags);

            return this;
        }

        public Book Build()
        {
            if (this.Book == null)
                throw new ArgumentNullException();

            this._builderActions.ForEach(ba => ba(this.Book));

            this._builderActions.Clear();

            return this.Book;
        }
    }
}