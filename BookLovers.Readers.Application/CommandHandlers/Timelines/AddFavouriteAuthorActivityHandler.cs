using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddFavouriteAuthorActivityHandler :
        ICommandHandler<AddFavouriteAuthorActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFavouriteAuthorActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddFavouriteAuthorActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var content = new ActivityContent(
                "Reader added favourite author.", DateTime.UtcNow,
                ActivityType.FavouriteAuthor);

            reader.AddToTimeLine(Activity.InitiallyPublic(command.AuthorGuid, content));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}