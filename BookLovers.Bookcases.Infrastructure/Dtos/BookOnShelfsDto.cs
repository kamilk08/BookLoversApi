using System.Collections.Generic;

namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class BookOnShelfsDto
    {
        /// <summary>
        /// KEY - SHELFID, VALUE - BOOKS_COUNT
        /// </summary>
        public Dictionary<int, int> BookOnShelfs { get; set; }
    }
}