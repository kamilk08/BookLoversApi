using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions
{
    internal static class QueriesFilteringExtensions
    {
        internal static IQueryable<BookcaseReadModel> GetBookcasesWithBook(
            this IQueryable<BookcaseReadModel> query,
            int bookId)
        {
            return query.Where(p => p.Shelves
                .Any(s => s.Books.Any(a => a.BookId == bookId)));
        }

        internal static IQueryable<BookcaseReadModel> GetBookcasesWithMultipleBooks(
            this IQueryable<BookcaseReadModel> query,
            List<int> bookIds)
        {
            return query.Where(p => p.Shelves
                .Any(s => s.Books.Any(a => bookIds.Contains(a.BookId))));
        }

        internal static IQueryable<ShelfReadModel> AllShelvesThatHaveBook(
            this IQueryable<ShelfReadModel> query,
            int bookId)
        {
            return query.Where(p => p.Books
                .Any(a => a.BookId == bookId));
        }

        internal static IQueryable<ShelfRecordReadModel> WithoutCustomShelves(
            this IQueryable<ShelfRecordReadModel> query)
        {
            return query.Where(p => p.Shelf.ShelfCategory
                                    != ShelfCategory.Custom.Value);
        }

        internal static IQueryable<ShelfRecordReadModel> WithBookcase(
            this IQueryable<ShelfRecordReadModel> query,
            int bookcaseId)
        {
            return query.Where(p => p.Shelf.Bookcase.Id == bookcaseId);
        }

        internal static IQueryable<ShelfRecordReadModel> WithBooksOnShelves(
            this IQueryable<ShelfRecordReadModel> query,
            List<int> bookIds)
        {
            return query.WhereIf(p => bookIds.Contains(p.Book.BookId), bookIds != null);
        }

        internal static IQueryable<ShelfReadModel> WithBookcase(
            this IQueryable<ShelfReadModel> query,
            int bookcaseId)
        {
            return query.Where(p => p.Bookcase.Id == bookcaseId);
        }
    }
}