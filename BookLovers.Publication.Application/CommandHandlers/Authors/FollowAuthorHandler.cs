using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Shared;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class FollowAuthorHandler : ICommandHandler<FollowAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public FollowAuthorHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(FollowAuthorCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);

            author.AddFollower(new Follower(this._contextAccessor.UserGuid));

            await this._unitOfWork.CommitAsync(author);
        }
    }
}