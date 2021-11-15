using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookAuthorsEditor : IBookEditor
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookAuthorsEditor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task EditBook(Book book, BookData bookData)
        {
            foreach (var guid in bookData.Authors)
            {
                var item = await _unitOfWork.GetAsync<Author>(guid);

                if (!item.IsActive())
                    throw new BusinessRuleNotMetException($"Author with GUID {guid} does not exist");

                var oldSequence = book.Authors.AsEnumerable();
                var newSequence = bookData.Authors.Select(s => new BookAuthor(s)).AsEnumerable();

                var booksToRemove = Distinguisher<BookAuthor>.ToRemove(oldSequence, newSequence).ToList();
                var booksToAdd = Distinguisher<BookAuthor>.ToAdd(oldSequence, newSequence).ToList();

                booksToRemove.ForEach(book.RemoveAuthor);

                booksToAdd.ForEach(book.AddAuthor);
            }
        }
    }
}