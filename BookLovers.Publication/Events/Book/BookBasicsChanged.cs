using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookBasicsChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public string Isbn { get; private set; }

        public string Title { get; private set; }

        public DateTime PublicationDate { get; private set; }

        public string CategoryName { get; private set; }

        public int CategoryId { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int SubCategoryId { get; private set; }

        public string SubCategoryName { get; private set; }

        private BookBasicsChanged()
        {
        }

        private BookBasicsChanged(
            Guid bookGuid,
            string isbn,
            string title,
            DateTime publicationDate,
            int categoryId,
            string categoryName,
            int subCategoryId,
            string subCategoryName)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.Isbn = isbn;
            this.Title = title;
            this.PublicationDate = publicationDate;
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
            this.SubCategoryId = subCategoryId;
            this.SubCategoryName = subCategoryName;
        }

        public static BookBasicsChanged Initialize()
        {
            return new BookBasicsChanged();
        }

        public BookBasicsChanged WithAggregate(Guid aggregateGuid)
        {
            return new BookBasicsChanged(aggregateGuid, this.Isbn,
                this.Title, this.PublicationDate, this.CategoryId, this.CategoryName, this.SubCategoryId,
                this.SubCategoryName);
        }

        public BookBasicsChanged WithTitleAndIsbn(string title, string isbn)
        {
            return new BookBasicsChanged(
                this.AggregateGuid, isbn, title, this.PublicationDate, this.CategoryId, this.CategoryName,
                this.SubCategoryId, this.SubCategoryName);
        }

        public BookBasicsChanged WithPublished(DateTime publicationDate)
        {
            return new BookBasicsChanged(
                this.AggregateGuid,
                this.Isbn, this.Title, publicationDate, this.CategoryId, this.CategoryName, this.SubCategoryId,
                this.SubCategoryName);
        }

        public BookBasicsChanged WithCategory(int categoryId, string categoryName)
        {
            return new BookBasicsChanged(
                this.AggregateGuid, this.Isbn, this.Title, this.PublicationDate, categoryId, categoryName,
                this.SubCategoryId, this.SubCategoryName);
        }

        public BookBasicsChanged WithSubCategory(
            int subCategoryId,
            string subCategoryName)
        {
            return new BookBasicsChanged(this.AggregateGuid, this.Isbn, this.Title, this.PublicationDate,
                this.CategoryId, this.CategoryName, subCategoryId, subCategoryName);
        }
    }
}