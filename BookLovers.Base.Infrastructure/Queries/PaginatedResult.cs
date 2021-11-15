using System;
using System.Collections.Generic;

namespace BookLovers.Base.Infrastructure.Queries
{
    public abstract class PaginatedResult
    {
        public static readonly int DefaultItemsPerPage = 10;
        public static readonly int DefaultPage = 0;
    }

    public class PaginatedResult<T> : PaginatedResult
    {
        public IList<T> Items { get; set; }

        public int Page { get; set; }

        public int PagesCount { get; set; }

        public int TotalItems { get; set; }

        private PaginatedResult()
        {
        }

        public PaginatedResult(int page)
        {
            Items = new List<T>();
            PagesCount = 0;
            Page = page;
        }

        public PaginatedResult(IList<T> items, int page, int itemsPerPage)
        {
            Items = items;
            Page = page;
            PagesCount = CountPages(itemsPerPage);
        }

        public PaginatedResult(IList<T> items, int page, int itemsPerPage, int totalItems)
        {
            Items = items;
            Page = page;
            TotalItems = totalItems;
            PagesCount = CountPages(itemsPerPage);
        }

        private int CountPages(int itemsPerPage)
        {
            return (int) Math.Ceiling(TotalItems / (double) itemsPerPage);
        }
    }
}