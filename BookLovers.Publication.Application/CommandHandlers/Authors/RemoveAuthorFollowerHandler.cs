using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class RemoveAuthorFollowerHandler : ICommandHandler<RemoveAuthorFollowerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveAuthorFollowerHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveAuthorFollowerInternalCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);
            var follower = author.GetFollower(command.ReaderGuid);

            author.RemoveFollower(follower);

            await this._unitOfWork.CommitAsync(author);
        }
    }
}