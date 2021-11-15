using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Integration.ApplicationEvents.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.PublisherCycles
{
    internal class AddPublisherCycleBookHandler : ICommandHandler<AddPublisherCycleBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public AddPublisherCycleBookHandler(IUnitOfWork unitOfWork, IInMemoryEventBus inMemoryEventBus)
        {
            this._unitOfWork = unitOfWork;
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public async Task HandleAsync(AddPublisherCycleBookCommand command)
        {
            var cycle = await this._unitOfWork.GetAsync<PublisherCycle>(command.CycleGuid);
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            if (cycle.Guid == Guid.Empty || book.Guid == Guid.Empty)
                throw new BusinessRuleNotMetException("Either cycle or book does not exist");

            cycle.AddBook(new CycleBook(book.Guid));

            await this._unitOfWork.CommitAsync(cycle);

            await this._inMemoryEventBus.Publish(
                new PublisherCycleHasNewBookIntegrationEvent(cycle.Guid, command.BookGuid));
        }
    }
}