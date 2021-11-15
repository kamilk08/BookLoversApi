using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddActivityOfTypeAuthorQuoteAddedHandler :
        ICommandHandler<AddActivityOfTypeAuthorQuoteAddedInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddActivityOfTypeAuthorQuoteAddedHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            AddActivityOfTypeAuthorQuoteAddedInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var activity = new Activity(
                command.QuoteGuid,
                new ActivityContent(
                    "Author quote added",
                    DateTime.UtcNow, ActivityType.NewAuthorQuote), true);

            reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}