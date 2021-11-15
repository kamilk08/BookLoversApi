using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddBookActivityHandler : ICommandHandler<AddBookActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddBookActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var activity = Activity.InitiallyPublic(
                command.BookGuid,
                new ActivityContent("Book created by reader.", command.AddedAt, ActivityType.AddedBook));

            reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}