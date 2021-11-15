using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Domain.Authors;

namespace BookLovers.Ratings.Application.CommandHandlers.Authors
{
    internal class RecalculateAuthorAverageHandler :
        ICommandHandler<RecalculateAuthorAverageInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorRepository _repository;

        public RecalculateAuthorAverageHandler(IUnitOfWork unitOfWork, IAuthorRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public async Task HandleAsync(
            RecalculateAuthorAverageInternalCommand internalCommand)
        {
            var authors = await this._repository.GetAuthorsWithBookAsync(internalCommand.BookGuid);

            foreach (var aggregate in authors)
                await this._unitOfWork.CommitAsync(aggregate);
        }
    }
}