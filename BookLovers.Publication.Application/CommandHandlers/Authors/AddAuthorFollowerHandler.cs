using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Shared;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class AddAuthorFollowerHandler : ICommandHandler<AddAuthorFollowerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAuthorFollowerHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddAuthorFollowerInternalCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);

            author.AddFollower(new Follower(command.ReaderGuid));

            await this._unitOfWork.CommitAsync(author);
        }
    }
}