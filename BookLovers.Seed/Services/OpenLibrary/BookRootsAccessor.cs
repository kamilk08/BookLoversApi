using System.Collections.Generic;
using System.Linq;
using BookLovers.Seed.Models.OpenLibrary.Books;

namespace BookLovers.Seed.Services.OpenLibrary
{
    internal class BookRootsAccessor
    {
        private List<BookRoot> _bookRoots;

        public BookRootsAccessor()
        {
            this._bookRoots = new List<BookRoot>();
        }

        public IEnumerable<BookRoot> GetBookRoots()
        {
            return this._bookRoots;
        }

        public void SetBookRoots(IEnumerable<BookRoot> roots)
        {
            if (this._bookRoots.Any())
                this._bookRoots.Clear();

            this._bookRoots = roots.ToList();
        }
    }
}