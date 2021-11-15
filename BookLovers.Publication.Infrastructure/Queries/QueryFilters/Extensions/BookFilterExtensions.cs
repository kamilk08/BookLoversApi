using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions
{
    internal static class BookFilterExtensions
    {
        internal static IQueryable<BookReadModel> FilterBooksByTitle(this IQueryable<BookReadModel> query, string title)
        {
            return query.WhereIf(
                p => p.Title.ToUpper().StartsWith(title.ToUpper()),
                !title.IsEmpty());
        }

        internal static IQueryable<BookReadModel> FilterBooksByAuthorFullName(
            this IQueryable<BookReadModel> query,
            string fullName)
        {
            return query.WhereIf(
                p => p.Authors.Any(a => a.FullName.ToUpper()
                    .StartsWith(fullName.ToUpper())),
                !fullName.IsEmpty());
        }

        internal static IQueryable<BookReadModel> FilterBooksByIsbn(this IQueryable<BookReadModel> query, string isbn)
        {
            return query.WhereIf(
                p => p.Isbn.ToUpper().StartsWith(isbn.ToUpper()),
                !isbn.IsEmpty());
        }

        internal static IQueryable<BookReadModel> FilterFromDate(this IQueryable<BookReadModel> query, DateTime? from)
        {
            return query.WhereIf(
                p => p.PublicationDate > from,
                from != null);
        }

        internal static IQueryable<BookReadModel> FilterTillDate(this IQueryable<BookReadModel> query, DateTime? till)
        {
            return query.WhereIf(
                p => p.PublicationDate < till,
                till != null);
        }

        internal static IQueryable<BookReadModel> FilterWithCategories(
            this IQueryable<BookReadModel> query,
            IList<int> categoriesIds)
        {
            return query.WhereIf(p => categoriesIds.Any(a => a == p.SubCategoryId), categoriesIds.Any());
        }

        internal static IQueryable<BookReadModel> WithExactTitle(this IQueryable<BookReadModel> query, string title)
        {
            return title != null ? query.Where(p => p.Title.ToUpper() == title.ToUpper()) : query;
        }

        internal static IQueryable<BookReadModel> WithExactIsbn(this IQueryable<BookReadModel> query, string isbn)
        {
            return isbn != null ? query.Where(p => p.Isbn == isbn) : query;
        }

        internal static IQueryable<BookReadModel> FindAuthorBooksStartingWith(
            this IQueryable<BookReadModel> query,
            int authorId, string bookTitle)
        {
            return query.WhereIf(
                p => p.Authors.Any(a => a.Id == authorId) && p.Title.ToUpper().StartsWith(bookTitle.ToUpper()),
                !bookTitle.IsEmpty());
        }
    }
}