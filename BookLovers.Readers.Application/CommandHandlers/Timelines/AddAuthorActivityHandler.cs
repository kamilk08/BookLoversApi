using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddAuthorActivityHandler : ICommandHandler<AddAuthorActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAuthorActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddAuthorActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var activity = Activity.InitiallyPublic(
                command.AuthorGuid,
                new ActivityContent("Reader created author", command.AddedAt, ActivityType.AddedAuthor));

            reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}