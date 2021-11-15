using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class UnFollowAuthorHandler : ICommandHandler<UnFollowAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public UnFollowAuthorHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(UnFollowAuthorCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);
            var follower = author.GetFollower(this._contextAccessor.UserGuid);

            author.RemoveFollower(follower);

            await this._unitOfWork.CommitAsync(author);
        }
    }
}