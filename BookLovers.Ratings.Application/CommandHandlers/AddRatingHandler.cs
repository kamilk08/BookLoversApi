using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Application.CommandHandlers
{
    internal class AddRatingHandler : ICommandHandler<AddRatingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RatingsService _ratingsService;

        public AddRatingHandler(IUnitOfWork unitOfWork, RatingsService ratingsService)
        {
            this._unitOfWork = unitOfWork;
            this._ratingsService = ratingsService;
        }

        public async Task HandleAsync(AddRatingCommand command)
        {
            var book = await this._ratingsService.AddRatingToBookAsync(
                command.WriteModel.BookGuid,
                command.WriteModel.Stars);

            await this._unitOfWork.CommitAsync<Book>(book);
        }
    }
}