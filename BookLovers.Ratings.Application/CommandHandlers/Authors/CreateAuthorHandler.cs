using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Domain.Authors;

namespace BookLovers.Ratings.Application.CommandHandlers.Authors
{
    internal class CreateAuthorHandler : ICommandHandler<CreateAuthorInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAuthorHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateAuthorInternalCommand command)
        {
            var author = Author.Create(new AuthorIdentification(command.AuthorGuid, command.AuthorId));

            return this._unitOfWork.CommitAsync(author);
        }
    }
}