using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Application.CommandHandlers
{
    internal class ChangeRatingHandler : ICommandHandler<ChangeRatingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RatingsService _service;

        public ChangeRatingHandler(IUnitOfWork unitOfWork, RatingsService service)
        {
            this._unitOfWork = unitOfWork;
            this._service = service;
        }

        public async Task HandleAsync(ChangeRatingCommand command)
        {
            var book = await this._service.ChangeBookRatingAsync(command.WriteModel.BookGuid, command.WriteModel.Stars);

            await this._unitOfWork.CommitAsync<Book>(book);
        }
    }
}