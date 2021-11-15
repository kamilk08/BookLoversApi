using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Shared;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class FollowReaderHandler : ICommandHandler<FollowReaderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public FollowReaderHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(FollowReaderCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.Dto.FollowedGuid);

            reader.AddFollower(new Follower(_contextAccessor.UserGuid));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}