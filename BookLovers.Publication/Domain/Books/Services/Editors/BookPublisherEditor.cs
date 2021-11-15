using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookPublisherEditor : IBookEditor
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookPublisherEditor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task EditBook(Book book, BookData bookDto)
        {
            var publisher = await _unitOfWork.GetAsync<Publisher>(bookDto.PublisherGuid);

            if (!publisher.IsActive())
                throw new BusinessRuleNotMetException($"Publisher with GUID {bookDto.PublisherGuid} does not exist.");

            var bookPublisher = new BookPublisher(bookDto.PublisherGuid);

            if (book.Publisher == bookPublisher)
                return;

            book.ChangePublisher(bookPublisher);
        }
    }
}