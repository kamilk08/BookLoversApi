using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddFavouriteBookActivityHandler :
        ICommandHandler<AddFavouriteBookActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFavouriteBookActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddFavouriteBookActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var content = new ActivityContent("Reader added favourite book", DateTime.UtcNow,
                ActivityType.FavouriteBook);

            reader.AddToTimeLine(Activity.InitiallyPublic(command.BookGuid, content));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}