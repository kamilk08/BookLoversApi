using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.RatingGivers;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Application.CommandHandlers.RatingGivers
{
    internal class RemoveAllRatingsHandler : ICommandHandler<RemoveAllRatingsInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;

        public RemoveAllRatingsHandler(IUnitOfWork unitOfWork, IBookRepository bookRepository)
        {
            this._unitOfWork = unitOfWork;
            this._bookRepository = bookRepository;
        }

        public async Task HandleAsync(RemoveAllRatingsInternalCommand internalCommand)
        {
            foreach (var bookId in internalCommand.BookIds)
            {
                var book = await this._bookRepository.GetByIdAsync(bookId);

                var rating = book.GetReaderRating(internalCommand.ReaderId);

                book.RemoveRating(rating);

                await this._unitOfWork.CommitAsync(book);
            }
        }
    }
}