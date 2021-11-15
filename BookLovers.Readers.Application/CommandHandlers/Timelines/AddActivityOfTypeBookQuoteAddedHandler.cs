using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddActivityOfTypeBookQuoteAddedHandler :
        ICommandHandler<AddActivityOfTypeBookQuoteAddedInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddActivityOfTypeBookQuoteAddedHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            AddActivityOfTypeBookQuoteAddedInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var activity = Activity.InitiallyPublic(
                command.QuoteGuid,
                new ActivityContent(
                    "Book quote added",
                    DateTime.UtcNow, ActivityType.NewBookQuote));

            reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}