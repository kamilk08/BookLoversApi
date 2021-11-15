using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.RatingGivers;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Application.CommandHandlers.RatingGivers
{
    internal class RemoveRatingInternalHandler : ICommandHandler<RemoveRatingInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;
        private readonly IRatingGiverRepository _ratingGiverRepository;

        public RemoveRatingInternalHandler(
            IUnitOfWork unitOfWork,
            IBookRepository bookRepository,
            IRatingGiverRepository ratingGiverRepository)
        {
            this._unitOfWork = unitOfWork;
            this._bookRepository = bookRepository;
            this._ratingGiverRepository = ratingGiverRepository;
        }

        public async Task HandleAsync(RemoveRatingInternalCommand command)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(command.BookGuid);

            var rating = await this._ratingGiverRepository.GetRatingGiverByReaderGuid(command.ReaderGuid);

            if (book.HasRating(rating.ReaderId))
                book.RemoveRating(book.GetReaderRating(rating.ReaderId));

            await this._unitOfWork.CommitAsync<Book>(book);
        }
    }
}