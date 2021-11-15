using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class UnFollowReaderHandler : ICommandHandler<UnFollowReaderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public UnFollowReaderHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(UnFollowReaderCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.Dto.FollowedGuid);
            var follower = reader.GetFollower(_contextAccessor.UserGuid);

            reader.RemoveFollower(follower);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}