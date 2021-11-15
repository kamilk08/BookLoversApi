using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Domain.Books.Services.Editors;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookEditService : IDomainService<Book>
    {
        private readonly List<IBookEditor> _bookEditors;

        public BookEditService(List<IBookEditor> bookEditors)
        {
            this._bookEditors = bookEditors;
        }

        public Task EditBook(Book book, BookData bookData)
        {
            return Task.WhenAll(
                this._bookEditors.Select(editor => editor.EditBook(book, bookData)));
        }
    }
}